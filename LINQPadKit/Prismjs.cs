using LINQPad;
using LINQPad.Controls;

namespace LINQPadKit
{
    /// <summary>
    /// Use prismjs to render code.
    /// </summary>
    public static class Prismjs
    {
        private static readonly string PrismjsVersion = "1.29.0";

        /// <summary>
        /// Load scripts of Prismjs.
        /// </summary>
        public static void Script()
        {
            // https://cdnjs.com/libraries/prism
            Util.HtmlHead.AddCssLink($"https://cdnjs.cloudflare.com/ajax/libs/prism/{PrismjsVersion}/themes/prism.min.css");
            Util.HtmlHead.AddScriptFromUri($"https://cdnjs.cloudflare.com/ajax/libs/prism/{PrismjsVersion}/prism.min.js");

            Util.HtmlHead.AddScript($@"
window.highlight = function highlight(language, code) {{
    return Prism.highlight(code, Prism.languages[language], language);
}}");

            Util.HtmlHead.AddStyles(@"
.code {
	font-family: 'Consolas';
	white-space: pre-wrap;
}");
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
                Util.HtmlHead.AddScriptFromUri($"https://cdnjs.cloudflare.com/ajax/libs/prism/{PrismjsVersion}/components/prism-{language}.min.js");
            }
        }

        /// <summary>
        /// Render code.
        /// <para>(The language grammar must be loaded before use.)</para> 
        /// <see href="https://prismjs.com/#supported-languages">Supported languages</see>.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Div Render(string language, string code)
        {
            var div = new Div
            {
                CssClass = "code"
            };
            div.HtmlElement.InnerHtml = Util.InvokeScript(true, "highlight", new[] { language, code }) as string;
            return div;
        }

    }
}
