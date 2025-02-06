using Plugin.Maui.MarkdownView.Sample.Pages;

namespace Plugin.Maui.MarkdownView.Sample;

public partial class AppShell : Shell
{
    private bool _hasShownFlyoutMenu;

    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(MarkdownFullExampleDetailPage), typeof(MarkdownFullExampleDetailPage));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // prevent HotReload from showing flyout menu again and again and again...
        if (!_hasShownFlyoutMenu)
        {
            ShowFlyoutMenu();
        }
    }

    private void ShowFlyoutMenu()
    {
        _hasShownFlyoutMenu = true;
        Dispatcher.Dispatch(async () =>
        {
            await Task.Delay(1000);
            FlyoutIsPresented = true;
        });
    }
}
