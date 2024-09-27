using CommonMark.Syntax;

namespace Plugin.Maui.MarkdownView;

public class MarkdownParseStream<T> : IDisposable
{
    private readonly TextReader _markdownSource;

    private Block _markdownDocument;
    private Block _workingBlock;

    // Format\Convert c# objects into <T> UI Components
    private ViewFormatter<T> _formatter;

    private readonly Queue<T> _readCache = new Queue<T>();

    public MarkdownParseStream(IViewSupplier<T> viewSupplier, TextReader markdownSource)
    {
        _markdownSource = markdownSource;
        _formatter = new ViewFormatter<T>(viewSupplier);
    }

    /// <summary>
    /// Read 1 element
    /// </summary>
    /// <returns>1 element or default(T)/null when finished</returns>
    public T Read()
    {
        if (_markdownDocument == null)
        {
            _markdownDocument = MarkdownParser<T>.GetMarkdownDocument(_markdownSource);

            // Set first block to parse
            _workingBlock = _markdownDocument.FirstChild;
        }

        if (_readCache.Count != 0)
        {
            return _readCache.Dequeue();
        }

        if (_workingBlock == null)
        {
            return default(T);
        }

        var uiComponents = _formatter.FormatSingleBlock(_workingBlock);
        _workingBlock = _workingBlock.NextSibling;

        AddComponentsToCache(uiComponents);

        if (_readCache.Count != 0)
        {
            return _readCache.Dequeue();
        }

        return Read();
    }

    private void AddComponentsToCache(List<T> components)
    {
        foreach (var uiComponent in components)
        {
            _readCache.Enqueue(uiComponent);
        }
    }

    public void Dispose()
    {
        _markdownDocument = null;
        _formatter = null;
        _workingBlock = null;
    }
}