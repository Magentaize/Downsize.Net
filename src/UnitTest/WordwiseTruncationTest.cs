using DownsizeNet;
using Xunit;

namespace UnitTest
{
    public class WordwiseTruncationTest
    {
        [Fact]
        void TaglessText()
        {
            var result = Downsize.Substring("this is a test of tagless input", new DownsizeOptions(words: 5));
            Assert.Equal("this is a test of", result);
        }

        [Fact]
        void NestedTag()
        {
            var result = Downsize.Substring("<p>this is a <strong>test of word downsizing</strong></p>",
                new DownsizeOptions(words: 5));
            Assert.Equal("<p>this is a <strong>test of</strong></p>", result);
        }

        [Fact]
        void SingleQuote()
        {
            var result = Downsize.Substring("<p><img src=\"/someUrl.jpg\" alt=\"Let's get in!\"></p><p>hello world</p>",
                new DownsizeOptions(words: 1));
            Assert.Equal("<p><img src=\"/someUrl.jpg\" alt=\"Let's get in!\"></p><p>hello</p>", result);
        }

        [Fact]
        void BalanceMarkup()
        {
            var result =
                Downsize.Substring("<p><p><p><p>this is a <strong>test of word downsizing</strong> some stuff</p>",
                    new DownsizeOptions(words: 5));
            Assert.Equal("<p><p><p><p>this is a <strong>test of</strong></p></p></p></p>", result);
        }

        [Fact]
        void IgnoreErroneouslyUnescapedCarets()
        {
            var result = Downsize.Substring("<p>this < is a <strong>test of word downsizing</strong> some stuff</p>",
                new DownsizeOptions(words: 5));
            Assert.Equal("<p>this < is a <strong>test of</strong></p>", result);

            result= Downsize.Substring("<p>this < is a > test < test > <strong>test of word downsizing</strong> some stuff</p>",
                new DownsizeOptions(words: 5));
            Assert.Equal("<p>this < is a > test < test</p>", result);
        }

        [Fact]
        void Comments()
        {
            var result = Downsize.Substring("<p>this <!-- is a > test < test --> <strong>test of word downsizing</strong> some stuff</p>",
                new DownsizeOptions(words: 2));
            Assert.Equal("<p>this <!-- is a > test < test --> <strong>test</strong></p>", result);
        }

        [Fact]
        void VoidTag()
        {
            var result = Downsize.Substring("<p>test <img src=\"blah.jpg\"> <strong>stuffo</strong> some stuff</p>",
                new DownsizeOptions(words: 2));
            Assert.Equal("<p>test <img src=\"blah.jpg\"> <strong>stuffo</strong></p>", result);
        }

        [Fact]
        void NotCloseSelfclosingTags()
        {
            var result = Downsize.Substring("<p>test <random selfclosing /> <strong>stuffo</strong> some stuff</p>",
                new DownsizeOptions(words: 2));
            Assert.Equal("<p>test <random selfclosing /> <strong>stuffo</strong></p>", result);

            result = Downsize.Substring("<p>test <random selfclosing / > <strong>stuffo</strong> some stuff</p>",
                new DownsizeOptions(words: 2));
            Assert.Equal("<p>test <random selfclosing / > <strong>stuffo</strong></p>", result);
        }

        [Fact]
        void CloseUnknownTags()
        {
            var result = Downsize.Substring("<p>test <unknown> <strong>stuffo</strong> some stuff</p>",
                new DownsizeOptions(words: 2));
            Assert.Equal("<p>test <unknown> <strong>stuffo</strong></unknown></p>", result);
        }
    }
}