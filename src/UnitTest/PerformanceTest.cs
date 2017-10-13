using System.Diagnostics;
using System.Text;
using DownsizeNet;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest
{
    public class PerformanceTest
    {
        private readonly ITestOutputHelper output;

        public PerformanceTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        void Test()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 1000000; i++)
            {
                sb.Append("<p>word truncate performance test</p>\n");
            }
            var perfTestSeed = sb.ToString();

            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            var options = new DownsizeOptions(words: 5);
            for (var i = 0; i < 1000000; i++)
            {
                Downsize.Substring(perfTestSeed, options);
            }

            sw.Stop();

            output.WriteLine($"Operation time: {sw.ElapsedMilliseconds}");
        }
    }
}