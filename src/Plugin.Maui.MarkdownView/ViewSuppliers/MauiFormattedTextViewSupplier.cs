using MarkdownParser.Models;
using MarkdownParser.Models.Segments.Indicators;
using MarkdownParser.Models.Segments;
using Plugin.Maui.MarkdownView.Common;
using Plugin.Maui.MarkdownView.Controls;
using Microsoft.Extensions.Logging;

namespace Plugin.Maui.MarkdownView.ViewSuppliers;

public class MauiFormattedTextViewSupplier : MauiBasicViewSupplier
{
    public MauiFormattedTextViewSupplierStyles? FormattedTextStyles { get; set; }

    public override View CreateTextView(TextBlock textBlock)
    {
        var textViewStyle = GetTextBlockStyleFor(textBlock);
        var spanStyle = GetSpanStyleFor(textBlock);

        var formattedString = new FormattedString();

        var activeIndicators = new List<SegmentIndicator>();
        LinkSegment? activeLinkSegment = null;

        foreach (var textBlockSegment in textBlock.TextSegments)
        {
            var literalContent = textBlockSegment.ToString();
            
            if (textBlockSegment is IndicatorSegment indicatorSegment)
            {
                switch (indicatorSegment.Indicator)
                {
                    case SegmentIndicator.Strong:
                    case SegmentIndicator.Italic:
                    case SegmentIndicator.Strikethrough:
                    case SegmentIndicator.Code:
                    case SegmentIndicator.Link:
                        if (indicatorSegment.IndicatorPosition == SegmentIndicatorPosition.Start)
                        {
                            activeIndicators.Add(indicatorSegment.Indicator);
                        }
                        else
                        {
                            activeIndicators.RemoveAll(x => x == indicatorSegment.Indicator);
                        }

                        if (indicatorSegment.Indicator == SegmentIndicator.Link)
                        {
                            activeLinkSegment = indicatorSegment as LinkSegment;
                        }

                        break;
                    case SegmentIndicator.LineBreak:
                        formattedString.Spans.Add(new Span { Text = Environment.NewLine });
                        break;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(literalContent))
            {
                Span span;
                if (activeIndicators.Contains(SegmentIndicator.Link))
                {
                    var text = string.IsNullOrWhiteSpace(activeLinkSegment?.Title) 
                                    ? literalContent
                                    : activeLinkSegment.Title;
                    var url = activeLinkSegment?.Url ?? string.Empty;

                    span = new HyperlinkSpan
                    {
                        Text = text,
                        Url = url,
                        Style = spanStyle,
                        TextDecorations = TextDecorations.Underline,
                        Command = new Command<HyperlinkSpan>(OnHyperlinkTappedAsync)
                    };

                    if (FormattedTextStyles?.SpanHyperlinkTextLightColor != null
                        && FormattedTextStyles?.SpanHyperlinkTextDarkColor != null)
                    {
                        span.SetAppThemeColor(Span.TextColorProperty,
                            FormattedTextStyles.SpanHyperlinkTextLightColor,
                            FormattedTextStyles.SpanHyperlinkTextDarkColor);
                    }
                }
                else
                {
                    span = new Span
                    {
                        Text = literalContent,
                        Style = spanStyle
                    };
                }
                
                if (activeIndicators.Contains(SegmentIndicator.Italic)
                    && activeIndicators.Contains(SegmentIndicator.Strong))
                {
                    span.FontAttributes = FontAttributes.Italic | FontAttributes.Italic;
                }
                else if (activeIndicators.Contains(SegmentIndicator.Strong))
                {
                    span.FontAttributes = FontAttributes.Bold;
                }
                else if (activeIndicators.Contains(SegmentIndicator.Italic))
                {
                    span.FontAttributes = FontAttributes.Italic;
                }

                if (activeIndicators.Contains(SegmentIndicator.Code))
                {
                    span.SetAppThemeColor(Span.BackgroundColorProperty, 
                        FormattedTextStyles?.SpanCodeBackgroundLightColor ?? Colors.Transparent, 
                        FormattedTextStyles?.SpanCodeBackgroundDarkColor ?? Colors.Transparent);
                }

                if (activeIndicators.Contains(SegmentIndicator.Strikethrough))
                {
                    span.TextDecorations = TextDecorations.Strikethrough;
                }

                formattedString.Spans.Add(span);
            }
        }

        var textview = new Label
        {
            Style = textViewStyle,
            FormattedText = formattedString
        };

        return textview;
    }

    protected virtual async void OnHyperlinkTappedAsync(HyperlinkSpan hyperlinkSpan)
    {
        try
        {
            if (HyperlinkHelper.IsHeadingLink(hyperlinkSpan.Url))
            {
                await HyperlinkHelper.TryScrollToHeading(hyperlinkSpan, hyperlinkSpan.Url);
                return;
            }

            var url = ConvertToAbsoluteUrlIfPossible(hyperlinkSpan.Url);
            if (url.HasHttp())
            {
                await Launcher.OpenAsync(url);
                return;
            }

            await Launcher.OpenAsync(new OpenFileRequest(hyperlinkSpan.Text, new ReadOnlyFile(url)));
        }
        catch (Exception exception)
        {
            var logger = hyperlinkSpan.GetLogger();
            logger?.Log(LogLevel.Error, "MauiFormattedTextViewSupplier fault: {exception}", exception);
        }
    }

    protected virtual Style? GetSpanStyleFor(TextBlock textBlock)
    {
        if (textBlock.AncestorsTree.Contains(BlockType.Quote))
        {
            return FormattedTextStyles?.SpanBlockquotesTextViewStyle;
        }
        else if (textBlock.AncestorsTree.Contains(BlockType.List))
        {
            return FormattedTextStyles?.SpanListTextViewStyle;
        }

        return FormattedTextStyles?.SpanTextViewStyle;
    }
}