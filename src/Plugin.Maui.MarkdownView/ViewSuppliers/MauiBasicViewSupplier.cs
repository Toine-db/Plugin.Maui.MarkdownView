using MarkdownParser.Models;
using Plugin.Maui.MarkdownView.Common;
using Plugin.Maui.MarkdownView.Controls;

namespace Plugin.Maui.MarkdownView.ViewSuppliers;

public class MauiBasicViewSupplier : IMauiViewSupplier
{
	public MauiBasicViewSupplierStyles? Styles { get; set; }

	public string? BasePathForRelativeUrlConversion { get; set; }
	public string[]? PrefixesToIgnoreForRelativeUrlConversion { get; set; }

	public IEnumerable<MarkdownReferenceDefinition>? PublishedMarkdownReferenceDefinitions { get; private set; }

	public bool IgnoreSafeArea { get; set; }

	public bool DoesReferenceDefinitionsContainPlaceholder(string placeholderId)
	{
		if (string.IsNullOrWhiteSpace(placeholderId))
		{
			return false;
		}

		var isKnown = PublishedMarkdownReferenceDefinitions?.Any(x => x.PlaceholderId == placeholderId) ?? false;
		return isKnown;
	}

	public virtual void OnReferenceDefinitionsPublished(
		IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
	{
		PublishedMarkdownReferenceDefinitions = markdownReferenceDefinitions;
	}

	public virtual View? CreateTextView(TextBlock textBlock)
	{
		var content = textBlock.ExtractLiteralContent(Environment.NewLine);
		var textViewStyle = GetTextBlockStyleFor(textBlock);

		var textview = new Label
		{
			Style = textViewStyle,
			Text = content
		};

		return textview;
	}

	public virtual View? CreateBlockquotesView(View? childView)
	{
		if (childView == null)
		{
			return null;
		}

		var innerBlockView = new Grid
		{
			Style = Styles?.BlockquotesInnerViewStyle,
			IgnoreSafeArea = IgnoreSafeArea,
			Children = { childView }
		};

		var outerBlockView = new Grid()
		{
			Style = Styles?.BlockquotesOuterViewStyle,
			IgnoreSafeArea = IgnoreSafeArea,
			Children = { innerBlockView }
		};

		return outerBlockView;
	}

	public virtual View? CreateHeaderView(TextBlock textBlock, int headerLevel)
	{
		var content = textBlock.ExtractLiteralContent(Environment.NewLine);

		var style = headerLevel switch
		{
			1 => Styles?.HeaderViewLevel1Style,
			2 => Styles?.HeaderViewLevel2Style,
			3 => Styles?.HeaderViewLevel3Style,
			4 => Styles?.HeaderViewLevel4Style,
			5 => Styles?.HeaderViewLevel5Style,
			6 => Styles?.HeaderViewLevel6Style,
			_ => null
		};

		var headingId = content.Replace(" ", "-")
			.RemoveSpecialCharactersExcept(['-']);

		var header = new HeaderLabel
		{
			Style = style,
			Text = content,
			HeadingId = headingId
		};

		return header;
	}

	public virtual View? CreateImageView(string url, string subscription, string imageId)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return new Label { Text = "Image URL is empty" };
		}

		url = ConvertToAbsoluteUrlIfPossible(url);

		var stackLayout = new VerticalStackLayout
		{
			Style = Styles?.ImageLayoutViewStyle,
			IgnoreSafeArea = IgnoreSafeArea
		};

		var imageView = new Image
		{
			Style = Styles?.ImageViewStyle
		};

		if (url.HasHttp())
		{
			try
			{
				var uri = new Uri(url);
				imageView.Source = ImageSource.FromUri(uri);
			}
			catch
			{
				// fallback on default ImageSource
			}
		}

		imageView.Source ??= ImageSource.FromFile(url);
		stackLayout.Add(imageView);

		if (!string.IsNullOrWhiteSpace(subscription))
		{
			var subscriptionLabel = new Label
			{
				Style = Styles?.ImageSubscriptionViewStyle,
				Text = subscription
			};
			stackLayout.Add(subscriptionLabel);
		}

