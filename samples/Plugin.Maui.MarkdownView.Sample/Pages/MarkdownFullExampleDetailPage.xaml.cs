namespace Plugin.Maui.MarkdownView.Sample.Pages;

public partial class MarkdownFullExampleDetailPage : ContentPage, IQueryAttributable
{
	public MarkdownFullExampleDetailPage()
	{
		InitializeComponent();
	}
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.ContainsKey("Url"))
        {
            return;
        }

        var url = query["Url"] as string;
        Title = $"Details {url.Replace("/", ": ").Replace("_", " ")}";
    }
}