using FluentAssertions;
using Plugin.Maui.MarkdownView.Test.Mocks;

namespace Plugin.Maui.MarkdownView.Test;

[TestClass]
public class MarkdownParserSingleComponentSpecs
{
    [TestMethod]
    public void When_parsing_paragraphs_it_should_output_a_textview()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = "hello";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult.First().Should().StartWith("textview:hello");
    }

    [TestMethod]
    public void When_parsing_header_it_should_output_a_headerview()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = @"## An h2 header ##";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult[0].Should().Be("headerview:2:An h2 header");
    }

    [TestMethod]
    public void When_parsing_image_it_should_output_a_imageview()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = "![example image](http://Example.jpg )";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult.First().Should().Be("imageview:http://Example.jpg:");
    }

    [TestMethod]
    public void When_parsing_image_with_subtitle_it_should_output_a_imageview_and_textview()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = "![example image](http://Example.jpg \"some comment\")";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult.First().Should().Be("imageview:http://Example.jpg:some comment");
    }

    [TestMethod]
    public void When_parsing_a_listitem_it_should_output_a_listview_with_single_item()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = @" * item9  ";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult.First().Should().StartWith("listview>:");
        parseResult.First().Should().EndWith("<listview");

        var listitemview = parseResult.First().Substring("listview>:".Length + 1); // +1 to remove the '-' before 'listitemview'
        listitemview = listitemview.Substring(0, listitemview.Length - "<listview".Length);
        listitemview.Should().StartWith("listitemview:");

        var listitemviewCount = listitemview.Split('-').Length;
        listitemviewCount.Should().Be(1);

        var listitemviewChildren = listitemview.Substring("-listitemview:".Length).Split('_');
        listitemviewChildren[0].Should().Be("False.1.1");
        listitemviewChildren[1].Should().Be("textview:item9");
    }

    [TestMethod]
    public void When_parsing_a_list_it_should_output_a_listview()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = @"  
* item4  
* item5  
* item6";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult.First().Should().StartWith("listview>:");

        var listitems = parseResult.First().Substring(parseResult.First().IndexOf("-listitemview") + 1).Split('-');
        listitems.Length.Should().Be(3);

        parseResult.Last().Should().EndWith("<listview");
    }

    [TestMethod]
    public void When_parsing_a_block_it_should_output_a_blockview()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = @" 
> Blockquotes are very handy in email to emulate reply text.
> This line is part of the same quote.

";

        var expectedResult = "Blockquotes are very handy in email to emulate reply text.";
        expectedResult += Environment.NewLine;
        expectedResult += "This line is part of the same quote.";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult.First().Should().StartWith("blockquoteview>:");
        parseResult.First().Should().EndWith("<blockquoteview");

        var content = parseResult.First().Substring("blockquoteview>:".Length);
        content = content.Substring(0, content.Length - "<blockquoteview".Length);
        content.Should().StartWith("textview:");

        var contentText = content.Substring("textview:".Length);
        contentText.Should().Be(expectedResult);
    }

    [TestMethod]
    public void When_parsing_a_ThematicBreak_it_should_output_a_ThematicBreakView()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = @" --- ";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult.First().Should().Be("thematicbreakview");
    }

    [TestMethod]
    public void When_parsing_a_placeholder_it_should_output_a_PlaceHolderView()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = @"[my-placeholder] ";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().Be(1);
        parseResult.First().Should().Be("placeholderview:my-placeholder");
    }
}