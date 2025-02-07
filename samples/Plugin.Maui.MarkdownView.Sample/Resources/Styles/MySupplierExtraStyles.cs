using Plugin.Maui.MarkdownView.Sample.CustomViewSuppliers;

namespace Plugin.Maui.MarkdownView.Sample.Resources.Styles;

public class MySupplierExtraStyles : SupplierExtraStyles
{
    public MySupplierExtraStyles()
    {
        LoadDefaultStylesIn(this);
    }

    public static void LoadDefaultStylesIn(SupplierExtraStyles styles)
    {
        var resources = Application.Current!.Resources;
        styles.ButtonViewStyle = (Style)resources["ExampleMarkdownIconButtonStyle"];
        styles.MenuItemViewStyle = (Style)resources["ExampleMarkdownMenuItemViewStyle"];
        styles.MenuItemLabelStyle = (Style)resources["ExampleMarkdownMenuItemLabelStyle"];
        styles.MenuItemSeparatorStyle = (Style)resources["ExampleMarkdownMenuItemSeparatorStyle"];
    }
}