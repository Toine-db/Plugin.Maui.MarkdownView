using MarkdownParser.Models;
using MarkdownParser.Models.Segments.Indicators;
using MarkdownParser.Models.Segments;
using Plugin.Maui.MarkdownView.Common;
using Plugin.Maui.MarkdownView.Controls;
using Microsoft.Extensions.Logging;
using Plugin.Maui.MarkdownView.Resources.Styles;

namespace Plugin.Maui.MarkdownView.ViewSuppliers;

public class MauiFormattedTextViewSupplier : MauiBasicViewSupplier
{
    private MauiFormattedTextViewSupplierStyles? _formattedTextStyles;
    public MauiFormattedTextViewSupplierStyles FormattedTextStyles
    {
        get
        {
            if (_formattedTextStyles == null)
            {
                _formattedTextStyles = new DefaultMauiFormattedTextViewSupplierStyles();
                var logger = ServiceHelper.GetLogger<MauiFormattedTextViewSupplier>();
                logger?.Log(LogLevel.Warning, "Don't be a copycat and create your own styles! \r\n" +
                                              "How to set styles in XAML:\r\n        <MarkdownView>\r\n            <MarkdownView.ViewSupplier>\r\n                <MauiFormattedTextViewSupplier>\r\n                    <MauiFormattedTextViewSupplier.FormattedTextStyles>\r\n                        <styles:DefaultMauiFormattedTextViewSupplierStyles />\r\n                    </MauiFormattedTextViewSupplier.FormattedTextStyles>\r\n                    <MauiBasicViewSupplier.Styles>\r\n                        <styles:DefaultMauiBasicViewSupplierStyles />\r\n                    </MauiBasicViewSupplier.Styles>\r\n                </MauiFormattedTextViewSupplier>\r\n            </MarkdownView.ViewSupplier>\r\n        </MarkdownView>");
            }
			return _formattedTextStyles;
        }
        set => _formattedTextStyles = value;
    }

    public Func<HyperlinkSpan, Task>? OnHyperlinkTappedFallback { get; set; }

	public override View? CreateTextView(TextBlock textBlock)
	{
		var textViewStyle = GetTextBlockStyleFor(textBlock);
		var spanStyle = GetSpanStyleFor(textBlock);

		List<View?> collectedRootViews = [];
		List<Span> textSpansCache = [];

		List<SegmentIndicator> activeIndicators = [];
		LinkSegment? activeLinkSegment = null;

		foreach (var segment in textBlock.TextSegments)
		{
			// Placeholders like [my placeholder] can be one of two types:
			// 1. Inline placeholder like a hyperlink (from ReferenceDefinitions) => this is handled by the default span and label creation 
			// 2. Designated Placeholder to be a custom view => this is handled by the TryCreateDesignatedPlaceholderView method
			if (segment is PlaceholderSegment placeholderSegment
			    && !DoesReferenceDefinitionsContainPlaceholder(placeholderSegment.PlaceholderId))
			{
				// Designated Placeholders cannot be placed inside a label
				// because of this, we need to break current formatted span/label creation to inject the placeholder in between
				var designatedPlaceholderView = CreateDesignatedPlaceholderView(placeholderSegment);

				// flush current Formatted spans if any
				var view = TryCreateFormattedLabel(textSpansCache, textViewStyle);
				collectedRootViews.Add(view);
				textSpansCache.Clear();

				// add designated Placeholder between to (possible) TextViews
				collectedRootViews.Add(designatedPlaceholderView);

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

		var textview = TryCreateFormattedLabel(textSpansCache, textViewStyle);
		collectedRootViews.Add(textview);

		var flattenedView = FlattenToSingleView(collectedRootViews);
		return flattenedView;
	}

	protected virtual Span CreateSpan(BaseSegment segment,
		List<SegmentIndicator> activeIndicators,
		LinkSegment? activeLinkSegment,
		Style? spanStyle)
	{
		var literalContent = segment.ToString();
		Span? span = null;

		if (segment is PlaceholderSegment placeholderTextBlockSegment
		    && DoesReferenceDefinitionsContainPlaceholder(placeholderTextBlockSegment.PlaceholderId))
		{
			var foundReferenceDefinition = PublishedMarkdownReferenceDefinitions?
				.First(x => x.PlaceholderId == placeholderTextBlockSegment.PlaceholderId);

			if (foundReferenceDefinition != null)
			{
				var url = foundReferenceDefinition.Url ?? foundReferenceDefinition.Title ?? string.Empty;

				span = new HyperlinkSpan
				{
					Text = literalContent,
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
		}
		else if (activeIndicators.Contains(SegmentIndicator.Link))
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

		// default span creation
		span ??= new Span
		{
			Text = literalContent,
			Style = spanStyle
		};

		return span;
	}

	protected virtual void AddFormattingToSpan(Span span, List<SegmentIndicator> activeIndicators)
	{
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
	}

	protected virtual View? CreateDesignatedPlaceholderView(PlaceholderSegment placeholderSegment)
	{
		var mySpecialView = new Label
		{
			Style = Styles?.TextViewStyle,
			Text = $">>>> placeholder '{placeholderSegment.PlaceholderId}' not found <<<<"
		};

		return mySpecialView;
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

			if (OnHyperlinkTappedFallback != null)
			{
				await OnHyperlinkTappedFallback(hyperlinkSpan);
				return;
			}

			throw new NotImplementedException(
				"No fallback for OnHyperlinkTappedAsync available in OnHyperlinkTappedFallback");
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

		if (textBlock.AncestorsTree.Contains(BlockType.List))
		{
			return FormattedTextStyles?.SpanListTextViewStyle;
		}

		return FormattedTextStyles?.SpanTextViewStyle;
	}

	protected virtual View? TryCreateFormattedLabel(List<Span> textSpans, Style? style)
	{
		if (!textSpans.Any())
		{
			return null;
		}

		var formattedString = new FormattedString();
		foreach (var textSpan in textSpans)
		{
			formattedString.Spans.Add(textSpan);
		}

		var textview = new Label
		{
			Style = style,
			FormattedText = formattedString
		};

		return textview;
	}

	protected virtual View? FlattenToSingleView(List<View?> views)
	{
		var validViews = views.Where(x => x != null).ToList();

		switch (validViews.Count)
		{
			case 0:
				return null;
			case 1:
				return validViews.First();
			default:
				var stackLayout = new VerticalStackLayout
				{
					Style = FormattedTextStyles?.LayoutForSplitTextViewStyle,
					IgnoreSafeArea = IgnoreSafeArea
				};

				foreach (var view in validViews)
				{
					if (view is not null)
					{
						stackLayout.Add(view);
					}
				}

				return stackLayout;
		}
	}
}