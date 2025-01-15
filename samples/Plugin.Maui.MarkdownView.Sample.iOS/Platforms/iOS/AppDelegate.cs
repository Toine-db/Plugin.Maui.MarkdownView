using Foundation;

namespace Plugin.Maui.MarkdownView.Sample.iOS;
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => Plugin.Maui.MarkdownView.Sample.MauiProgram.CreateMauiApp();
}
