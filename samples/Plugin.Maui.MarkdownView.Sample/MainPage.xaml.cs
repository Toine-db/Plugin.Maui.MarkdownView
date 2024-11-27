namespace Plugin.Maui.MarkdownView.Sample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //using var stream = await FileSystem.OpenAppPackageFileAsync("LorumMarkdown.md");
        //using var reader = new StreamReader(stream);
        //var markdown = reader.ReadToEnd();

        //MarkdownView.MarkdownText = markdown;
    }
}

