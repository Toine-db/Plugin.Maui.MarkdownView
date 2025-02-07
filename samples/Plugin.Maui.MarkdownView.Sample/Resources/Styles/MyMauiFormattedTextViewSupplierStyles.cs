using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Sample.Resources.Styles;

public class MyMauiFormattedTextViewSupplierStyles : MauiFormattedTextViewSupplierStyles
{
    public MyMauiFormattedTextViewSupplierStyles()
    {
        LoadDefaultFormattedTextStylesIn(this);
    }

    public static void LoadDefaultFormattedTextStylesIn(MauiFormattedTextViewSupplierStyles styles)
    {
        var resources = Application.Current!.Resources;
        styles.SpanTextViewStyle = (Style)resources["ExampleMarkdownSpanTextViewStyle"];
        styles.SpanListTextViewStyle = (Style)resources["ExampleMarkdownSpanListTextViewStyle"];
        styles.SpanBlockquotesTextViewStyle = (Style)resources["ExampleMarkdownSpanBlockquotesTextViewStyle"];
        styles.SpanHyperlinkTextLightColor = (Color)resources["ExampleMarkdownSpanHyperlinkTextColorLight"];
        styles.SpanHyperlinkTextDarkColor = (Color)resources["ExampleMarkdownSpanHyperlinkTextColorDark"];
        styles.SpanCodeBackgroundLightColor = (Color)resources["ExampleMarkdownSpanCodeBackgroundColorLight"];
        styles.SpanCodeBackgroundDarkColor = (Color)resources["ExampleMarkdownSpanCodeBackgroundColorDark"];
        styles.LayoutForSplitTextViewStyle = (Style)resources["ExampleMarkdownLayoutForSplitTextViewStyle"];
    }
}