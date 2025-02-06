namespace Plugin.Maui.MarkdownView.ViewSuppliers;

public class MauiFormattedTextViewSupplierStyles
{
    /// <summary>
    /// Span color for Code background
    /// </summary>
    public Color? SpanCodeBackgroundLightColor { get; set; }

    /// <summary>
    /// Span color for Code background
    /// </summary>
    public Color? SpanCodeBackgroundDarkColor { get; set; }

    /// <summary>
    /// Span color for text in Hyperlink
    /// </summary>
    public Color? SpanHyperlinkTextLightColor { get; set; }

    /// <summary>
    /// Span color for text in Hyperlink
    /// </summary>
    public Color? SpanHyperlinkTextDarkColor { get; set; }

    /// <summary>
    /// Span style for default Span
    /// </summary>
    public Style? SpanTextViewStyle { get; set; }

    /// <summary>
    /// Span style for TextView in List
    /// </summary>
    public Style? SpanListTextViewStyle { get; set; }

    /// <summary>
    /// Span style for TextView in Blockquotes
    /// </summary>
    public Style? SpanBlockquotesTextViewStyle { get; set; }

    /// <summary>
    /// VerticalStackLayout style for Views when broken by designated placeholder
    /// </summary>
    public Style? LayoutForSplitTextViewStyle { get; set; }
}