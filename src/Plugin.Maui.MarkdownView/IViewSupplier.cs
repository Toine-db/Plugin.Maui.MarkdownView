namespace Plugin.Maui.MarkdownView;

public interface IViewSupplier<T>
{
    /// <summary>
    /// a default text view
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    T GetTextView(string content);

    /// <summary>
    /// a block quote view, where other views can be inserted
    /// </summary>
    /// <param name="childView"></param>
    /// <returns></returns>
    T GetBlockquotesView(T childView);

    /// <summary>
    /// a title, subtitle or header view
    /// </summary>
    /// <param name="content"></param>
    /// <param name="headerLevel"></param>
    /// <returns></returns>
    T GetHeaderView(string content, int headerLevel);

    /// <summary>
    /// a image view with a optional subscription text view
    /// </summary>
    /// <param name="url">image location</param>
    /// <param name="subscription">(optional) null or empty when not used</param>
    /// <param name="imageId">(optional) id for image</param>
    /// <returns></returns>
    T GetImageView(string url, string subscription, string imageId);

    /// <summary>
    /// a view that shows a list of listitems 
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    T GetListView(List<T> items);

    /// <summary>
    /// a view that shows a single item for a ListView (return View can be used in GetListView)
    /// </summary>
    /// <param name="childView">view as childview (or use the content parameter)</param>
    /// <param name="isOrderedList">does the item belong to a ordered (numbered) list</param>
    /// <param name="sequenceNumber">number of sequence</param>
    /// <param name="listLevel">level depth of the list, root level starting at 1</param>
    /// <returns></returns>
    T GetListItemView(T childView, bool isOrderedList, int sequenceNumber, int listLevel);

    /// <summary>
    /// a layout that shows a collection of views
    /// </summary>
    /// <param name="childViews">collection of views</param>
    /// <returns></returns>
    T GetStackLayoutView(List<T> childViews);

    /// <summary>
    /// a image view that separates content 
    /// </summary>
    /// <returns></returns>
    T GetThematicBreak();


    /// <summary>
    /// a placeholder for views or other objects
    /// </summary>
    /// <param name="placeholderName">placeholder string</param>
    /// <returns></returns>
    T GetPlaceholder(string placeholderName);
}