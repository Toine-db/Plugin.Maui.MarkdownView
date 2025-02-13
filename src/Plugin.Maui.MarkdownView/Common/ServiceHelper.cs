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
}