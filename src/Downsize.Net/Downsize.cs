using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DownsizeNet
{
    public static class Downsize
    {
        public static string Substring(string text, DownsizeOptions options)
        {
            var stack = new Stack<string>();
            var pointer = 0;
            var tagBuffer = string.Empty;
            var truncatedText = string.Empty;
            var parseState = default(ParseState);
            var trackedState = (UnitCount:0, CountState:false);

            var exit = false;

            for (; pointer < text.Length && !exit; pointer++)
            {
                if (parseState != ParseState.Uninitialised)
                {
                    tagBuffer += text[pointer];
                }

                switch (text[pointer])
                {
                    case '<':
                        // Ooh look — we're starting a new tag.
                        // (Provided we're in uninitialised state and the next
                        // character is a word character, explamation mark or slash)
                        if (parseState == ParseState.Uninitialised &&
                            Regex.IsMatch(text[pointer + 1].ToString(), "[a-z0-9\\-\\_\\/\\!]"))
                        {
                            if (IsAtLimit())
                            {
                                exit = true;
                                break;
                            }
                            parseState = ParseState.TagCommenced;
                            tagBuffer += text[pointer];
                        }

                        break;
                    case '!':
                        if (parseState == ParseState.TagCommenced && text[pointer + 1] == '<')
                        {
                            parseState = ParseState.Comment;
                        }

                        break;
                    case '-':
                        if (parseState == ParseState.Comment)
                            parseState = ParseState.Comment;

                        break;
                    case '\"':
                        if (parseState == ParseState.Comment)
                        {
                            parseState = ParseState.TagCommenced;
                        }
                        else if (parseState == ParseState.TagStringSingle)
                        {
                            // if double quote is found in a single quote string,
                            // ignore it and let the string finish
                            break;
                        }
                        else if (parseState != ParseState.Uninitialised)
                        {
                            parseState = ParseState.TagString;
                        }

                        break;
                    case '\'':
                        if (parseState == ParseState.TagStringSingle)
                        {
                            parseState = ParseState.TagCommenced;
                        }
                        else if (parseState == ParseState.TagString)
                        {
                            // if single quote is found in a double quote string,
                            // ignore it and let the string finish
                            break;
                        }
                        else if (parseState != ParseState.Uninitialised)
                        {
                            parseState = ParseState.TagStringSingle;
                        }

                        break;
                    case '>':
                        if (parseState == ParseState.TagCommenced)
                        {
                            parseState = ParseState.Uninitialised;
                            truncatedText += tagBuffer;
                            var tagName = GetTagName(tagBuffer);

                            // Closing tag. (Do we need to be more lenient/)
                            if (tagBuffer.IsMatch("<\\s*\\/"))
                            {
                                // We don't attempt to walk up the stack to close
                                // tags. If the text to be truncated contains
                                // malformed nesting, we just close what we're
                                // permitted to and clean up at the end.
                                if (GetTagName(stack.ElementAt(stack.Count - 1)) == tagName)
                                {
                                    stack.Pop();
                                }
                            }
                            else
                            {
                                // Nope, it's an opening tag.

                                // Don't push self closing or void elements on to
                                // the stack, since they have no effect on nesting.
                                if (Constants.VoidElements.IndexOf(tagName) < 0 && !tagBuffer.IsMatch("\\/\\s*>$"))
                                {
                                    stack.Push(tagBuffer);
                                }
                            }

                            tagBuffer = string.Empty;

                            // Closed tags are word boundries. Count!
                            if (!IsAtLimit())
                            {
                                Count("");
                                continue;
                            }
                        }
                        else if (parseState == ParseState.Comment)
                        {
                            if (text.Substring(pointer - 2, pointer) == "--")
                            {
                                parseState = ParseState.Uninitialised;
                                truncatedText += tagBuffer;
                                tagBuffer = string.Empty;

                                // Closed tags are word boundries. Count!
                                if (!IsAtLimit())
                                {
                                    Count("");
                                    continue;
                                }
                            }
                        }

                        break;
                }

                // We're not inside a tag, comment, attribute, or string.
                // This is just text.
                if (parseState == ParseState.Uninitialised)
                {
                    // Have we had enough of a good thing?
                    if (IsAtLimit())
                    {
                        // console.log("limit at: '" + text[pointer] +"'");
                        // console.log(trackedState.unitCount);
                        break;
                    }
                    Count(text[pointer].ToString());

                    // Nope, we still thirst for more.
                    truncatedText += text[pointer];
                }
            } // end of main parsing for loop

            // 'Tock' for word counting happens when next whitespace is added.
            // Strip this and any other trailing whitespace.
            // TODO: what should the whitespace behavior be?
            truncatedText = truncatedText.Trim();

            if (options.Append != null && IsAtLimit())
            {
                truncatedText += options.Append;
            }

            // Append anything still left in the buffer
            truncatedText += tagBuffer;

            // Balance anything still left on the stack
            while (stack.Count > 0)
            {
                truncatedText += CloseTag(stack.Pop());
            }

            return truncatedText;

            bool IsAtLimit()
            {
                var stackIndex = 0;

                // Return true when we've hit our limit
                if (trackedState.UnitCount < options.Limit)
                {
                    return false;
                }

                // If we've got no special context to retain, do an early return.
                if (!options.KeepContext)
                {
                    return true;
                }

                for (; stackIndex < stack.Count; stackIndex++)
                {
                    if (Convert.ToBoolean(options.ContextualTags.IndexOf(GetTagName(stack.ElementAt(stackIndex)))))
                        return false;
                }

                // There are no contextual tags left, we can stop.
                return true;
            }

            void Count(string chr)
            {
                // TODO: 'Tock' for word counting happens when next whitespace is added.
                //        i.e. it then needs stripping.
                //        Should a pointer be passed to count instead of the chr?
                //        This would allow forward lookup and allow 'Tock' on final char.
                switch (options.CountType)
                {
                    case CountType.Words:
                        bool tempCountState;
                        if ((tempCountState = options.WordChars.IsMatch(chr)) != trackedState.CountState)
                        {
                            trackedState.CountState = tempCountState;

                            // Only count the words on the "tock", or we'd be counting
                            // them twice.
                            if (!trackedState.CountState)
                            {
                                trackedState.UnitCount++;
                            }
                        }

                        break;

                    case CountType.Characters:
                        // We pass in empty values to count word boundries
                        // defined by tags.
                        // This isn't relevant to character truncation.
                        if (chr != string.Empty)
                        {
                            trackedState.UnitCount++;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static string CloseTag(string openingTag)
        {
            // Grab the tag name, including namespace, if there is one.
            var tagName = GetTagName(openingTag);

            // We didn't get a tag name, so return nothing. Better than
            // a bad prediction, or a junk tag.
            if (tagName == null)
            {
                return string.Empty;
            }

            return $"</{tagName}>";
        }

        private static string GetTagName(string tag)
        {
            var tagName = Regex.Matches(tag, "<\\/*([a-z0-9\\:\\-\\_]+)", RegexOptions.IgnoreCase);
            return tagName.Count > 0 ? tagName[1].Value : null;
        }

        private enum ParseState
        {
            Uninitialised,
            TagCommenced,
            TagString,
            TagStringSingle,
            Comment
        }
    }
}