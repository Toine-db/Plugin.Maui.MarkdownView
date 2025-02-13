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
	private bool isWaitingToDisplayViewsFromMarkdown = false;
	private CancellationTokenSource? _loadingCts;

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
			markdownView.InvalidateMarkdownAsync();
		}
	}

	private static void OnViewSupplierChanged(BindableObject bindable, object oldvalue, object newvalue)
	{
		if (bindable is MarkdownView markdownView)
		{
			markdownView.SyncIgnoreSafeAreaSettingToSupplier();

			if (!string.IsNullOrWhiteSpace(markdownView.MarkdownText))
			{
				markdownView.InvalidateMarkdownAsync();
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

	/// <summary>
	/// Invalidate current displayed views generated from Markdown,
	/// starting new view creation cycle by forcing one cycle at a time, restarting each time markdown is invalidated
	/// with maximum 1 waiting request in line
	/// </summary>
	private async void InvalidateMarkdownAsync()
	{
		if (isWaitingToDisplayViewsFromMarkdown)
		{
			return;
		}
		isWaitingToDisplayViewsFromMarkdown = true;

		CancelDisplayViewsFromMarkdown();

		await LoadingSemaphore.WaitAsync();
		isWaitingToDisplayViewsFromMarkdown = false;

		var loadingCts = new CancellationTokenSource();
		_loadingCts = loadingCts;

		var isCanceled = false;

		try
		{
			await DisplayViewsFromMarkdownAsync(MarkdownText, loadingCts.Token).ConfigureAwait(false);
		}
		catch (OperationCanceledException)
		{
			isCanceled = true;
		}
		finally
		{
			_loadingCts?.Dispose();
			_loadingCts = null;

			LoadingSemaphore.Release();
		}

		if (isCanceled)
		{
			InvalidateMarkdownAsync();
		}
	}

	protected async Task DisplayViewsFromMarkdownAsync(string markdownText, CancellationToken loadingToken)
	{
		if (string.IsNullOrWhiteSpace(MarkdownText))
		{
			await Dispatcher.DispatchAsync(_container.Clear);
			IsLoadingMarkdown = false;

			return;
		}

		IsLoadingMarkdown = true;

		var uiComponentSupplier = ViewSupplier != null
			? ViewSupplier
			: new MauiBasicViewSupplier();

		var markdownParser = new MarkdownParser<View>(uiComponentSupplier);

		await Dispatcher.DispatchAsync(() =>
		{
			var views = markdownParser.Parse(markdownText);
			loadingToken.ThrowIfCancellationRequested();

			_container.Clear();
			foreach (var view in views)
			{
				loadingToken.ThrowIfCancellationRequested();
				_container.Add(view);
			}

			_logger?.Log(LogLevel.Trace, "Markdown used > {markdownText}", markdownText);
			_logger?.Log(LogLevel.Information, "{viewCount} top-level views created", views.Count);
		});

		IsLoadingMarkdown = false;
	}

	private void CancelDisplayViewsFromMarkdown()
	{
		if (_loadingCts == null)
		{
			return;
		}

		_loadingCts?.Cancel();
		_loadingCts?.Dispose();
		_loadingCts = null;
	}
}