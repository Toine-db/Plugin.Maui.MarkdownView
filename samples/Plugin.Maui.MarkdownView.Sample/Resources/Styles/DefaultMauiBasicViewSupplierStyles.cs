using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Sample.Resources.Styles;

public class DefaultMauiBasicViewSupplierStyles : MauiBasicViewSupplierStyles
{
    public DefaultMauiBasicViewSupplierStyles()
    {
        LoadDefaultStylesIn(this);
    }

    public static void LoadDefaultStylesIn(MauiBasicViewSupplierStyles styles)
    {
        var resources = Application.Current!.Resources;
        styles.LayoutViewStyle = (Style)resources["MarkdownLayoutViewStyle"];
        styles.TextViewStyle = (Style)resources["MarkdownTextViewStyle"];
        styles.ListTextViewStyle = (Style)resources["MarkdownListTextViewStyle"];
        styles.BlockquotesTextViewStyle = (Style)resources["MarkdownBlockquotesTextViewStyle"];
        styles.HeaderViewLevel1Style = (Style)resources["MarkdownHeaderViewLevel1Style"];
        styles.HeaderViewLevel2Style = (Style)resources["MarkdownHeaderViewLevel2Style"];
        styles.HeaderViewLevel3Style = (Style)resources["MarkdownHeaderViewLevel3Style"];
        styles.HeaderViewLevel4Style = (Style)resources["MarkdownHeaderViewLevel4Style"];
        styles.HeaderViewLevel5Style = (Style)resources["MarkdownHeaderViewLevel5Style"];
        styles.HeaderViewLevel6Style = (Style)resources["MarkdownHeaderViewLevel6Style"];
        styles.BlockquotesOuterViewStyle = (Style)resources["MarkdownBlockquotesOuterViewStyle"];
        styles.BlockquotesInnerViewStyle = (Style)resources["MarkdownBlockquotesInnerViewStyle"];
        styles.FencedCodeBlockLayoutStyle = (Style)resources["MarkdownFencedCodeBlockLayoutStyle"];
        styles.FencedCodeBlockLabelStyle = (Style)resources["MarkdownFencedCodeBlockLabelStyle"];
        styles.HtmlBlockLayoutStyle = (Style)resources["MarkdownHtmlBlockLayoutStyle"];
        styles.HtmlBlockLabelStyle = (Style)resources["MarkdownHtmlBlockLabelStyle"];
        styles.ImageLayoutViewStyle = (Style)resources["MarkdownImageLayoutViewStyle"];
        styles.ImageSubscriptionViewStyle = (Style)resources["MarkdownImageSubscriptionViewStyle"];
        styles.ImageViewStyle = (Style)resources["MarkdownImageViewStyle"];
        styles.IndentedCodeBlockLabelStyle = (Style)resources["MarkdownIndentedCodeBlockLabelStyle"];
        styles.IndentedCodeBlockLayoutStyle = (Style)resources["MarkdownIndentedCodeBlockLayoutStyle"];
        styles.ListItemBulletViewLevel1Style = (Style)resources["MarkdownListItemBulletViewLevel1Style"];
        styles.ListItemBulletViewLevel2Style = (Style)resources["MarkdownListItemBulletViewLevel2Style"];
        styles.ListItemBulletViewLevel3Style = (Style)resources["MarkdownListItemBulletViewLevel3Style"];
        styles.ListItemBulletViewLevel4Style = (Style)resources["MarkdownListItemBulletViewLevel4Style"];
        styles.ListItemBulletViewLevel5Style = (Style)resources["MarkdownListItemBulletViewLevel5Style"];
        styles.ListItemBulletViewLevel6Style = (Style)resources["MarkdownListItemBulletViewLevel6Style"];
        styles.ListItemViewStyle = (Style)resources["MarkdownListItemViewStyle"];
        styles.ListViewStyle = (Style)resources["MarkdownListViewStyle"];
        styles.ThematicBreakStyle = (Style)resources["MarkdownThematicBreakStyle"];
    }
}