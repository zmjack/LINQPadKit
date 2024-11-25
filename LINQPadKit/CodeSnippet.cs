using LINQPad;
using LINQPad.Controls;
using System.Collections;
using System.Text;

namespace LINQPadKit;

public partial class CodeSnippet : Div, IEnumerable<string?>
{
    public SyntaxLanguageStyle Language { get; }
    public bool AutoFormat { get; }

    public CodeSnippet(SyntaxLanguageStyle language, bool autoFormat = false)
    {
        Language = language;
        AutoFormat = autoFormat;
    }

    public void Refresh()
    {
        var text = new SyntaxColoredText(Content, Language);
        HtmlElement.InnerHtml = AutoFormat
            ? text.AutoFormat().Html
            : text.Html;
    }

    private readonly StringBuilder _content = new();
    public string? Content
    {
        get => _content.ToString();
        set
        {
            _content.Clear();
            _content.AppendLine(value);
            Refresh();
        }
    }

    public virtual void Add(string? content)
    {
        _content.AppendLine(content);
        Refresh();
    }

    public IEnumerator GetEnumerator()
    {
        return new[] { Content }.GetEnumerator();
    }

    IEnumerator<string?> IEnumerable<string?>.GetEnumerator()
    {
        return new[] { Content }.AsEnumerable().GetEnumerator();
    }

}
