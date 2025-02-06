using MarkdownParser.Models.Segments.Indicators;
using MarkdownParser.Models.Segments;
using MarkdownParser.Models;
using Plugin.Maui.MarkdownView.Sample.Views;
using Plugin.Maui.MarkdownView.ViewSuppliers;

namespace Plugin.Maui.MarkdownView.Sample.CustomViewSuppliers;

public class MyViewSupplier : MauiFormattedTextViewSupplier
{
    public SupplierExtraStyles? ExtraStyles { get; set; }

    public Func<string?, Task>? OnMenuItemTapped { get; set; }

    public override View? CreateHeaderView(TextBlock textBlock, int headerLevel)
    {
        var header = base.CreateHeaderView(textBlock, headerLevel);
        if (headerLevel is 1 or 2)
        {
            var bottomLine = new BoxView
            {
                Style = ExtraStyles?.MenuItemSeparatorStyle
            };

            var gridLayout = new Grid
            {
                RowDefinitions = 
                [
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                ],
                IgnoreSafeArea = IgnoreSafeArea,
                Style = headerLevel switch
                {
                    1 => ExtraStyles?.Header1LayoutViewStyle,
                    2 => ExtraStyles?.Header2LayoutViewStyle
                }
            };

            gridLayout.Add(header);
            Grid.SetRow(header, 0);

            gridLayout.Add(bottomLine);
            Grid.SetRow(bottomLine, 1);

            return gridLayout;
        }

        return header;
    }

    public override View? CreateTextView(TextBlock textBlock)
    {
        var textViewStyle = GetTextBlockStyleFor(textBlock);
        var spanStyle = GetSpanStyleFor(textBlock);

        var collectedRootViews = new List<View?>();
        var textSpansCache = new List<Span>();

        var activeIndicators = new List<SegmentIndicator>();
        LinkSegment? activeLinkSegment = null;

        var isCreatingPlaceholderMenu = false;
        MenuItemView? placeholderMenuItemCache = null;

        foreach (var segment in textBlock.TextSegments)
        {
            // Placeholders like [my placeholder] can be one of two types:
            // 1. Inline placeholder like a (hyper)link => this is handled by the default span and label creation
            // 2. Designated Placeholder to be a custom view => this is handled by the TryCreateDesignatedPlaceholderView method
            if (segment is PlaceholderSegment placeholderSegment
                && !DoesReferenceDefinitionsContainPlaceholder(placeholderSegment.PlaceholderId))
            {
                if (placeholderSegment.PlaceholderId == "MENU|START")
                {
                    isCreatingPlaceholderMenu = true;
                    FlushSpansToRootViews(ref textSpansCache, ref collectedRootViews, textViewStyle);

                    continue;
                }

                if (placeholderSegment.PlaceholderId == "MENU|END")
                {
                    isCreatingPlaceholderMenu = false;

                    continue;
                }
                
                FlushSpansToRootViews(ref textSpansCache, ref collectedRootViews, textViewStyle);

                // add designated Placeholder between to TextViews
                var designatedPlaceholderView = CreateDesignatedPlaceholderView(placeholderSegment);
                collectedRootViews.Add(designatedPlaceholderView);

                continue;
            }

            if (isCreatingPlaceholderMenu)
            {
                if (segment is LinkSegment linkSegment)
                {
                    if (linkSegment.IndicatorPosition == SegmentIndicatorPosition.Start)
                    {
                        placeholderMenuItemCache = new MenuItemView
                        {
                            Url = linkSegment.Url,
                            Icon = linkSegment.Title,
                            IgnoreSafeArea = IgnoreSafeArea,
                            Style = ExtraStyles?.MenuItemViewStyle,
                            LabelStyle = ExtraStyles?.MenuItemLabelStyle,
                            SeparatorStyle = ExtraStyles?.MenuItemSeparatorStyle,
                            OnItemTapped = async url =>
                            {
                                if (OnMenuItemTapped != null)
                                {
                                    await OnMenuItemTapped.Invoke(url);
                                }
                            }
                        };
                    }

                    if (linkSegment.IndicatorPosition == SegmentIndicatorPosition.End)
                    {
                        collectedRootViews.Add(placeholderMenuItemCache);
                        placeholderMenuItemCache = null;
                    }
                }

                if (segment.HasLiteralContent
                    && placeholderMenuItemCache != null)
                {
                    placeholderMenuItemCache.Text = segment.ToString();
                }

                continue;
            }

            // Special segments like indicators
            if (segment is IndicatorSegment indicatorSegment)
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
                        textSpansCache.Add(new Span { Text = Environment.NewLine });
                        break;
                }
            }

            // Segments with text
            if (segment.HasLiteralContent)
            {
                var span = CreateSpan(segment, activeIndicators, activeLinkSegment, spanStyle);
                AddFormattingToSpan(span, activeIndicators);

                textSpansCache.Add(span);
            }
        }

        FlushSpansToRootViews(ref textSpansCache, ref collectedRootViews, textViewStyle);

        var flattenedView = FlattenToSingleView(collectedRootViews);
        return flattenedView;
    }
    
    protected override View? CreateDesignatedPlaceholderView(PlaceholderSegment placeholderSegment)
    {
        var placeholderParts = placeholderSegment.Title.Split('|');
        if (placeholderParts.Any()
            && placeholderParts.First().Equals("audio", StringComparison.OrdinalIgnoreCase))
        {
            if (placeholderParts.Length > 1)
            {
                var audioSource = placeholderParts[1];
                var button = new AudioButton
                {
                    Text = $"Play {audioSource}",
                    AudioFileName = audioSource,
                    ButtonStyle = ExtraStyles?.ButtonViewStyle
                };

                return button;
            }
        }

        return base.CreateDesignatedPlaceholderView(placeholderSegment);
    }

    private void FlushSpansToRootViews(ref List<Span> spans, ref List<View?> rootViewSink, Style? textViewStyle)
    {
        var flushedFormattedLabel = TryCreateFormattedLabel(spans, textViewStyle);
        rootViewSink.Add(flushedFormattedLabel);
        spans.Clear();
    }
}
