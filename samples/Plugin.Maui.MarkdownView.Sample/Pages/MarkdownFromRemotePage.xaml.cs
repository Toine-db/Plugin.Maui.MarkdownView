namespace Plugin.Maui.MarkdownView.Sample.Pages;

public partial class MarkdownFromRemotePage : ContentPage
{
    private const string RemoteFile = "https://enbyin.com/resources/MarkdownExample.md";

    public MarkdownFromRemotePage()
	{
		InitializeComponent();

        SetRemoteBasePathForResources();
    }

    private void SetRemoteBasePathForResources()
    {
        // To work with online images and other resources 
        // 1. determine images/resources base path
        // 2. create and set new IViewSupplier that works with the base path
        // 3. set some (prefix) paths that the system can ignore when converting relative paths

        var remoteFile = new Uri(RemoteFile);
        var basePathRemoteFile = string.Format("{0}{1}"
            , remoteFile.GetLeftPart(UriPartial.Authority)
            , string.Join(string.Empty,
                remoteFile.Segments.Take(remoteFile.Segments.Length - 1)));
        
        MarkdownView.ViewSupplier = new MauiViewSupplier
        {
            BasePathForRelativeUrlConversion = basePathRemoteFile,
            PrefixesToIgnoreForRelativeUrlConversion = Array.Empty<string>()
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshView.IsRefreshing = true;
    }

    private async void OnRefreshing(object? sender, EventArgs e)
    {
        await LoadMarkdownFromRemote();
        RefreshView.IsRefreshing = false;
    }

    private async Task LoadMarkdownFromRemote()
    {
        try
        {
            using var http = new HttpClient();
            var markdown = await http.GetStringAsync(RemoteFile);

            MarkdownView.MarkdownText = markdown;
        }
        catch (Exception e)
        {
            MarkdownView.MarkdownText = $"__error: wasn't able to retrieve '{RemoteFile}'__";
        }
    }
}