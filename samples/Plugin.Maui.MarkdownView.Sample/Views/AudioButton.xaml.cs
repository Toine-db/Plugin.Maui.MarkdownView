
using Plugin.Maui.Audio;

namespace Plugin.Maui.MarkdownView.Sample.Views;

public partial class AudioButton
{
	public AudioButton()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(AudioButton), string.Empty);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly BindableProperty AudioFileNameProperty =
		BindableProperty.Create(nameof(AudioFileName), typeof(string), typeof(AudioButton), string.Empty);

	public string AudioFileName
	{
		get => (string)GetValue(AudioFileNameProperty);
		set => SetValue(AudioFileNameProperty, value);
	}

	public static readonly BindableProperty ColorProperty =
		BindableProperty.Create(nameof(Color), typeof(Color), typeof(AudioButton), Colors.White);

	public Color Color
	{
		get => (Color)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	public static readonly BindableProperty ButtonColorProperty =
		BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(AudioButton), Colors.Black);

	public Color ButtonColor
	{
		get => (Color)GetValue(ButtonColorProperty);
		set => SetValue(ButtonColorProperty, value);
	}

	public static readonly BindableProperty ButtonStyleProperty =
		BindableProperty.Create(nameof(ButtonStyle), typeof(Style), typeof(AudioButton), null);

	public Style ButtonStyle
	{
		get => (Style)GetValue(ButtonStyleProperty);
		set => SetValue(ButtonStyleProperty, value);
	}

	private async void OnClicked(object? sender, EventArgs e)
	{
		var audioFile = await FileSystem.OpenAppPackageFileAsync(AudioFileName);
		var audioPlayer = AudioManager.Current.CreatePlayer(audioFile);
		audioPlayer.Play();
	}
}