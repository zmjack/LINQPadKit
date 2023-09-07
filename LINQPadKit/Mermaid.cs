using LINQPad;
using System.Collections;

namespace LINQPadKit
{
    /// <summary>
    /// Use <see cref="Mermaid"/> to render code.
    /// </summary>
    public class Mermaid : Pre, IEnumerable<string>
    {
        public static readonly string Version = "10.4.0";

        /// <summary>
        /// Load scripts of <see cref="Mermaid"/>.
        /// </summary>
        public static void Import()
        {
            Util.HtmlHead.AddScriptFromUri(Path.Combine(
                Path.GetDirectoryName(typeof(Mermaid).Assembly.Location),
                "..",
                "..",
                "content",
                "scripts",
                $"cdn.jsdelivr.net_npm_mermaid@{Version}_dist_mermaid.min.js"
            ));
            Util.HtmlHead.AddScript("mermaid.initialize({ startOnLoad: false });");

            Util.HtmlHead.AddScript("""
window.graph = function graph(id) {
    mermaid.run({ querySelector: '#' + id });
}
"""
            );
        }

        public Mermaid()
        {
            CssClass = "mermaid";
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                HtmlElement.InnerHtml = value;
                HtmlElement.InvokeScript(false, "graph", HtmlElement.ID);
            }
        }

        public void Add(string content)
        {
            Content = content;
        }

        public IEnumerator GetEnumerator()
        {
            return new[] { _content }.GetEnumerator();
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return new[] { _content }.AsEnumerable().GetEnumerator();
        }

    }
}
