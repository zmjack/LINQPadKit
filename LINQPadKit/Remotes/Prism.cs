using LINQPad;
using Microsoft.OpenApi.Extensions;
using System.Collections;

namespace LINQPadKit.Remotes;

public partial class Prism : Pre, IEnumerable<string?>
{
    public static void Import(params Lang[] languages)
    {
        if (!languages.Any()) throw new ArgumentException("The parameter can not be empty.", nameof(languages));

        Util.HtmlHead.AddCssLink("https://cdn.jsdelivr.net/npm/prismjs@1.29.0/themes/prism.min.css");
        Util.HtmlHead.AddScriptFromUri("https://cdn.jsdelivr.net/npm/prismjs@1.29.0/prism.min.js");

        foreach (var lang in languages)
        {
            Util.HtmlHead.AddScriptFromUri($"https://cdn.jsdelivr.net/npm/prismjs@1.29.0/components/prism-{lang.GetDisplayName()}.min.js");
        }

        Util.HtmlHead.AddStyles(
"""
.prism {
  white-space: pre-wrap;
}
"""
        );

        Util.HtmlHead.AddScript(
"""
window.call_prism = function(language, code) {
    return Prism.highlight(code, Prism.languages[language], language);
}
"""
        );
    }

    public Lang Language { get; }

    public Prism(Lang language)
    {
        Language = language;
        CssClass = "prism";
    }

    private string? _content;
    public string? Content
    {
        get => _content;
        set
        {
            _content = value;
            var html = Util.InvokeScript(true, "call_prism", new[] { Language.GetDisplayName(), value }) as string;
            HtmlElement.InnerHtml = html;
        }
    }

    public virtual void Add(string? content)
    {
        Content = content;
    }

    public IEnumerator GetEnumerator()
    {
        return new[] { _content }.GetEnumerator();
    }

    IEnumerator<string?> IEnumerable<string?>.GetEnumerator()
    {
        return new[] { _content }.AsEnumerable().GetEnumerator();
    }
}
