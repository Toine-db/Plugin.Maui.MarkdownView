using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Resources.Styles;

internal class DefaultMauiFormattedTextViewSupplierStyles : MauiFormattedTextViewSupplierStyles
{
	public DefaultMauiFormattedTextViewSupplierStyles()
    {
        DefaultMauiBasicViewSupplierStyles.EnsureStyleDictionaryIsLoaded();
        LoadDefaultFormattedTextStylesIn(this);
	}

    public static void LoadDefaultFormattedTextStylesIn(MauiFormattedTextViewSupplierStyles styles)
	{
		var resources = Application.Current!.Resources;
		styles.SpanTextViewStyle = (Style)resources["DefaultMarkdownSpanTextViewStyle"];
		styles.SpanListTextViewStyle = (Style)resources["DefaultMarkdownSpanListTextViewStyle"];
		styles.SpanBlockquotesTextViewStyle = (Style)resources["DefaultMarkdownSpanBlockquotesTextViewStyle"];
		styles.SpanHyperlinkTextLightColor = (Color)resources["DefaultMarkdownSpanHyperlinkTextColorLight"];
		styles.SpanHyperlinkTextDarkColor = (Color)resources["DefaultMarkdownSpanHyperlinkTextColorDark"];
		styles.SpanCodeBackgroundLightColor = (Color)resources["DefaultMarkdownSpanCodeBackgroundColorLight"];
		styles.SpanCodeBackgroundDarkColor = (Color)resources["DefaultMarkdownSpanCodeBackgroundColorDark"];
		styles.LayoutForSplitTextViewStyle = (Style)resources["DefaultMarkdownLayoutForSplitTextViewStyle"];
	}
}