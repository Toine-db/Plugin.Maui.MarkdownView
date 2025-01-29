namespace Plugin.Maui.MarkdownView.ViewSuppliers;

public interface IMauiBasicViewSupplierStyles
{
    /// <summary>
    /// VerticalStackLayout style for stacking all Views
    /// </summary>
    Style? LayoutViewStyle { get; set; }

    /// <summary>
    /// Label style for default TextView
    /// </summary>
    Style? TextViewStyle { get; set; }

    /// <summary>
    /// Label style for TextView in List
    /// </summary>
    Style? ListTextViewStyle { get; set; }

    /// <summary>
    /// Label style for TextView in Blockquotes
    /// </summary>
    Style? BlockquotesTextViewStyle { get; set; }

    /// <summary>
    /// Label style for HeaderView level 1
    /// </summary>
    Style? HeaderViewLevel1Style { get; set; }

    /// <summary>
    /// Label style for HeaderView level 2
    /// </summary>
    Style? HeaderViewLevel2Style { get; set; }

    /// <summary>
    /// Label style for HeaderView level 3
    /// </summary>
    Style? HeaderViewLevel3Style { get; set; }

    /// <summary>
    /// Label style for HeaderView level 4
    /// </summary>
    Style? HeaderViewLevel4Style { get; set; }

    /// <summary>
    /// Label style for HeaderView level 5
    /// </summary>
    Style? HeaderViewLevel5Style { get; set; }

    /// <summary>
    /// Label style for HeaderView level 6
    /// </summary>
    Style? HeaderViewLevel6Style { get; set; }

    /// <summary>
    /// Grid style for BlockquotesView outer layout
    /// </summary>
    Style? BlockquotesOuterViewStyle { get; set; }

    /// <summary>
    /// Grid style for BlockquotesView inner layout
    /// </summary>
    Style? BlockquotesInnerViewStyle { get; set; }

    /// <summary>
    /// Border layout style for FencedCodeBlock
    /// </summary>
    Style? FencedCodeBlockLayoutStyle { get; set; }

    /// <summary>
    /// Label style for FencedCodeBlock
    /// </summary>
    Style? FencedCodeBlockLabelStyle { get; set; }

    /// <summary>
    /// Border layout style for IndentedCodeBlock
    /// </summary>
    Style? IndentedCodeBlockLayoutStyle { get; set; }

    /// <summary>
    /// Label style for IndentedCodeBlock
    /// </summary>
    Style? IndentedCodeBlockLabelStyle { get; set; }

    /// <summary>
    /// VerticalStackLayout style for OrderedList
    /// </summary>
    Style? ListViewStyle { get; set; }

    /// <summary>
    /// HorizontalStackLayout style for ListItemView
    /// </summary>
    Style? ListItemViewStyle { get; set; }

    /// <summary>
    /// Label style for the list item bullet level 1
    /// </summary>
    Style? ListItemBulletViewLevel1Style { get; set; }

    /// <summary>
    /// Label style for the list item bullet level 2
    /// </summary>
    Style? ListItemBulletViewLevel2Style { get; set; }

    /// <summary>
    /// Label style for the list item bullet level 3
    /// </summary>
    Style? ListItemBulletViewLevel3Style { get; set; }

    /// <summary>
    /// Label style for the list item bullet level 4
    /// </summary>
    Style? ListItemBulletViewLevel4Style { get; set; }

    /// <summary>
    /// Label style for the list item bullet level 5
    /// </summary>
    Style? ListItemBulletViewLevel5Style { get; set; }

    /// <summary>
    /// Label style for the list item bullet level 6
    /// </summary>
    Style? ListItemBulletViewLevel6Style { get; set; }

    /// <summary>
    /// Image style for the image in the ImageView
    /// </summary>
    Style? ImageViewStyle { get; set; }

    /// <summary>
    /// VerticalStackLayout style for stacklayout of image and subcription
    /// </summary>
    Style? ImageLayoutViewStyle { get; set; }

    /// <summary>
    /// Label style for the subscription in the ImageView
    /// </summary>
    Style? ImageSubscriptionViewStyle { get; set; }

    /// <summary>
    /// Grid layout style for HtmlBlock
    /// </summary>
    Style? HtmlBlockLayoutStyle { get; set; }

    /// <summary>
    /// Label style for HtmlBlock
    /// </summary>
    Style? HtmlBlockLabelStyle { get; set; }

    /// <summary>
    /// BoxView style for ThematicBreak
    /// </summary>
    Style? ThematicBreakStyle { get; set; }
}