namespace Plugin.Maui.MarkdownView.Sample.Views;

public partial class MenuItemView
{
	public static readonly BindableProperty UrlProperty =
		BindableProperty.Create(nameof(Url), typeof(string), typeof(MenuItemView), string.Empty);

	public string Url
	{
		get => (string)GetValue(UrlProperty);
		set => SetValue(UrlProperty, value);
	}

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(MenuItemView), string.Empty);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(string), typeof(MenuItemView), string.Empty);

	public string Icon
	{
		get => (string)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public static readonly BindableProperty IconColorProperty =
		BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(MenuItemView), Colors.Black);

	public Color IconColor
	{
		get => (Color)GetValue(IconColorProperty);
		set => SetValue(IconColorProperty, value);
	}

	public static readonly BindableProperty ChevronColorProperty =
		BindableProperty.Create(nameof(ChevronColor), typeof(Color), typeof(MenuItemView), Colors.Black);

	public Color ChevronColor
	{
		get => (Color)GetValue(ChevronColorProperty);
		set => SetValue(ChevronColorProperty, value);
	}

	public static readonly BindableProperty LabelStyleProperty =
		BindableProperty.Create(nameof(LabelStyle), typeof(Style), typeof(MenuItemView), null);

	public Style LabelStyle
	{
		get => (Style)GetValue(LabelStyleProperty);
		set => SetValue(LabelStyleProperty, value);
	}

	public static readonly BindableProperty SeparatorStyleProperty =
		BindableProperty.Create(nameof(SeparatorStyle), typeof(Style), typeof(MenuItemView), null);

	public Style SeparatorStyle
	{
		get => (Style)GetValue(SeparatorStyleProperty);
		set => SetValue(SeparatorStyleProperty, value);
	}

	public Func<string?, Task>? OnItemTapped { get; set; }

	public MenuItemView()
	{
		InitializeComponent();
	}

	private async void OnTapped(object? sender, TappedEventArgs e)
	{
		VisualStateManager.GoToState(this, "Pressed");
		await Task.Delay(50);
		if (OnItemTapped != null)
		{
			await OnItemTapped(Url);
		}
		VisualStateManager.GoToState(this, "Normal");
	}
}