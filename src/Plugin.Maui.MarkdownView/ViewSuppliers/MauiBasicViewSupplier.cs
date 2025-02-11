using MarkdownParser.Models;

using Plugin.Maui.MarkdownView.Common;
using Plugin.Maui.MarkdownView.Controls;

namespace Plugin.Maui.MarkdownView.ViewSuppliers;

public class MauiBasicViewSupplier : IMauiViewSupplier
{
    public MauiBasicViewSupplierStyles? Styles { get; set; }

    public string? BasePathForRelativeUrlConversion { get; set; }
    public string[]? PrefixesToIgnoreForRelativeUrlConversion { get; set; }

    protected Dictionary<string, List<string>> Headings = [];

    public IEnumerable<MarkdownReferenceDefinition>? PublishedMarkdownReferenceDefinitions { get; private set; }

    public bool IgnoreSafeArea { get; set; }

    public bool DoesReferenceDefinitionsContainPlaceholder(string placeholderId)
    {
        if (string.IsNullOrWhiteSpace(placeholderId))
        {
            return false;
        }

        return PublishedMarkdownReferenceDefinitions?.Any(x => x.PlaceholderId == placeholderId) is true;
    }

    public virtual void OnReferenceDefinitionsPublished(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions)
    {
        PublishedMarkdownReferenceDefinitions = markdownReferenceDefinitions;
    }

    public virtual View CreateTextView(TextBlock textBlock)
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
        if (childView is null)
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

        var headingId = "#" + string.Join("-", content.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries))
            .RemoveSpecialCharactersExcept('-')
            .ToLower();

        if (Headings.TryGetValue(headingId, out var list))
        {
            headingId = $"{headingId}-{list.Count}";
            list.Add(headingId);
        }
        else
        {
            Headings[headingId] = [headingId];
        }

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

        ImageSource? source = null;
        if (url.TryCreateUri(out var uri))
        {
            source = ImageSource.FromUri(uri);
        }

        source ??= ImageSource.FromFile(url);

        View view = new Image
        {
            Style = Styles?.ImageViewStyle,
            Source = source
        };

        if (!string.IsNullOrWhiteSpace(subscription))
        {
            var subscriptionLabel = new Label
            {
                Style = Styles?.ImageSubscriptionViewStyle,
                Text = subscription
            };
            var stackLayout = new VerticalStackLayout
            {
                Style = Styles?.ImageLayoutViewStyle,
                IgnoreSafeArea = IgnoreSafeArea
            };
            stackLayout.Add(view);
            stackLayout.Add(subscriptionLabel);
            view = stackLayout;
        }

        return view;
    }

    public virtual View? CreateListItemView(View? childView, bool isOrderedList, int sequenceNumber, int listLevel)
    {
        if (childView is null)
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
        if (items?.Count is null or 0)
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
        if (childViews?.Count is null or 0)
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
            if (view is not null)
            {
                stackLayout.Children.Add(view);
            }
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
        Headings.Clear();
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

        if (PrefixesToIgnoreForRelativeUrlConversion?.Any(url.StartsWith) is not false)
        {
            return url;
        }

        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return url;
        }

        // Try to create absolute URI using proper URI combination
        if (Uri.TryCreate(BasePathForRelativeUrlConversion, UriKind.Absolute, out Uri? baseUri)
            && Uri.TryCreate(baseUri, url, out Uri? absoluteUri))
        {
            return absoluteUri.AbsoluteUri;
        }

        // Fallback to path combination with normalized slashes
        return string.Format("{0}/{1}",
            BasePathForRelativeUrlConversion.TrimEnd('/', '\\'),
            url.TrimStart('/', '\\'));
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