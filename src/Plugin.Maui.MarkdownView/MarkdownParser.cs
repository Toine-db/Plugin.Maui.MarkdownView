using CommonMark;
using CommonMark.Syntax;

namespace Plugin.Maui.MarkdownView;

public class MarkdownParser<T>
{
    private readonly IViewSupplier<T> _viewSupplier;

    public MarkdownParser(IViewSupplier<T> viewSupplier)
    {
        _viewSupplier = viewSupplier;
    }

    public List<T> Parse(string markdownSource)
    {
        using (var reader = new StringReader(markdownSource))
        {
            return Parse(reader);
        }
    }

    public List<T> Parse(TextReader markdownSource)
    {
        // Parse to usable c# objects
        var markdownDocument = GetMarkdownDocument(markdownSource);

        // Format\Convert c# objects into <T> UI Components
        var formatter = new ViewFormatter<T>(_viewSupplier);
        var uiComponents = formatter.Format(markdownDocument);

        return uiComponents;
    }

    public static Block GetMarkdownDocument(TextReader markdownSource)
    {
        // Parse to usable c# objects
        var settings = CommonMarkSettings.Default.Clone();
        settings.AdditionalFeatures |= CommonMarkAdditionalFeatures.PlaceholderBracket;
        return CommonMarkConverter.Parse(markdownSource, settings);
    }
}