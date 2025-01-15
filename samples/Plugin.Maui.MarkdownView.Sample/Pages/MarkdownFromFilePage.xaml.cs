namespace Plugin.Maui.MarkdownView.Sample.Pages;

public partial class MarkdownFromFilePage : ContentPage
{
	public MarkdownFromFilePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await using var stream = await FileSystem.OpenAppPackageFileAsync("LorumMarkdown.md");
        using var reader = new StreamReader(stream);
        var markdown = await reader.ReadToEndAsync();

        MarkdownView.MarkdownText = markdown;
    }
}