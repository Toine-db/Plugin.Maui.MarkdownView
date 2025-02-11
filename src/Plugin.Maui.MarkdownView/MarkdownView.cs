using System.Runtime.CompilerServices;

using MarkdownParser;

using Microsoft.Extensions.Logging;

using Plugin.Maui.MarkdownView.Common;
using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView;

[ContentProperty(nameof(MarkdownText))]
public class MarkdownView : ContentView
{
    protected readonly ILogger<MarkdownView>? Logger;
    protected readonly VerticalStackLayout Container;
    protected CancellationTokenSource? LoadingCts;

    public MarkdownView()
    {
        Container = new VerticalStackLayout
        {
            IgnoreSafeArea = this.IgnoreSafeArea,
        };
        Content = Container;

        SyncIgnoreSafeAreaSettingToSupplier();

        Logger ??= this.GetLogger();
    }

    protected override void OnPropertyChanging([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanging(propertyName);

        if (propertyName == MarkdownTextProperty.PropertyName)
        {
            OnMarkdownTextChanging();
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == MarkdownTextProperty.PropertyName)
        {
            OnMarkdownTextChanged();
        }
        else if (propertyName == InnerStackLayoutStyleProperty.PropertyName)
        {
            Container.Style = InnerStackLayoutStyle;
        }
        else if (propertyName == ViewSupplierProperty.PropertyName)
        {
            SyncIgnoreSafeAreaSettingToSupplier();
            OnMarkdownTextChanging();
            OnMarkdownTextChanged();
        }
        else if (propertyName == IgnoreSafeAreaProperty.PropertyName)
        {
            Container.IgnoreSafeArea = IgnoreSafeArea;
            SyncIgnoreSafeAreaSettingToSupplier();
        }
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
        BindableProperty.Create(nameof(MarkdownText), typeof(string), typeof(MarkdownView));

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
        BindableProperty.Create(nameof(IgnoreSafeArea), typeof(bool), typeof(MarkdownView));

    public bool IgnoreSafeArea
    {
        get => (bool)GetValue(IgnoreSafeAreaProperty);
        set => SetValue(IgnoreSafeAreaProperty, value);
    }

    public static readonly BindableProperty ViewSupplierProperty =
        BindableProperty.Create(nameof(ViewSupplier), typeof(IMauiViewSupplier), typeof(MarkdownView), null, BindingMode.TwoWay);

    public IMauiViewSupplier? ViewSupplier
    {
        get => (IMauiViewSupplier?)GetValue(ViewSupplierProperty);
        set => SetValue(ViewSupplierProperty, value);
    }

    public static readonly BindableProperty InnerStackLayoutStyleProperty =
        BindableProperty.Create(nameof(InnerStackLayoutStyle), typeof(Style), typeof(MarkdownView));

    public Style InnerStackLayoutStyle
    {
        get => (Style)GetValue(InnerStackLayoutStyleProperty);
        set => SetValue(InnerStackLayoutStyleProperty, value);
    }

    public IView[] GetRootChildren()
    {
        return Container.Children.ToArray();
    }

    protected virtual void SyncIgnoreSafeAreaSettingToSupplier()
    {
        if (ViewSupplier is not null)
        {
            ViewSupplier.IgnoreSafeArea = IgnoreSafeArea;
        }
    }

    protected virtual void OnMarkdownTextChanging()
    {
        try
        {
            LoadingCts?.Cancel();
            LoadingCts?.Dispose();
            LoadingCts = null;
        }
        catch { }

        Container.Clear();
    }

    protected virtual void OnMarkdownTextChanged()
    {
        if (string.IsNullOrEmpty(MarkdownText)) return;

        LoadingCts = new();

        var token = LoadingCts.Token;

        try
        {
            IsLoadingMarkdown = true;

            Render(MarkdownText, token);
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, $"An error occured during parsing of markdown text");
        }
        finally
        {
            IsLoadingMarkdown = false;
        }
    }

    protected virtual
#if DEBUG
        async
#endif
        void Render(string text, CancellationToken token = default)
    {
        var uiComponentSupplier = ViewSupplier ?? new MauiBasicViewSupplier();

        MarkdownParser<View> markdownParser = new(uiComponentSupplier);

#if DEBUG
        // only for hot reload purpose
        await Dispatcher.DispatchAsync(() =>
        {
#endif
            if (token.IsCancellationRequested) return;

            var views = markdownParser.Parse(text);

            if (token.IsCancellationRequested) return;

            foreach (var view in views)
            {
                if (token.IsCancellationRequested) return;

                Container.Add(view);
            }

            uiComponentSupplier.Clear();

            Logger?.Log(LogLevel.Trace, "Markdown used > {markdownText}", text);
            Logger?.Log(LogLevel.Information, "{viewCount} top-level views created", views.Count);
#if DEBUG
        });
#endif
    }
}