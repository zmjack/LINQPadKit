using LINQPad;
using System.Collections;
using System.Reflection;

namespace LINQPadKit
{
    /// <summary>
    /// Use <see cref="Prism"/>(Prismjs) to render code.
    /// </summary>
    public class Prism : Pre, IEnumerable<string>
    {
        public static readonly string Version = "1.29.0";

        /// <summary>
        /// Load scripts of <see cref="Prism"/>.
        /// </summary>
        public static void Import()
        {
            // https://cdnjs.com/libraries/prism

            Util.HtmlHead.AddScriptFromUri(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "..",
                "..",
                "content",
                "scripts",
                $"prism@{Version}.min.js"
            ));

            Util.HtmlHead.AddStyles(File.ReadAllText(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "..",
                "..",
                "content",
                "scripts",
                $"prism@{Version}.min.css"))
            );

            Util.HtmlHead.AddScript("""
window.call_prism = function(language, code) {
    return Prism.highlight(code, Prism.languages[language], language);
}
"""
            );
        }

        public string Language { get; }

        public Prism(string language)
        {
            Language = language;
            CssClass = "prism";
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                HtmlElement.InnerHtml = Util.InvokeScript(true, "call_prism", new[] { Language, value }) as string;
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
