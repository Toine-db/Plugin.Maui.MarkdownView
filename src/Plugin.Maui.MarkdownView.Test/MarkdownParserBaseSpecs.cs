using FluentAssertions;
using Plugin.Maui.MarkdownView.Test.Mocks;

namespace Plugin.Maui.MarkdownView.Test;

[TestClass]
public class MarkdownParserBaseSpecs
{
    [TestMethod]
    public void When_starting_a_parse_it_should_minimal_output_one_item()
    {
        //-----------------------------------------------------------------------------------------------------------
        // Arrange
        //-----------------------------------------------------------------------------------------------------------
        var markdown = "hello markdown";

        var mockComponentSupplier = new StringViewSupplier();
        var parser = new MarkdownParser<string>(mockComponentSupplier);

        //-----------------------------------------------------------------------------------------------------------
        // Act
        //-----------------------------------------------------------------------------------------------------------
        var parseResult = parser.Parse(markdown);

        //-----------------------------------------------------------------------------------------------------------
        // Assert
        //-----------------------------------------------------------------------------------------------------------
        parseResult.Count.Should().BeGreaterThan(0);
    }
}