		return stackLayout;
	}

	public virtual View? CreateListItemView(View? childView, bool isOrderedList, int sequenceNumber, int listLevel)
	{
		if (childView == null)
		{
			return null;
		}

		var layout = new Grid()
		{
			Style = Styles?.ListItemViewStyle,
			IgnoreSafeArea = IgnoreSafeArea,
			ColumnDefinitions =
			[
				new ColumnDefinition { Width = GridLength.Auto },
				new ColumnDefinition { Width = GridLength.Star }
			]
		};

		var bulletView = CreateListItemBullet(isOrderedList, sequenceNumber, listLevel);
		Grid.SetColumn(bulletView, 0);
		Grid.SetColumn(childView, 1);

		layout.Children.Add(bulletView);
		layout.Children.Add(childView);

		return layout;
	}

	public virtual View? CreateListView(List<View?> items)
	{
		if (items == null
		    || !items.Any())
		{
			return null;
		}

		var stackLayout = new VerticalStackLayout
		{
			Style = Styles?.ListViewStyle,
			IgnoreSafeArea = IgnoreSafeArea
		};

		foreach (var view in items)
		{
			if (view is not null)
			{
				stackLayout.Children.Add(view);
			}
		}

		return stackLayout;
	}

	public virtual View? CreateFencedCodeBlock(TextBlock textBlock, string codeInfo)
	{
		var content = textBlock.ExtractLiteralContent(Environment.NewLine);
		var label = new Label
		{
			Style = Styles?.FencedCodeBlockLabelStyle,
			Text = content
		};

		var blockView = new Border
		{
			Style = Styles?.FencedCodeBlockLayoutStyle,
			Content = label
		};

		return blockView;
	}

	public virtual View? CreateIndentedCodeBlock(TextBlock textBlock)
	{
		var content = textBlock.ExtractLiteralContent(Environment.NewLine);
		var label = new Label
		{
			Style = Styles?.IndentedCodeBlockLabelStyle,
			Text = content
		};

		var blockView = new Border
		{
			Style = Styles?.IndentedCodeBlockLayoutStyle,
			Content = label
		};

		return blockView;
	}

	public virtual View? CreateHtmlBlock(TextBlock textBlock)
	{
		var content = textBlock.ExtractLiteralContent(Environment.NewLine);
		var label = new Label
		{
			Style = Styles?.HtmlBlockLabelStyle,
			Text = content
		};

		var blockView = new Grid
		{
			Style = Styles?.HtmlBlockLayoutStyle,
			IgnoreSafeArea = IgnoreSafeArea,
			Children = { label }
		};

		return blockView;
	}

	public virtual View? CreateStackLayoutView(List<View?> childViews)
	{
		if (childViews == null
		    || !childViews.Any())
		{
			return null;
		}

		var stackLayout = new VerticalStackLayout
		{
			Style = Styles?.LayoutViewStyle,
			IgnoreSafeArea = IgnoreSafeArea
		};

		foreach (var view in childViews)
		{
			stackLayout.Children.Add(view);
		}

		return stackLayout;
	}

	public virtual View? CreateThematicBreak()
	{
		return new BoxView
		{
			Style = Styles?.ThematicBreakStyle
		};
	}

	public virtual void Clear()
	{
		PublishedMarkdownReferenceDefinitions = [];
	}

	protected virtual View? CreateListItemBullet(bool isOrderedList, int sequenceNumber, int listLevel)
	{
		var bulletStyle = listLevel switch
		{
			1 => Styles?.ListItemBulletViewLevel1Style,
			2 => Styles?.ListItemBulletViewLevel2Style,
			3 => Styles?.ListItemBulletViewLevel3Style,
			4 => Styles?.ListItemBulletViewLevel4Style,
			5 => Styles?.ListItemBulletViewLevel5Style,
			6 => Styles?.ListItemBulletViewLevel6Style,
			_ => null
		};

		var isListLevelEvenNumber = listLevel % 2 == 0;

		string bulletText;
		if (isOrderedList)
		{
			if (!isListLevelEvenNumber)
			{
				bulletText = $"{sequenceNumber}.";
			}
			else
			{
				var alphabet = "abcdefghijklmnopqrstuvwxyz";
				bulletText = alphabet[sequenceNumber - 1].ToString();
			}
		}
		else
		{
			// "\u229a" ⊚  "\ud83d\udd18" 🔘  "\u25cb" ○   "\u25c9" ◉  "\u25e6" ◦ 
			// "\u26ab" ⚫  "\u25cf" ●  "•" •
			bulletText = isListLevelEvenNumber
				? "\u25cb"
				: "\u25cf";
		}

		var bullet = new Label
		{
			Style = bulletStyle,
			Text = bulletText
		};

		return bullet;
	}

	protected virtual string ConvertToAbsoluteUrlIfPossible(string url)
	{
		if (string.IsNullOrWhiteSpace(BasePathForRelativeUrlConversion)
		    || string.IsNullOrWhiteSpace(url))
		{
			return url;
		}

		if (PrefixesToIgnoreForRelativeUrlConversion == null
		    || PrefixesToIgnoreForRelativeUrlConversion.Any(prefix => url.StartsWith(prefix)))
		{
			return url;
		}

		var isAbsolutePath = Uri.TryCreate(url, UriKind.Absolute, out _);
		if (isAbsolutePath)
		{
			return url;
		}

		var rootPath = BasePathForRelativeUrlConversion.TrimEnd('/').TrimEnd('\\');
		var subPath = url.TrimStart('/').TrimStart('\\');
		var absolutePath = $"{rootPath}/{subPath}";

		return absolutePath;
	}

	protected virtual Style? GetTextBlockStyleFor(TextBlock textBlock)
	{
		if (textBlock.AncestorsTree.Contains(BlockType.Quote))
		{
			return Styles?.BlockquotesTextViewStyle;
		}
		else if (textBlock.AncestorsTree.Contains(BlockType.List))
		{
			return Styles?.ListTextViewStyle;
		}

		return Styles?.TextViewStyle;
	}
}