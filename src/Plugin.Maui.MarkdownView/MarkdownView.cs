using MarkdownParser;
using Microsoft.Extensions.Logging;
using Plugin.Maui.MarkdownView.Common;
using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView;

[ContentProperty(nameof(MarkdownText))]
public class MarkdownView : ContentView
{
	private readonly ILogger<MarkdownView>? _logger;
	private static readonly SemaphoreSlim LoadingSemaphore = new(1, 1);
	private CancellationTokenSource? _loadingCts;

	private bool _isWaitingRenderViewsAsynchronous;

	private readonly VerticalStackLayout _container;

	public MarkdownView()
	{
		_container = new VerticalStackLayout
		{
			IgnoreSafeArea = this.IgnoreSafeArea,
		};
		Content = _container;

		SyncIgnoreSafeAreaSettingToSupplier();

		_logger ??= this.GetLogger();
	}

	public event EventHandler<MarkdownParseExceptionEventArgs> MarkdownParseExceptionThrown;

	/// <summary>
	/// Workaround: space (lowercase) required for using xml:space="preserve" in XAML to keep linebreaks and spaces during HotReload
	/// </summary>
	public static readonly BindableProperty SpaceProperty = BindableProperty.Create(nameof(space), typeof(string), typeof(MarkdownView), "preserve");
	public string space
	{
		get => (string)GetValue(SpaceProperty);
		set => SetValue(SpaceProperty, value);
	}

	public static readonly BindableProperty MarkdownTextProperty =
		BindableProperty.Create(nameof(MarkdownText), typeof(string), typeof(MarkdownView), string.Empty, propertyChanged: MarkdownTextPropertyChanged);

	public string MarkdownText
	{
		get => (string)GetValue(MarkdownTextProperty);
		set => SetValue(MarkdownTextProperty, value);
	}

	public static readonly BindableProperty IsLoadingMarkdownProperty =
		BindableProperty.Create(nameof(IsLoadingMarkdown), typeof(bool), typeof(MarkdownView), false, BindingMode.OneWayToSource);

	public bool IsLoadingMarkdown
	{
		get => (bool)GetValue(IsLoadingMarkdownProperty);
		private set => SetValue(IsLoadingMarkdownProperty, value);
	}

	public static readonly BindableProperty RenderSynchronouslyProperty =
		BindableProperty.Create(nameof(RenderSynchronously), typeof(bool), typeof(MarkdownView), false);

	public bool RenderSynchronously
	{
		get => (bool)GetValue(RenderSynchronouslyProperty);
		set => SetValue(RenderSynchronouslyProperty, value);
	}

	public static readonly BindableProperty MaskParseExceptionsProperty =
		BindableProperty.Create(nameof(MaskParseExceptions), typeof(bool), typeof(MarkdownView), true);

	public bool MaskParseExceptions
	{
		get => (bool)GetValue(MaskParseExceptionsProperty);
		set => SetValue(MaskParseExceptionsProperty, value);
	}

	public static readonly BindableProperty IgnoreSafeAreaProperty =
		BindableProperty.Create(nameof(IgnoreSafeArea), typeof(bool), typeof(MarkdownView), false, propertyChanged: OnIgnoreSafeAreaChanged);

	public bool IgnoreSafeArea
	{
		get => (bool)GetValue(IgnoreSafeAreaProperty);
		set => SetValue(IgnoreSafeAreaProperty, value);
	}

	public static readonly BindableProperty ViewSupplierProperty =
		BindableProperty.Create(nameof(ViewSupplier), typeof(IMauiViewSupplier), typeof(MarkdownView), null, BindingMode.TwoWay, propertyChanged: OnViewSupplierChanged);

	public IMauiViewSupplier? ViewSupplier
	{
		get => (IMauiViewSupplier?)GetValue(ViewSupplierProperty);
		set => SetValue(ViewSupplierProperty, value);
	}

	public static readonly BindableProperty InnerStackLayoutStyleProperty =
		BindableProperty.Create(nameof(InnerStackLayoutStyle), typeof(Style), typeof(MarkdownView), default(Style), propertyChanged: InnerStackLayoutStyleChanged);

	public Style InnerStackLayoutStyle
	{
		get => (Style)GetValue(InnerStackLayoutStyleProperty);
		set => SetValue(InnerStackLayoutStyleProperty, value);
	}

	public IView[] GetRootChildren()
	{
		var rootChildren = _container.Children.ToArray();
		return rootChildren;
	}

	private static void InnerStackLayoutStyleChanged(BindableObject bindable, object oldvalue, object newvalue)
	{
		if (bindable is MarkdownView markdownView)
		{
			markdownView._container.Style = (Style)newvalue;
		}
	}

