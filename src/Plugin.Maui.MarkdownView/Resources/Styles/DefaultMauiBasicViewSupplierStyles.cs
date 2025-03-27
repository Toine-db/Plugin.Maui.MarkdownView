using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Resources.Styles;

public class DefaultMauiBasicViewSupplierStyles : MauiBasicViewSupplierStyles
{
	internal const string UniqueDefaultMarkdownLayoutViewStyleKey = "DefaultMarkdownLayoutViewStyleAxQwewrR";

    public DefaultMauiBasicViewSupplierStyles()
    {
        EnsureStyleDictionaryIsLoaded();

        LoadDefaultStylesIn(this);
    }

    internal static void EnsureStyleDictionaryIsLoaded()
    {
        if (!Application.Current!.Resources.MergedDictionaries.Reverse().Any(
                x => x.ContainsKey(DefaultMauiBasicViewSupplierStyles.UniqueDefaultMarkdownLayoutViewStyleKey)))
        {
            Application.Current.Resources.MergedDictionaries.Add(new DefaultMarkdownStyles());
        }
    }

    public static void LoadDefaultStylesIn(MauiBasicViewSupplierStyles styles)
	{
		var resources = Application.Current!.Resources;
		styles.LayoutViewStyle = (Style)resources[UniqueDefaultMarkdownLayoutViewStyleKey];
		styles.TextViewStyle = (Style)resources["DefaultMarkdownTextViewStyle"];
		styles.ListTextViewStyle = (Style)resources["DefaultMarkdownListTextViewStyle"];
		styles.BlockquotesTextViewStyle = (Style)resources["DefaultMarkdownBlockquotesTextViewStyle"];
		styles.HeaderViewLevel1Style = (Style)resources["DefaultMarkdownHeaderViewLevel1Style"];
		styles.HeaderViewLevel2Style = (Style)resources["DefaultMarkdownHeaderViewLevel2Style"];
		styles.HeaderViewLevel3Style = (Style)resources["DefaultMarkdownHeaderViewLevel3Style"];
		styles.HeaderViewLevel4Style = (Style)resources["DefaultMarkdownHeaderViewLevel4Style"];
		styles.HeaderViewLevel5Style = (Style)resources["DefaultMarkdownHeaderViewLevel5Style"];
		styles.HeaderViewLevel6Style = (Style)resources["DefaultMarkdownHeaderViewLevel6Style"];
		styles.BlockquotesOuterViewStyle = (Style)resources["DefaultMarkdownBlockquotesOuterViewStyle"];
		styles.BlockquotesInnerViewStyle = (Style)resources["DefaultMarkdownBlockquotesInnerViewStyle"];
		styles.FencedCodeBlockLayoutStyle = (Style)resources["DefaultMarkdownFencedCodeBlockLayoutStyle"];
		styles.FencedCodeBlockLabelStyle = (Style)resources["DefaultMarkdownFencedCodeBlockLabelStyle"];
		styles.HtmlBlockLayoutStyle = (Style)resources["DefaultMarkdownHtmlBlockLayoutStyle"];
		styles.HtmlBlockLabelStyle = (Style)resources["DefaultMarkdownHtmlBlockLabelStyle"];
		styles.ImageLayoutViewStyle = (Style)resources["DefaultMarkdownImageLayoutViewStyle"];
		styles.ImageSubscriptionViewStyle = (Style)resources["DefaultMarkdownImageSubscriptionViewStyle"];
		styles.ImageViewStyle = (Style)resources["DefaultMarkdownImageViewStyle"];
		styles.IndentedCodeBlockLabelStyle = (Style)resources["DefaultMarkdownIndentedCodeBlockLabelStyle"];
		styles.IndentedCodeBlockLayoutStyle = (Style)resources["DefaultMarkdownIndentedCodeBlockLayoutStyle"];
		styles.ListItemBulletViewLevel1Style = (Style)resources["DefaultMarkdownListItemBulletViewLevel1Style"];
		styles.ListItemBulletViewLevel2Style = (Style)resources["DefaultMarkdownListItemBulletViewLevel2Style"];
		styles.ListItemBulletViewLevel3Style = (Style)resources["DefaultMarkdownListItemBulletViewLevel3Style"];
		styles.ListItemBulletViewLevel4Style = (Style)resources["DefaultMarkdownListItemBulletViewLevel4Style"];
		styles.ListItemBulletViewLevel5Style = (Style)resources["DefaultMarkdownListItemBulletViewLevel5Style"];
		styles.ListItemBulletViewLevel6Style = (Style)resources["DefaultMarkdownListItemBulletViewLevel6Style"];
		styles.ListItemViewStyle = (Style)resources["DefaultMarkdownListItemViewStyle"];
		styles.ListViewStyle = (Style)resources["DefaultMarkdownListViewStyle"];
		styles.ThematicBreakStyle = (Style)resources["DefaultMarkdownThematicBreakStyle"];
	}
}