using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Sample.Resources.Styles;

public class MyMauiBasicViewSupplierStyles : MauiBasicViewSupplierStyles
{
    public MyMauiBasicViewSupplierStyles()
    {
        LoadDefaultStylesIn(this);
    }

    public static void LoadDefaultStylesIn(MauiBasicViewSupplierStyles styles)
    {
        var resources = Application.Current!.Resources;
        styles.LayoutViewStyle = (Style)resources["ExampleMarkdownLayoutViewStyle"];
        styles.TextViewStyle = (Style)resources["ExampleMarkdownTextViewStyle"];
        styles.ListTextViewStyle = (Style)resources["ExampleMarkdownListTextViewStyle"];
        styles.BlockquotesTextViewStyle = (Style)resources["ExampleMarkdownBlockquotesTextViewStyle"];
        styles.HeaderViewLevel1Style = (Style)resources["ExampleMarkdownHeaderViewLevel1Style"];
        styles.HeaderViewLevel2Style = (Style)resources["ExampleMarkdownHeaderViewLevel2Style"];
        styles.HeaderViewLevel3Style = (Style)resources["ExampleMarkdownHeaderViewLevel3Style"];
        styles.HeaderViewLevel4Style = (Style)resources["ExampleMarkdownHeaderViewLevel4Style"];
        styles.HeaderViewLevel5Style = (Style)resources["ExampleMarkdownHeaderViewLevel5Style"];
        styles.HeaderViewLevel6Style = (Style)resources["ExampleMarkdownHeaderViewLevel6Style"];
        styles.BlockquotesOuterViewStyle = (Style)resources["ExampleMarkdownBlockquotesOuterViewStyle"];
        styles.BlockquotesInnerViewStyle = (Style)resources["ExampleMarkdownBlockquotesInnerViewStyle"];
        styles.FencedCodeBlockLayoutStyle = (Style)resources["ExampleMarkdownFencedCodeBlockLayoutStyle"];
        styles.FencedCodeBlockLabelStyle = (Style)resources["ExampleMarkdownFencedCodeBlockLabelStyle"];
        styles.HtmlBlockLayoutStyle = (Style)resources["ExampleMarkdownHtmlBlockLayoutStyle"];
        styles.HtmlBlockLabelStyle = (Style)resources["ExampleMarkdownHtmlBlockLabelStyle"];
        styles.ImageLayoutViewStyle = (Style)resources["ExampleMarkdownImageLayoutViewStyle"];
        styles.ImageSubscriptionViewStyle = (Style)resources["ExampleMarkdownImageSubscriptionViewStyle"];
        styles.ImageViewStyle = (Style)resources["ExampleMarkdownImageViewStyle"];
        styles.IndentedCodeBlockLabelStyle = (Style)resources["ExampleMarkdownIndentedCodeBlockLabelStyle"];
        styles.IndentedCodeBlockLayoutStyle = (Style)resources["ExampleMarkdownIndentedCodeBlockLayoutStyle"];
        styles.ListItemBulletViewLevel1Style = (Style)resources["ExampleMarkdownListItemBulletViewLevel1Style"];
        styles.ListItemBulletViewLevel2Style = (Style)resources["ExampleMarkdownListItemBulletViewLevel2Style"];
        styles.ListItemBulletViewLevel3Style = (Style)resources["ExampleMarkdownListItemBulletViewLevel3Style"];
        styles.ListItemBulletViewLevel4Style = (Style)resources["ExampleMarkdownListItemBulletViewLevel4Style"];
        styles.ListItemBulletViewLevel5Style = (Style)resources["ExampleMarkdownListItemBulletViewLevel5Style"];
        styles.ListItemBulletViewLevel6Style = (Style)resources["ExampleMarkdownListItemBulletViewLevel6Style"];
        styles.ListItemViewStyle = (Style)resources["ExampleMarkdownListItemViewStyle"];
        styles.ListViewStyle = (Style)resources["ExampleMarkdownListViewStyle"];
        styles.ThematicBreakStyle = (Style)resources["ExampleMarkdownThematicBreakStyle"];
    }
}