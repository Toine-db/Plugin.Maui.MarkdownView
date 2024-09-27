using System.Diagnostics;
using CommonMark.Syntax;

namespace Plugin.Maui.MarkdownView;

public class ViewWriter<T>
{
    private IViewSupplier<T> ViewSupplier { get; }
    private List<T> WrittenViews { get; set; } = new List<T>();

    private Stack<ViewWriterCache<T>> Workbench { get; } = new Stack<ViewWriterCache<T>>();
    private ViewWriterCache<T> GetWorkbenchItem()
    {
        if (Workbench.Count == 0)
        {
            return null;
        }

        return Workbench.Peek();
    }

    public ViewWriter(IViewSupplier<T> viewSupplier)
    {
        ViewSupplier = viewSupplier;
    }

    public List<T> Flush()
    {
        var collectedViews = WrittenViews;
        WrittenViews = new List<T>();

        return collectedViews;
    }

    public void StartBlock(BlockTag blockType, string content = "")
    {
        Workbench.Push(new ViewWriterCache<T> { ComponentType = blockType });
        GetWorkbenchItem().Add(content);
    }

    public void FinalizeParagraphBlock()
    {
        var wbi = GetWorkbenchItem();
        if (wbi.ComponentType != BlockTag.Paragraph)
        {
            Debug.WriteLine($"Finalizing Paragraph can not finalize {wbi.ComponentType}");
            return;
        }

        var views = new List<T>();

        var topWorkbenchItem = Workbench.Pop();
        var itemsCache = topWorkbenchItem.GetGroupedCachedValues();

        foreach (var itemsCacheTuple in itemsCache)
        {
            var view = !string.IsNullOrEmpty(itemsCacheTuple.Item1) ?
                ViewSupplier.GetTextView(itemsCacheTuple.Item1) : itemsCacheTuple.Item2;

            if (view != null)
            {
                views.Add(view);
            }
        }

        StoreView(StackViews(views));
    }

    public void FinalizeBlockquoteBlock()
    {
        var wbi = GetWorkbenchItem();
        if (wbi.ComponentType != BlockTag.BlockQuote)
        {
            Debug.WriteLine($"Finalizing BlockQuote can not finalize {wbi.ComponentType}");
            return;
        }

        var topWorkbenchItem = Workbench.Pop();
        var itemsCache = topWorkbenchItem.GetGroupedCachedValues();

        var childViews = itemsCache.Select(itemsCacheTuple => itemsCacheTuple.Item2).ToList();
        var childView = StackViews(childViews);

        var blockView = ViewSupplier.GetBlockquotesView(childView);

        StoreView(blockView);
    }

    public void FinalizeHeaderBlock(int headerLevel)
    {
        var wbi = GetWorkbenchItem();
        if (wbi.ComponentType != BlockTag.AtxHeading
            && wbi.ComponentType != BlockTag.SetextHeading)

        {
            Debug.WriteLine($"Finalizing Header can not finalize {wbi.ComponentType}");
            return;
        }

        var views = new List<T>();

        var topWorkbenchItem = Workbench.Pop();
        var itemsCache = topWorkbenchItem.GetGroupedCachedValues();

        foreach (var itemsCacheTuple in itemsCache)
        {
            var view = !string.IsNullOrEmpty(itemsCacheTuple.Item1) ?
                ViewSupplier.GetHeaderView(itemsCacheTuple.Item1, headerLevel) : itemsCacheTuple.Item2;

            views.Add(view);
        }

        StoreView(StackViews(views));
    }

    public void FinalizeListBlock()
    {
        var wbi = GetWorkbenchItem();
        if (wbi.ComponentType != BlockTag.List)
        {
            Debug.WriteLine($"Finalizing List can not finalize {wbi.ComponentType}");
            return;
        }

        var topWorkbenchItem = Workbench.Pop();
        var itemsCache = topWorkbenchItem.GetGroupedCachedValues();

        var listItems = itemsCache.Select(itemsCacheTuple => itemsCacheTuple.Item2).ToList();
        var listView = ViewSupplier.GetListView(listItems);

        StoreView(listView);
    }

    public void FinalizeListItemBlock(ListData listData)
    {
        var wbi = GetWorkbenchItem();
        if (wbi.ComponentType != BlockTag.ListItem)
        {
            Debug.WriteLine($"Finalizing ListItem can not finalize {wbi.ComponentType}");
            return;
        }

        var views = new List<T>();

        var isOrderedList = listData.ListType == ListType.Ordered;
        var sequenceNumber = listData.Start;
        var depthLevel = Workbench.Count(wbItem => wbItem.ComponentType == BlockTag.List);

        var topWorkbenchItem = Workbench.Pop();
        var itemsCache = topWorkbenchItem.GetGroupedCachedValues();


        foreach (var itemsCacheTuple in itemsCache)
        {
            var view = !string.IsNullOrEmpty(itemsCacheTuple.Item1) ?
                ViewSupplier.GetTextView(itemsCacheTuple.Item1) : itemsCacheTuple.Item2;

            if (view != null)
            {
                views.Add(view);
            }
        }

        var flattendView = StackViews(views);

        var listItemView = ViewSupplier.GetListItemView(flattendView, isOrderedList, sequenceNumber, depthLevel);

        StoreView(listItemView);
    }

    public void AddText(string content)
    {
        GetWorkbenchItem().Add(content);
    }

    public void StartAndFinalizeImageBlock(string targetUrl, string subscription, string imageId)
    {
        var imageView = ViewSupplier.GetImageView(targetUrl, subscription, imageId);
        StoreView(imageView);
    }

    public void StartAndFinalizeThematicBreak()
    {
        var seperator = ViewSupplier.GetThematicBreak();
        StoreView(seperator);
    }

    public void StartAndFinalizePlaceholderBlock(string placeholderName)
    {
        var placeholderView = ViewSupplier.GetPlaceholder(placeholderName);
        StoreView(placeholderView);
    }

    private T StackViews(List<T> views)
    {
        if (views == null || views.Count == 0)
        {
            return default(T);
        }

        // multiple views combine a single stack layout
        var viewToStore = views.Count == 1 ?
            views[0] : ViewSupplier.GetStackLayoutView(views);

        return viewToStore;
    }

    private void StoreView(T view)
    {
        if (view == null)
        {
            return;
        }

        // Check if Workbench has an item where its working on
        var wbi = GetWorkbenchItem();
        if (wbi != null)  // add the new View to the WorkbenchItem
        {
            wbi.Add(view);
        }
        else // otherwise add the new View to finalized views collection
        {
            WrittenViews.Add(view);
        }
    }

}