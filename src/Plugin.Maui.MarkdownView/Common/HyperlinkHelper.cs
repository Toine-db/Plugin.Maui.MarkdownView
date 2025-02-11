using Microsoft.Extensions.Logging;

using Plugin.Maui.MarkdownView.Controls;

namespace Plugin.Maui.MarkdownView.Common;

public static class HyperlinkHelper
{
    public static bool IsHeadingLink(string hyperlink)
    {
        return !string.IsNullOrWhiteSpace(hyperlink)
            && hyperlink.StartsWith('#');
    }

    public static async Task<bool> TryScrollToHeading(Element hyperlinkControl, string hyperlink)
    {
        if (!IsHeadingLink(hyperlink))
        {
            return false;
        }

        try
        {
            var parent = hyperlinkControl.Parent;
            IView? linkedHeader = null;

            // Search for Header with HeadingId
            while (linkedHeader is null
                && parent is not null)
            {
                if (parent is MarkdownView markdownView)
                {
                    linkedHeader = markdownView
                        .GetRootChildren()
                        .OfType<HeaderLabel>()
                        .FirstOrDefault(x => hyperlink.Equals(x.HeadingId, StringComparison.OrdinalIgnoreCase));
                }

                parent = parent.Parent;
            }

            if (linkedHeader is null)
            {
                return false;
            }

            // Search ScrollView and scroll to Header
            while (parent is not null)
            {
                if (parent is ScrollView scrollView)
                {
                    await scrollView.ScrollToAsync(linkedHeader as Element, ScrollToPosition.Start, true);

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