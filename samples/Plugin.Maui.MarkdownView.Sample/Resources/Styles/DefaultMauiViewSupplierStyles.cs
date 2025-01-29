using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Sample.Resources.Styles;

public class DefaultMauiBasicViewSupplierStyles : MauiBasicViewSupplierStyles
{
    public DefaultMauiBasicViewSupplierStyles()
    {
        var resources = Application.Current!.Resources;
        LayoutViewStyle = (Style)resources["MarkdownLayoutViewStyle"];
        TextViewStyle = (Style)resources["MarkdownTextViewStyle"];
        ListTextViewStyle = (Style)resources["MarkdownListTextViewStyle"];
        BlockquotesTextViewStyle = (Style)resources["MarkdownBlockquotesTextViewStyle"];
        HeaderViewLevel1Style = (Style)resources["MarkdownHeaderViewLevel1Style"];
        HeaderViewLevel2Style = (Style)resources["MarkdownHeaderViewLevel2Style"];
        HeaderViewLevel3Style = (Style)resources["MarkdownHeaderViewLevel3Style"];
        HeaderViewLevel4Style = (Style)resources["MarkdownHeaderViewLevel4Style"];
        HeaderViewLevel5Style = (Style)resources["MarkdownHeaderViewLevel5Style"];
        HeaderViewLevel6Style = (Style)resources["MarkdownHeaderViewLevel6Style"];
        BlockquotesOuterViewStyle = (Style)resources["MarkdownBlockquotesOuterViewStyle"];
        BlockquotesInnerViewStyle = (Style)resources["MarkdownBlockquotesInnerViewStyle"];
        FencedCodeBlockLayoutStyle = (Style)resources["MarkdownFencedCodeBlockLayoutStyle"];
        FencedCodeBlockLabelStyle = (Style)resources["MarkdownFencedCodeBlockLabelStyle"];
        HtmlBlockLayoutStyle = (Style)resources["MarkdownHtmlBlockLayoutStyle"];
        HtmlBlockLabelStyle = (Style)resources["MarkdownHtmlBlockLabelStyle"];
        ImageLayoutViewStyle = (Style)resources["MarkdownImageLayoutViewStyle"];
        ImageSubscriptionViewStyle = (Style)resources["MarkdownImageSubscriptionViewStyle"];
        ImageViewStyle = (Style)resources["MarkdownImageViewStyle"];
        IndentedCodeBlockLabelStyle = (Style)resources["MarkdownIndentedCodeBlockLabelStyle"];
        IndentedCodeBlockLayoutStyle = (Style)resources["MarkdownIndentedCodeBlockLayoutStyle"];
        ListItemBulletViewLevel1Style = (Style)resources["MarkdownListItemBulletViewLevel1Style"];
        ListItemBulletViewLevel2Style = (Style)resources["MarkdownListItemBulletViewLevel2Style"];
        ListItemBulletViewLevel3Style = (Style)resources["MarkdownListItemBulletViewLevel3Style"];
        ListItemBulletViewLevel4Style = (Style)resources["MarkdownListItemBulletViewLevel4Style"];
        ListItemBulletViewLevel5Style = (Style)resources["MarkdownListItemBulletViewLevel5Style"];
        ListItemBulletViewLevel6Style = (Style)resources["MarkdownListItemBulletViewLevel6Style"];
        ListItemViewStyle = (Style)resources["MarkdownListItemViewStyle"];
        ListViewStyle = (Style)resources["MarkdownListViewStyle"];
        ThematicBreakStyle = (Style)resources["MarkdownThematicBreakStyle"];
    }
}