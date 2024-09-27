using FluentAssertions;
using Plugin.Maui.MarkdownView.Test.Mocks;

namespace Plugin.Maui.MarkdownView.Test;

[TestClass]
public class MarkdownParserListComponentSpecs
{
    [TestMethod]
    [Ignore]
    public void When_parsing_a_list_it_should_output_a_specific_bullet()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = @" 
  * item1
  * item2
  * item3
";
        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        throw new NotImplementedException();
    }

    [TestMethod]
    public void When_parsing_a_nested_list_it_should_output_a_specific_nesting()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = @"  
1. Dog
    1. German Shepherd
    2. Belgian Shepherd
";
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
        var view = parseResult.First();
        ContainsCount(view, "listview>:").Should().Be(2);
        ContainsCount(view, "<listview").Should().Be(2);
        ContainsCount(view, "listitemview:").Should().Be(3);

        var splittedViews = view.Split(':');
        splittedViews[0].Should().Be("listview>");
        splittedViews[1].Should().Be("-listitemview");
        splittedViews[2].Should().Be("_True.1.1_stackview>");
        splittedViews[3].Should().Be("+textview");
        splittedViews[4].Should().Be("Dog+listview>");
        splittedViews[5].Should().Be("-listitemview");
        splittedViews[6].Should().Be("_True.2.1_textview");
        splittedViews[7].Should().Be("German Shepherd-listitemview");
        splittedViews[8].Should().Be("_True.2.2_textview");
        splittedViews[9].Should().Be("Belgian Shepherd<listview<stackview<listview");
    }

    private int ContainsCount(string withinView, string searchValue)
    {
        return withinView.Select((c, i) => withinView.Substring(i))
            .Count(sub => sub.StartsWith(searchValue));
    }

}