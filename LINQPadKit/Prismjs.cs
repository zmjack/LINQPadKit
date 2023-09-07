using LINQPad;
using LINQPad.Controls;
using System.Collections;

namespace LINQPadKit
{
    /// <summary>
    /// Use <see cref="Prismjs"/> to render code.
    /// </summary>
    public class Prismjs : Div, IEnumerable<string>
    {
        public static readonly string Version = "1.29.0";

        /// <summary>
        /// Load scripts of <see cref="Prismjs"/>.
        /// </summary>
        public static void Import()
        {
            // https://cdnjs.com/libraries/prism
            Util.HtmlHead.AddCssLink($"https://cdnjs.cloudflare.com/ajax/libs/prism/{Version}/themes/prism.min.css");
            Util.HtmlHead.AddScriptFromUri($"https://cdnjs.cloudflare.com/ajax/libs/prism/{Version}/prism.min.js");

            Util.HtmlHead.AddScript("""
window.highlight = function highlight(language, code) {
    return Prism.highlight(code, Prism.languages[language], language);
}
"""
            );

            Util.HtmlHead.AddStyles("""
.code {
	font-family: 'Consolas';
	white-space: pre-wrap;
}
"""
            );
        }

        /// <summary>
        /// Load language grammars.
        /// <para><see href="https://prismjs.com/#supported-languages">Supported languages</see></para>
        /// </summary>
        /// <param name="languages"></param>
        public static void Load(params string[] languages)
        {
            foreach (var language in languages)
            {
                Util.HtmlHead.AddScriptFromUri($"https://cdnjs.cloudflare.com/ajax/libs/prism/{Version}/components/prism-{language}.min.js");
            }
        }

        public string Language { get; }

        public Prismjs(string language)
        {
            Language = language;
            CssClass = "code";
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                HtmlElement.InnerHtml = Util.InvokeScript(true, "highlight", new[] { Language, value }) as string;
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
