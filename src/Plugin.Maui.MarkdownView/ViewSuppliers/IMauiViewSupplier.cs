using MarkdownParser;

namespace Plugin.Maui.MarkdownView.ViewSuppliers;

public interface IMauiViewSupplier : IViewSupplier<View>
{
    bool IgnoreSafeArea { get; set; }
}