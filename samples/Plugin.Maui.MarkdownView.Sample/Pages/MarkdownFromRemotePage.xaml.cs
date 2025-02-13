using Plugin.Maui.MarkdownView.Sample.Resources.Styles;
using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Sample.Pages;

public partial class MarkdownFromRemotePage : ContentPage
{
	private const string DefaultRemoteFile = "https://enbyin.com/resources/MarkdownExample.md";

	public MarkdownFromRemotePage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (string.IsNullOrWhiteSpace(MarkdownUrlEntry.Text))
		{
			MarkdownUrlEntry.Text = DefaultRemoteFile;
		}

		RefreshView.IsRefreshing = true; // this triggers OnRefreshing()
	}

	private void OnLoadMarkdownUrl(object? sender, EventArgs e)
	{
		RefreshView.IsRefreshing = true; // this triggers OnRefreshing()
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
			SetRemoteBasePathForResources();

			using var http = new HttpClient();
			var markdown = await http.GetStringAsync(MarkdownUrlEntry.Text);

			MarkdownView.MarkdownText = markdown;
		}
		catch (Exception e)
		{
			MarkdownView.MarkdownText = $"__error: wasn't able to retrieve '{MarkdownUrlEntry.Text}'__";
		}
	}

	private void SetRemoteBasePathForResources()
	{
		// To work with online images and other resources 
		// 1. determine images/resources base path
		// 2. create and set new IViewSupplier that works with the base path
		// 3. set some (prefix) paths that the system can ignore when converting relative paths

		var remoteFile = new Uri(MarkdownUrlEntry.Text);
		var basePathRemoteFile = string.Format("{0}{1}"
			, remoteFile.GetLeftPart(UriPartial.Authority)
			, string.Join(string.Empty,
				remoteFile.Segments.Take(remoteFile.Segments.Length - 1)));

		MarkdownView.ViewSupplier = new MauiFormattedTextViewSupplier()
		{
			BasePathForRelativeUrlConversion = basePathRemoteFile,
			PrefixesToIgnoreForRelativeUrlConversion = Array.Empty<string>(),
			Styles = new DefaultMauiBasicViewSupplierStyles(),
			FormattedTextStyles = new DefaultMauiFormattedTextViewSupplierStyles()
		};
	}
}