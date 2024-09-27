using System.Text;
using CommonMark.Syntax;

namespace Plugin.Maui.MarkdownView;

public class ViewWriterCache<T>
{
    public BlockTag? ComponentType { get; set; }

    private readonly Stack<Tuple<string, T>> _valuesStack = new Stack<Tuple<string, T>>();
    private readonly T _defaultT = default(T);

    public void Add(T item)
    {
        if (EqualityComparer<T>.Default.Equals(item, _defaultT))
        {
            return;
        } 

        _valuesStack.Push(new Tuple<string, T>(null, item));
    }

    public void Add(string item)
    {
        if (string.IsNullOrEmpty(item))
        {
            return;
        }

        _valuesStack.Push(new Tuple<string, T>(item, _defaultT));
    }

    /// <summary>
    /// Get cached items in order, each Tuple has a text or T value (never both in the same Tuple)
    /// </summary>
    /// <returns>collection that contain a string of T (never both in the same Tuple)</returns>
    public List<Tuple<string, T>> GetGroupedCachedValues()
    {
        var groupedCache = new List<Tuple<string, T>>();

        var workbenchItemTextCacheBuilder = new StringBuilder();
        var values = _valuesStack.Reverse().ToArray();

        foreach (var value in values)
        {
            // Check for text
            if (!string.IsNullOrEmpty(value.Item1))
            {
                workbenchItemTextCacheBuilder.Append(value.Item1);
                continue;
            }

            // No text anymore: Store workbenchItemTextCache if any to groupedCache
            if (workbenchItemTextCacheBuilder.Length != 0)
            {
                groupedCache.Add(new Tuple<string, T>(workbenchItemTextCacheBuilder.ToString(), _defaultT));
                workbenchItemTextCacheBuilder.Clear();
            }

            // If item2 is not null
            if (!EqualityComparer<T>.Default.Equals(value.Item2, _defaultT))
            {
                groupedCache.Add(new Tuple<string, T>(null, value.Item2));
            }
        }

        // Store leftovers workbenchItemTextCache 
        if (workbenchItemTextCacheBuilder.Length != 0)
        {
            groupedCache.Add(new Tuple<string, T>(workbenchItemTextCacheBuilder.ToString(), _defaultT));
            workbenchItemTextCacheBuilder.Clear();
        }

        return groupedCache;
    }
}