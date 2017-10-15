using Xunit;

namespace DownsizeNet.Test
{
    public class AppendingTest
    {
        [Fact]
        void WordTruncationAppend()
        {
            var result = Downsize.Substring("<p>abcdefghij</p><p>klmnop</p><p>qrs</p>",
                new DownsizeOptions(
                    characters: 15,
                    append: "..."));
            Assert.Equal("<p>abcdefghij</p><p>klmno...</p>", result);
        }

        [Fact]
        void CharacterTruncationAppend()
        {
            var result = Downsize.Substring("<p>here's some text.</p>",
                new DownsizeOptions(
                    words: 2,
                    append: "... (read more)"));
            Assert.Equal("<p>here's some... (read more)</p>", result);
        }

        [Fact]
        void NotAppendEllipsisWhereNotRequired()
        {
            var result = Downsize.Substring("<p>here's some text.</p>",
                new DownsizeOptions(
                    words: 5,
                    append: "..."));
            Assert.Equal("<p>here's some text.</p>", result);
        }

        [Fact]
        void WordTruncationAppendWithoutHtml()
        {
            var result = Downsize.Substring("here's some text.",
                new DownsizeOptions(
                    words: 2,
                    append: "..."));
            Assert.Equal("here's some...", result);
        }

        [Fact]
        void CharacterTruncationAppendWithoutHtml()
        {
            var result = Downsize.Substring("here's some text.",
                new DownsizeOptions(
                    characters: 6,
                    append: "..."));
            Assert.Equal("here's...", result);
        }
    }
}