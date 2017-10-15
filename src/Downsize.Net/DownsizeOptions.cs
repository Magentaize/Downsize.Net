using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DownsizeNet
{
    public class DownsizeOptions
    {
        public const int DefaultTruncationLimit = -1;
        public const bool DefaultRound = false;

        internal string Append { get; set; }
        internal Regex WordChars { get; set; }
        internal CountType CountType { get; set; }
        internal IList<string> ContextualTags { get; set; }
        internal bool KeepContext { get; set; }
        internal int Limit { get; set; }

        public DownsizeOptions(int words = DefaultTruncationLimit, int characters = DefaultTruncationLimit, bool round = DefaultRound, Regex wordChars = null,
            IList<string> contextualTags = null, string append = null)
        {
            Append = append;
            if (words < 0 && characters <= 0)
            {
                throw new ArgumentOutOfRangeException("\"words\" or \"characters\" must be greater than ZERO.");
            }
            if (words >= 0)
            {
                CountType = CountType.Words;
                Limit = words;
            }
            else
            {
                CountType = CountType.Characters;
                Limit = characters;
            }

            WordChars = wordChars ?? new Regex("[\\p{L}0-9\\-\\']", RegexOptions.IgnoreCase);

            if (round)
            {
                contextualTags = Constants.DefaultContextualTags;
            }
            if (contextualTags == null)
            {
                KeepContext = false;
                contextualTags = new string[0];
            }
            else
            {
                KeepContext = true;
            }
            ContextualTags = contextualTags;
        }
    }
}