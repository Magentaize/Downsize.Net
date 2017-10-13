using System.Collections.Generic;
using DownsizeNet;
using Xunit;

namespace UnitTest
{
    public class CharacterTruncationTest
    {
        [Fact]
        void TaglessText()
        {
            var result = Downsize.Substring("this is a test of tagless input",
                new DownsizeOptions(characters: 6));
            Assert.Equal("this i", result);
        }

        [Fact]
        void CharacterTruncateAcrossTag()
        {
            var result = Downsize.Substring("<p>abcdefghij</p><p>klmnop</p><p>qrs</p>",
                new DownsizeOptions(characters: 15));
            Assert.Equal("<p>abcdefghij</p><p>klmno</p>", result);

            result = Downsize.Substring("<p>a</p><p>b</p><p>cdefghij</p><p>klmnop</p><p>qrs</p>",
                new DownsizeOptions(characters: 15));
            Assert.Equal("<p>a</p><p>b</p><p>cdefghij</p><p>klmno</p>", result);
        }

        [Fact]
        void AwaitTheEndOfTheContainingParagraph()
        {
            var result = Downsize.Substring(
                "<p>there are many more than seven characters in this paragraph</p><p>this is unrelated</p>",
                new DownsizeOptions(
                    characters: 7,
                    contextualTags: new List<string>() {"p", "ul", "ol", "pre", "blockquote"}));
            Assert.Equal("<p>there are many more than seven characters in this paragraph</p>", result);
        }

        [Fact]
        void NoTrailingEmptyTags()
        {
            var result = Downsize.Substring("<p>characters</p><i>what</i>",
                new DownsizeOptions(characters: 10));
            Assert.Equal("<p>characters</p>", result);
        }
    }
}