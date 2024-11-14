using MarkdownParser;
using MarkdownParser.Models;

namespace Plugin.Maui.MarkdownView;

public class MauiViewSupplier : IViewSupplier<View>
{
    public IEnumerable<MarkdownReferenceDefinition>? MarkdownReferenceDefinitions { get; private set; }

    public void RegisterReferenceDefinitions(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
    {
        MarkdownReferenceDefinitions = markdownReferenceDefinitions;
    }

    public View GetTextView(TextBlock textBlock)
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

    public View GetBlockquotesView(View childView)
    {
        var boxview = new BoxView()
        {
            BackgroundColor = Colors.Gray
        };

        boxview.AddLogicalChild(childView);

        return boxview;
    }

    public View GetHeaderView(TextBlock textBlock, int headerLevel)
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

    public View GetImageView(string url, string subscription, string imageId)
    {
        //var imageView = new Image();
        //imageView.Source = new Uri(url);

        //return imageView;

        return new BoxView()
        {
            HeightRequest = 50,
            BackgroundColor = Colors.Blue,
            HorizontalOptions = LayoutOptions.Fill
        };
    }

    public View GetListItemView(View childView, bool isOrderedList, int sequenceNumber, int listLevel)
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

    public View GetListView(List<View> items)
    {
        var stacklayout = new VerticalStackLayout();
        foreach (var view in items)
        {
            stacklayout.Children.Add(view);
        }

        return stacklayout;
    }

    public View GetPlaceholder(string placeholderName)
    {
        return new BoxView()
        {
            HeightRequest = 20,
            BackgroundColor = Colors.Red,
            HorizontalOptions = LayoutOptions.Fill
        };
    }

    public View GetFencedCodeBlock(TextBlock textBlock, string codeInfo)
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

    public View GetIndentedCodeBlock(TextBlock textBlock)
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

    public View GetHtmlBlock(TextBlock textBlock)
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

    public View GetStackLayoutView(List<View> childViews)
    {
        var stacklayout = new VerticalStackLayout();
        foreach (var view in childViews)
        {
            stacklayout.Children.Add(view);
        }

        return stacklayout;
    }

    public View GetThematicBreak()
    {
        return new BoxView()
        {
            HeightRequest = 2,
            BackgroundColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Fill
        };
    }
}