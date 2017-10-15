using System.Collections.Generic;
using Xunit;

namespace DownsizeNet.Test
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

            result = Downsize.Substring(
                "<p>this < is a > test < test > <strong>test of word downsizing</strong> some stuff</p>",
                new DownsizeOptions(words: 5));
            Assert.Equal("<p>this < is a > test < test</p>", result);
        }

        [Fact]
        void Comments()
        {
            var result = Downsize.Substring(
                "<p>this <!-- is a > test < test --> <strong>test of word downsizing</strong> some stuff</p>",
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

        [Fact]
        void UnescapedCaretsInsideDoubleQuotedStrings()
        {
            var result = Downsize.Substring("<p>test string <img \"<stuffo>\"> <strong>stuffo</strong> some stuff</p>",
                new DownsizeOptions(words: 3));
            Assert.Equal("<p>test string <img \"<stuffo>\"> <strong>stuffo</strong></p>", result);
        }

        [Fact]
        void PermitUnescapedCaretsInsideSingleQuotedStrings()
        {
            var result = Downsize.Substring("<p>test string <img '<stuffo>'> <strong>stuffo</strong> some stuff</p>",
                new DownsizeOptions(words: 3));
            Assert.Equal("<p>test string <img '<stuffo>'> <strong>stuffo</strong></p>", result);
        }

        [Fact]
        void ManuallyClosedElements()
        {
            var result = Downsize.Substring(
                "<p>tag closing test</p><p>There should only</p><p>be one terminating para</p>",
                new DownsizeOptions(words: 7));
            Assert.Equal("<p>tag closing test</p><p>There should only</p><p>be</p>", result);
        }

        [Fact]
        void UnicodeChar()
        {
            var result = Downsize.Substring(
                "Рэпудёандаэ конжыквуюнтюр эю прё, нэ квуй янжольэнж квюальизквюэ чадипжкёнг. Ед кюм жкрипта",
                new DownsizeOptions(words: 3));
            Assert.Equal("Рэпудёандаэ конжыквуюнтюр эю", result);
        }

        [Fact]
        void UnicodeCharInTag()
        {
            var result = Downsize.Substring(
                "<p>Рэпудёандаэ конжыквуюнтюр эю прё, <span>нэ квуй янжольэнж квюальизквюэ</span> чадипжкёнг. Ед кюм жкрипта</p>",
                new DownsizeOptions(words: 3));
            Assert.Equal("<p>Рэпудёандаэ конжыквуюнтюр эю</p>", result);
        }

        [Fact]
        void NoTrailingEmptyTags()
        {
            var result = Downsize.Substring("<p>there are five words here</p><i>what</i>",
                new DownsizeOptions(words: 5));
            Assert.Equal("<p>there are five words here</p>", result);
        }

        [Fact]
        void AwaitTheEndOfTheContainingParagraph()
        {
            var result = Downsize.Substring(
                "<p>there are more than seven words in this paragraph</p><p>this is unrelated</p>",
                new DownsizeOptions(
                    words: 7,
                    contextualTags: new List<string>() {"p", "ul", "ol", "pre", "blockquote"}));
            Assert.Equal("<p>there are more than seven words in this paragraph</p>", result);
        }

        [Fact]
        void AwaitTheEndOfTheContainingUnorderedList()
        {
            var result = Downsize.Substring(
                "<ul><li>item one</li><li>item two</li><li>item three</li></ul><p>paragraph</p>",
                new DownsizeOptions(
                    words: 5,
                    contextualTags: new List<string>() {"p", "ul", "ol", "pre", "blockquote"}));
            Assert.Equal("<ul><li>item one</li><li>item two</li><li>item three</li></ul>", result);
        }

        [Fact]
        void ZeroWordsOption()
        {
            var result = Downsize.Substring("<p>this is a <strong>test of word downsizing</strong></p>",
                new DownsizeOptions(words: 0));
            Assert.Equal(string.Empty, result);
        }
    }
}