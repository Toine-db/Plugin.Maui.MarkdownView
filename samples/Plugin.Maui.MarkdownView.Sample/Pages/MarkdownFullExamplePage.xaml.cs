namespace Plugin.Maui.MarkdownView.Sample.Pages;

public partial class MarkdownFullExamplePage : ContentPage
{
	public MarkdownFullExamplePage()
	{
		InitializeComponent();

        MyViewSupplier.OnMenuItemTapped = OnMenuItemTapped;
    }

    private async Task OnMenuItemTapped(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return;
        }

        await Shell.Current.GoToAsync(nameof(MarkdownFullExampleDetailPage), true, new Dictionary<string, object>()
        {
            { "Url", url }
        });
    }
}