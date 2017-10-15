using Xunit;

namespace DownsizeNet.Test
{
    public class RoundingTest
    {
        [Fact]
        void RoundSentenceUp()
        {
            var result = Downsize.Substring("<p>abcdefghij</p><p>klmnop</p><p>qrs</p>",
                new DownsizeOptions(
                    characters: 15,
                    round: true));
            Assert.Equal("<p>abcdefghij</p><p>klmnop</p>", result);
        }

        [Fact]
        void SentencesShorterThanRequired()
        {
            var result = Downsize.Substring("<p>here's some text.</p>",
                new DownsizeOptions(
                    words: 5,
                    round: true));
            Assert.Equal("<p>here's some text.</p>", result);
        }

        [Fact]
        void RoundUpToEndOfSentence()
        {
            var result = Downsize.Substring("<p>here's <em>some</em> text.</p>",
                new DownsizeOptions(
                    characters: 2,
                    round: true));
            Assert.Equal("<p>here's <em>some</em> text.</p>", result);
        }

        [Fact]
        void NoTrailingEmptyTags()
        {
            var result = Downsize.Substring("<p>characters</p><i>what</i>",
                new DownsizeOptions(
                    characters: 10,
                    round: true));
            Assert.Equal("<p>characters</p>", result);
        }
    }
}