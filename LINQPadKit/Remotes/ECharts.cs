using LINQPad;
using LINQPad.Controls;
using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace LINQPadKit.Remotes;

public partial class ECharts : Div, IEnumerable<string?>
{
    public static void Import()
    {
        Util.HtmlHead.AddScriptFromUri("https://cdn.jsdelivr.net/npm/echarts@5.5.0/dist/echarts.min.js");
        Util.HtmlHead.AddScript(
"""
window.call_echarts = function(id, option) {
    var chart = echarts.init(document.getElementById(id));
    chart.setOption(JSON.parse(option.replace(/\n/g,'\\n').replace(/\r/g,'\\r')));
}
"""
        );
    }

    public ECharts() : this("100%", "400px")
    {
    }

    public ECharts(int width, int height)
    {
        HtmlElement.SetAttribute("style", $"width:{width}px;height:{height}px");
    }

    public ECharts(string width, string height)
    {
        HtmlElement.SetAttribute("style", $"width:{width};height:{height}");
    }

    private string? _option;
    public string? Option
    {
        get => _option;
        set
        {
            _option = value;
            HtmlElement.InvokeScript(false, "call_echarts", HtmlElement.ID, value);
        }
    }

    public virtual void Add(string? option)
    {
        Option = option;
    }

    public virtual void Add(object? chart)
    {
        if (chart is null) Option = null;
        else
        {
            Option = $"""
{JsonSerializer.Serialize(chart, new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            })}
""";
        }
    }

    public IEnumerator GetEnumerator()
    {
        return new[] { _option }.GetEnumerator();
    }

    IEnumerator<string?> IEnumerable<string?>.GetEnumerator()
    {
        return new[] { _option }.AsEnumerable().GetEnumerator();
    }

}
