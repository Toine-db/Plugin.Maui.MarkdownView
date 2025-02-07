using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Sample.Resources.Styles;

public class DefaultMauiFormattedTextViewSupplierStyles : MauiFormattedTextViewSupplierStyles
{
    public DefaultMauiFormattedTextViewSupplierStyles()
    {
        LoadDefaultFormattedTextStylesIn(this);
    }

    public static void LoadDefaultFormattedTextStylesIn(MauiFormattedTextViewSupplierStyles styles)
    {
        var resources = Application.Current!.Resources;
        styles.SpanTextViewStyle = (Style)resources["MarkdownSpanTextViewStyle"];
        styles.SpanListTextViewStyle = (Style)resources["MarkdownSpanListTextViewStyle"];
        styles.SpanBlockquotesTextViewStyle = (Style)resources["MarkdownSpanBlockquotesTextViewStyle"];
        styles.SpanHyperlinkTextLightColor = (Color)resources["MarkdownSpanHyperlinkTextColorLight"];
        styles.SpanHyperlinkTextDarkColor = (Color)resources["MarkdownSpanHyperlinkTextColorDark"];
        styles.SpanCodeBackgroundLightColor = (Color)resources["MarkdownSpanCodeBackgroundColorLight"];
        styles.SpanCodeBackgroundDarkColor = (Color)resources["MarkdownSpanCodeBackgroundColorDark"];
        styles.LayoutForSplitTextViewStyle = (Style)resources["MarkdownLayoutForSplitTextViewStyle"];
    }
}