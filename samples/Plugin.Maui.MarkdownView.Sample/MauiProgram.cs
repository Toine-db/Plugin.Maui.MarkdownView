using Microsoft.Extensions.Logging;

namespace Plugin.Maui.MarkdownView.Sample;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("FontAwesomeRegular.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesomeBrands.otf", "FontAwesomeBrands");
                fonts.AddFont("FontAwesomeSolid.otf", "FontAwesomeSolid");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
