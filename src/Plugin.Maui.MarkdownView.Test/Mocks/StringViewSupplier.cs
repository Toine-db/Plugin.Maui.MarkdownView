namespace Plugin.Maui.MarkdownView.Test.Mocks;

internal class StringViewSupplier : IViewSupplier<string>
{
    public string GetTextView(string content)
    {
        return $"textview:{content}";
    }

    public string GetBlockquotesView(string content)
    {
        return $"blockquoteview>:{content}<blockquoteview";
    }

    public string GetHeaderView(string content, int headerLevel)
    {
        return $"headerview:{headerLevel}:{content}";
    }

    public string GetImageView(string url, string subscription, string imageId)
    {
        return $"imageview:{url}:{subscription}";
    }

    public string GetListView(List<string> items)
    {
        // Each item will start with a '-'
        var listItems = items.Aggregate(string.Empty, (current, item) => current + $"-{item}");

        return $"listview>:{listItems}<listview";
    }

    public string GetListItemView(string content, bool isOrderedList, int sequenceNumber, int listLevel)
    {
        return $"listitemview:_{isOrderedList}.{listLevel}.{sequenceNumber}_{content}";
    }

    public string GetStackLayoutView(List<string> childViews)
    {
        var listItems = childViews.Aggregate(string.Empty, (current, item) => current + $"+{item}");

        return $"stackview>:{listItems}<stackview";
    }

    public string GetThematicBreak()
    {
        return "thematicbreakview";
    }

    public string GetPlaceholder(string placeholderName)
    {
        return $"placeholderview:{placeholderName}";
    }
}