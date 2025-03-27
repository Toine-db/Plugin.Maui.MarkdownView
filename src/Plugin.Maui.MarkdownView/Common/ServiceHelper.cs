using Microsoft.Extensions.Logging;

namespace Plugin.Maui.MarkdownView.Common;

public static class ServiceHelper
{
	public static ILogger<MarkdownView>? GetLogger(this Element? element)
	{
		var logger = GetService<ILogger<MarkdownView>>(element);
		return logger;
	}

    public static T? GetService<T>(Element? element) where T : class
	{
		var service = Application.Current?.Handler?.MauiContext?.Services.GetService(typeof(T));
		return service as T;
	}

    public static ILogger<T>? GetLogger<T>() where T : class
    {
        var logger = Application.Current?.Handler?.MauiContext?.Services.GetService(typeof(ILogger<T>));
        return logger as ILogger<T>;
    }
}