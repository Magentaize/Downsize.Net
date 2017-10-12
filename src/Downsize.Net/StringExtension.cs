using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DownsizeNet
{
    [DebuggerStepThrough]
    internal static class StringExtension
    {
        public static bool IsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        public static Match Match(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Match(input, pattern, options);
        }
    }
}