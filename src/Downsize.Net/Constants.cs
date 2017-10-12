using System.Collections.Generic;

namespace DownsizeNet
{
    internal static class Constants
    {
        // Nodes which should be considered implicitly self-closing
        // Taken from http://www.whatwg.org/specs/web-apps/current-work/multipage/syntax.html#void-elements
        public static readonly IList<string> VoidElements = new List<string>
        {
            "area", "base", "br", "col", "command", "embed", "hr", "img", "input",
            "keygen", "link", "meta", "param", "source", "track", "wbr"
        };

        // These tags are intended to be sufficient to provide ghost markdown
        // construct level context.
        // http://support.ghost.org/markdown-guide/
        public static readonly IList<string> DefaultContextualTags = new List<string>{
            "p", "ul", "ol", "pre", "blockquote",
            "h1", "h2", "h3", "h4", "h5", "h6"
        };
    }
}