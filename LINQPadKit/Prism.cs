using LINQPad;
using System.Collections;

namespace LINQPadKit;

public class Prism : Pre, IEnumerable<string>
{
    public static void Import()
    {
        // https://cdnjs.com/libraries/prism
        KitUtil.Load("package", "dist", $"prism@1.29.0.min.css");
        KitUtil.Load("package", "dist", $"prism@1.29.0.min.js");

        Util.HtmlHead.AddStyles("""
.prism {
  white-space: pre-wrap;
}
"""
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
            var html = Util.InvokeScript(true, "call_prism", new[] { Language, value }) as string;
            HtmlElement.InnerHtml = html;
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
