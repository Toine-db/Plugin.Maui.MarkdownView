using MarkdownParser;
using MarkdownParser.Models;

namespace Plugin.Maui.MarkdownView;

public class MauiViewSupplier : IViewSupplier<View>
{
    public IEnumerable<MarkdownReferenceDefinition>? PublishedMarkdownReferenceDefinitions { get; private set; }

    public void OnReferenceDefinitionsPublished(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
    {
        PublishedMarkdownReferenceDefinitions = markdownReferenceDefinitions;
    }

    public virtual View CreateTextView(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Environment.NewLine);
        var textview = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = content
        };

        return textview;
    }

    public virtual View CreateBlockquotesView(View childView)
    {
        var blockView = new Grid()
        {
            BackgroundColor = Colors.Gray,
            Padding = new Thickness(10)
        };

        blockView.Add(childView);

        return blockView;
    }

    public virtual View CreateHeaderView(TextBlock textBlock, int headerLevel)
    {
        var content = textBlock.ExtractLiteralContent(Environment.NewLine);
        var header = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20 + (10 - (headerLevel * 2)),
            Text = content
        };

        return header;
    }

    public virtual View CreateImageView(string url, string subscription, string imageId)
    {
        var imageView = new Image();
        imageView.Source = ImageSource.FromFile(url);

        return imageView;
    }

    public virtual View CreateListItemView(View childView, bool isOrderedList, int sequenceNumber, int listLevel)
    {
        var stacklayout = new HorizontalStackLayout();

        var bullet = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = " + "
        };

        stacklayout.Children.Add(bullet);
        stacklayout.Children.Add(childView);

        return stacklayout;
    }

    public virtual View CreateListView(List<View> items)
    {
        var stacklayout = new VerticalStackLayout();
        foreach (var view in items)
        {
            stacklayout.Children.Add(view);
        }

        return stacklayout;
    }

    public virtual View CreatePlaceholder(string placeholderName)
    {
        return new BoxView()
        {
            HeightRequest = 20,
            BackgroundColor = Colors.Red,
            HorizontalOptions = LayoutOptions.Fill
        };
    }

    public virtual View CreateFencedCodeBlock(TextBlock textBlock, string codeInfo)
    {
        var content = textBlock.ExtractLiteralContent(Environment.NewLine);
        var label = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = content,
            Padding = new Thickness(10),
            Margin = new Thickness(10,0,0,0),
            BackgroundColor = Colors.LightGreen
        };

        return label;
    }

    public virtual View CreateIndentedCodeBlock(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Environment.NewLine);
        var label = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = content,
            Padding = new Thickness(10),
            Margin = new Thickness(10, 0, 0, 0),
            BackgroundColor = Colors.LightPink
        };

        return label;
    }

    public virtual View CreateHtmlBlock(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Environment.NewLine);
        var label = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = content,
            Padding = new Thickness(10),
            Margin = new Thickness(0, 0, 0, 0),
            BackgroundColor = Colors.LightBlue
        };

        return label;
    }

    public virtual View CreateStackLayoutView(List<View> childViews)
    {
        var stacklayout = new VerticalStackLayout();
        foreach (var view in childViews)
        {
            stacklayout.Children.Add(view);
        }

        return stacklayout;
    }

    public virtual View CreateThematicBreak()
    {
        return new BoxView()
        {
            HeightRequest = 2,
            BackgroundColor = Colors.Purple,
            HorizontalOptions = LayoutOptions.Fill
        };
    }
}