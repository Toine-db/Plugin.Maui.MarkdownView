using Microsoft.Extensions.Logging;

namespace Plugin.Maui.MarkdownView.Common;

public static class ServiceHelper
{
    public static ILogger<MarkdownView>? GetLogger(this Element? element)
    {
        return GetService<ILogger<MarkdownView>>(element);
    }

    public static T? GetService<T>(Element? element) where T : class
    {
        return IPlatformApplication.Current?.Services.GetService<T>();
    }
}