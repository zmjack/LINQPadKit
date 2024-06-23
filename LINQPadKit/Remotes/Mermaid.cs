using LINQPad;
using System.Collections;

namespace LINQPadKit.Remotes;

public partial class Mermaid : Pre, IEnumerable<string?>
{
    public static void Import()
    {
        Util.HtmlHead.AddScriptFromUri("https://cdn.jsdelivr.net/npm/mermaid@10.9.1/dist/mermaid.min.js");

        Util.HtmlHead.AddScript(
"""
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

    private string? _content;
    public string? Content
    {
        get => _content;
        set
        {
            _content = value;
            HtmlElement.InnerHtml = value;
            HtmlElement.InvokeScript(false, "call_mermaid", HtmlElement.ID);
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
