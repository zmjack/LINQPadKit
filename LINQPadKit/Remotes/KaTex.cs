using LINQPad;
using System.Collections;

namespace LINQPadKit.Remotes;

public partial class KaTex : Pre, IEnumerable<string>
{
    public static void Import()
    {
        Util.HtmlHead.AddCssLink("https://cdn.jsdelivr.net/npm/katex@0.16.10/dist/katex.min.css");
        Util.HtmlHead.AddScriptFromUri("https://cdn.jsdelivr.net/npm/katex@0.16.10/dist/katex.min.js");

        Util.HtmlHead.AddScript(
"""
window.call_katex = function(id) {
    var el = document.getElementById(id);
    katex.render(el.textContent, el, {
        throwOnError: false
    });
}
"""
        );
    }

    public KaTex()
    {
        CssClass = "katex";
    }

    private string _content;
    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            HtmlElement.InnerHtml = value;
            HtmlElement.InvokeScript(false, "call_katex", HtmlElement.ID);
        }
    }

    public virtual void Add(string content)
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
