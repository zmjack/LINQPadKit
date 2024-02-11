using LINQPad;
using System.Collections;

namespace LINQPadKit;

public partial class KaTex : Pre, IEnumerable<string>
{
    public static void Import()
    {
        // https://katex.org/docs/browser
        // https://cdn.jsdelivr.net/npm/katex@0.16.9/dist/katex.min.css
        // https://cdn.jsdelivr.net/npm/katex@0.16.9/dist/katex.min.js
        KitUtil.Load("package", "dist", $"katex@0.16.9.min.css");
        KitUtil.Load("package", "dist", $"katex@0.16.9.min.js");

        Util.HtmlHead.AddScript("""
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
