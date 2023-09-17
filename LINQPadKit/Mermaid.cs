using LINQPad;
using System.Collections;
using System.Reflection;

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
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "..",
                "..",
                "content",
                "scripts",
                $"mermaid@{Version}.min.js"
            ));

            Util.HtmlHead.AddScript("mermaid.initialize({ startOnLoad: false });");
            Util.HtmlHead.AddScript("""
mermaid.initialize({ startOnLoad: false });
window.call_mermaid = function(id) {
    var el = document.getElementById(id);
    if (el.hasAttribute('data-processed')) {
        el.removeAttribute('data-processed');
    }
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
                HtmlElement.InvokeScript(false, "call_mermaid", HtmlElement.ID);
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
