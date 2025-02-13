namespace Plugin.Maui.MarkdownView.Controls;

public class HyperlinkSpan : Span
{
	public static readonly BindableProperty UrlProperty =
		BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkSpan), null);

	public string Url
	{
		get => (string)GetValue(UrlProperty);
		set => SetValue(UrlProperty, value);
	}

	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(Command<HyperlinkSpan>), typeof(HyperlinkSpan), null);

	public Command<HyperlinkSpan>? Command
	{
		get => GetValue(CommandProperty) as Command<HyperlinkSpan>;
		set => SetValue(CommandProperty, value);
	}

	public HyperlinkSpan()
	{
		GestureRecognizers.Add(new TapGestureRecognizer
		{
			Command = new Command(OnHyperlinkTapped)
		});
	}

	private void OnHyperlinkTapped()
	{
		if (Command != null
		    && Command.CanExecute(Url))
		{
			Command.Execute(this);
		}
	}
}