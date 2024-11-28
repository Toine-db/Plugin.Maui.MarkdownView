using MarkdownParser;
using MarkdownParser.Models;

namespace Plugin.Maui.MarkdownView;

public class MauiViewSupplier : IViewSupplier<View>
{
    public IEnumerable<MarkdownReferenceDefinition>? MarkdownReferenceDefinitions { get; private set; }

    public void OnReferenceDefinitionsPublished(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
    {
        MarkdownReferenceDefinitions = markdownReferenceDefinitions;
    }

    public View CreateTextView(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Environment.CommandLine);
        var textview = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = content
        };

        return textview;
    }

    public View CreateBlockquotesView(View childView)
    {
        var boxview = new BoxView()
        {
            BackgroundColor = Colors.Gray
        };

        boxview.AddLogicalChild(childView);

        return boxview;
    }

    public View CreateHeaderView(TextBlock textBlock, int headerLevel)
    {
        var content = textBlock.ExtractLiteralContent(Environment.CommandLine);
        var header = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20 + (10 - (headerLevel * 2)),
            Text = content
        };

        return header;
    }

    public View CreateImageView(string url, string subscription, string imageId)
    {
        var imageView = new Image();
        imageView.Source = ImageSource.FromFile(url);

        return imageView;
    }

    public View CreateListItemView(View childView, bool isOrderedList, int sequenceNumber, int listLevel)
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

    public View CreateListView(List<View> items)
    {
        var stacklayout = new VerticalStackLayout();
        foreach (var view in items)
        {
            stacklayout.Children.Add(view);
        }

        return stacklayout;
    }

    public View CreatePlaceholder(string placeholderName)
    {
        return new BoxView()
        {
            HeightRequest = 20,
            BackgroundColor = Colors.Red,
            HorizontalOptions = LayoutOptions.Fill
        };
    }

    public View CreateFencedCodeBlock(TextBlock textBlock, string codeInfo)
    {
        var content = textBlock.ExtractLiteralContent(Environment.CommandLine);
        var label = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = content,
            Padding = new Thickness(10),
            Margin = new Thickness(10,0,0,0),
            BackgroundColor = Colors.LightGray
        };

        return label;
    }

    public View CreateIndentedCodeBlock(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Environment.CommandLine);
        var label = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = content,
            Padding = new Thickness(10),
            Margin = new Thickness(10, 0, 0, 0),
            BackgroundColor = Colors.LightGray
        };

        return label;
    }

    public View CreateHtmlBlock(TextBlock textBlock)
    {
        var content = textBlock.ExtractLiteralContent(Environment.CommandLine);
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

    public View CreateStackLayoutView(List<View> childViews)
    {
        var stacklayout = new VerticalStackLayout();
        foreach (var view in childViews)
        {
            stacklayout.Children.Add(view);
        }

        return stacklayout;
    }

    public View CreateThematicBreak()
    {
        return new BoxView()
        {
            HeightRequest = 2,
            BackgroundColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Fill
        };
    }
}