	private static void MarkdownTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is MarkdownView markdownView)
		{
			markdownView.RefreshMarkdown();
		}
	}

	private static void OnViewSupplierChanged(BindableObject bindable, object oldvalue, object newvalue)
	{
		if (bindable is MarkdownView markdownView)
		{
			markdownView.SyncIgnoreSafeAreaSettingToSupplier();

			if (!string.IsNullOrWhiteSpace(markdownView.MarkdownText))
			{
				markdownView.RefreshMarkdown();
			}
		}
	}

	private static void OnIgnoreSafeAreaChanged(BindableObject bindable, object oldvalue, object newvalue)
	{
		if (bindable is MarkdownView markdownView)
		{
			markdownView._container.IgnoreSafeArea = (bool)newvalue;
			markdownView.SyncIgnoreSafeAreaSettingToSupplier();
		}
	}

	protected void SyncIgnoreSafeAreaSettingToSupplier()
	{
		if (ViewSupplier != null)
		{
			ViewSupplier.IgnoreSafeArea = IgnoreSafeArea;
		}
	}

	public void RefreshMarkdown()
	{
		if (RenderSynchronously)
		{
			_logger?.Log(LogLevel.Trace, "Refreshing Markdown synchronously");
			RenderViewsFromMarkdown(MarkdownText);
		}
		else
		{
			_logger?.Log(LogLevel.Trace, "Refreshing Markdown asynchronously");
			RequestToRenderViewsFromMarkdownAsync();
		}
	}

	//
	// Synchronous Rendering
	//

	protected void RenderViewsFromMarkdown(string markdownText)
	{
		if (string.IsNullOrWhiteSpace(MarkdownText))
		{
			_container.Clear();
			IsLoadingMarkdown = false;

			return;
		}

		IsLoadingMarkdown = true;

		var uiComponentSupplier = ViewSupplier != null
			? ViewSupplier
			: new MauiBasicViewSupplier();

		try
		{
			var markdownParser = new MarkdownParser<View>(uiComponentSupplier);
			ParseMarkdownAndAddToContainer(markdownParser, markdownText, null);
		}
		catch (Exception exception)
		{
			_logger?.Log(LogLevel.Error, "RenderViewsFromMarkdown exception: {exception}", exception);
			MarkdownParseExceptionThrown?.Invoke(this, new MarkdownParseExceptionEventArgs(exception));

			if (!MaskParseExceptions)
			{
				throw;
			}
		}

		IsLoadingMarkdown = false;
	}

	protected void ParseMarkdownAndAddToContainer(
		MarkdownParser<View> markdownParser, 
		string markdownText, 
		CancellationToken? loadingToken)
	{
		var views = markdownParser.Parse(markdownText);
		loadingToken?.ThrowIfCancellationRequested();

		_container.Clear();
		foreach (var view in views)
		{
			loadingToken?.ThrowIfCancellationRequested();
			_container.Add(view);
		}

		_logger?.Log(LogLevel.Trace, "Markdown used > {markdownText}", markdownText);
		_logger?.Log(LogLevel.Information, "{viewCount} top-level views created", views.Count);
	}

	//
	// Asynchronous Rendering
	//

	protected async void RequestToRenderViewsFromMarkdownAsync()
	{
		// Trigger new async start for view creation cycle by allowing one cycle at a time
		// and with maximum 1 trigger/waiting request in queue

		if (_isWaitingRenderViewsAsynchronous)
		{
			_logger?.Log(LogLevel.Trace, "RenderViewsFromMarkdownAsync is skipping request because another thread is already waiting");
			return;
		}
		_isWaitingRenderViewsAsynchronous = true;

		if (_loadingCts != null)
		{
			_loadingCts?.Cancel();
			_loadingCts = null;
		}

		await LoadingSemaphore.WaitAsync();
		_isWaitingRenderViewsAsynchronous = false;

		var loadingCts = new CancellationTokenSource();
		_loadingCts = loadingCts;

		try
		{
			await RenderViewsFromMarkdownAsync(MarkdownText, loadingCts.Token).ConfigureAwait(false);
		}
		catch (OperationCanceledException)
		{
			_logger?.Log(LogLevel.Trace, "RenderViewsFromMarkdownAsync canceled");
		}
		catch (Exception exception)
		{
			_logger?.Log(LogLevel.Error, "RenderViewsFromMarkdownAsync exception: {exception}", exception);
			MarkdownParseExceptionThrown?.Invoke(this, new MarkdownParseExceptionEventArgs(exception));

			if (!MaskParseExceptions)
			{
				throw;
			}
		}
		finally
		{
			loadingCts?.Dispose();
			_loadingCts = null;

			LoadingSemaphore.Release();
		}
	}

	protected async Task RenderViewsFromMarkdownAsync(string markdownText, CancellationToken loadingToken)
	{
		if (string.IsNullOrWhiteSpace(MarkdownText))
		{
			await Dispatcher.DispatchAsync(_container.Clear);
			IsLoadingMarkdown = false;

			return;
		}

		IsLoadingMarkdown = true;

		await Task.Run(async () =>
		{
			var uiComponentSupplier = ViewSupplier != null
				? ViewSupplier
				: new MauiBasicViewSupplier();

			var markdownParser = new MarkdownParser<View>(uiComponentSupplier);

			await Dispatcher.DispatchAsync(() =>
			{
				ParseMarkdownAndAddToContainer(markdownParser, markdownText, loadingToken);
			});
		}, loadingToken);

		IsLoadingMarkdown = false;
	}
}