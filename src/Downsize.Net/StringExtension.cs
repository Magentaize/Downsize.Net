using System.Text.RegularExpressions;

namespace DownsizeNet
{
    internal static class StringExtension
    {
        public static bool IsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }
    }
}