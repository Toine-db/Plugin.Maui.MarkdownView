using MarkdownParser;

namespace Plugin.Maui.MarkdownView;

public class MauiViewSupplier : IViewSupplier<View>
{
    public View GetBlockquotesView(View childView)
    {
        var boxview = new BoxView()
        {
            BackgroundColor = Colors.Gray
        };

        boxview.AddLogicalChild(childView);

        return boxview;
    }

    public View GetHeaderView(string content, int headerLevel)
    {
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

    public View GetStackLayoutView(List<View> childViews)
    {
        var stacklayout = new VerticalStackLayout();
        foreach (var view in childViews)
        {
            stacklayout.Children.Add(view);
        }

        return stacklayout;
    }

    public View GetTextView(string content)
    {
        var textview = new Label()
        {
            LineBreakMode = LineBreakMode.WordWrap,
            FontSize = 20,
            Text = content
        };

        return textview;
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