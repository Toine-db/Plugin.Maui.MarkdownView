using Microsoft.Extensions.Logging;
using Plugin.Maui.MarkdownView.Controls;

namespace Plugin.Maui.MarkdownView.Common;

public static class HyperlinkHelper
{
	public static bool IsHeadingLink(string hyperlink)
	{
		var isHeadingLink = !string.IsNullOrWhiteSpace(hyperlink)
		                    && hyperlink.StartsWith("#");

		return isHeadingLink;
	}

	public static async Task<bool> TryScrollToHeading(Element hyperlinkControl, string hyperlink)
	{
		if (!IsHeadingLink(hyperlink))
		{
			return false;
		}

		var internalLink = hyperlink.Replace("#", "");
		internalLink = internalLink.Replace(" ", "").Trim();

		try
		{
			var parent = hyperlinkControl.Parent;
			IView? linkedHeader = null;

			// Search for Header with HeadingId
			while (linkedHeader == null
			       && parent != null)
			{
				if (parent is MarkdownView markdownView)
				{
					var rootChildren = markdownView.GetRootChildren();
					linkedHeader = rootChildren
						.Where(x => x is HeaderLabel)
						.Cast<HeaderLabel>()
						.FirstOrDefault(x => x.HeadingId?.ToLower() == internalLink.ToLower());
				}

				parent = parent.Parent;
			}

			if (linkedHeader == null)
			{
				return false;
			}

			// Search ScrollView and scroll to Header
			while (parent != null)
			{
				if (parent is ScrollView scrollView)
				{
					var headerElement = linkedHeader as Element;
					await scrollView.ScrollToAsync(headerElement, ScrollToPosition.Start, true);

					return true;
				}

				parent = parent.Parent;
			}
		}
		catch (Exception exception)
		{
			var logger = hyperlinkControl.GetLogger();
			logger?.Log(LogLevel.Error, "TryScrollToHeading fault: {exception}", exception);
		}

		return false;
	}
}