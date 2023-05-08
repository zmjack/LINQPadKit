using LINQPad.Controls;
using LINQPad.Controls.Core;
using LINQPadKit.Design;
using System.Text.Json;

namespace LINQPadKit
{
    public partial class TreeGraph : Div, IKit
    {
        public string Export => $"{HtmlElement.ID}_treeGraph";
        public bool Ready { get; set; }

        public TreeGraph()
        {
            HtmlElement.InvokeScript(false, "eval", $"var {Export} = new TreeGraph();");
            (this as IKit).StartRenderTask(HtmlElement);
        }

        public void Refresh(ITreeGraphNode? node = null)
        {
            (this as IKit).WaitReady();

            var js_node = node != null
                ? JsonSerializer.Serialize(node, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                : "undefined";
            HtmlElement.InvokeScript(false, "eval", $"{Export}.refresh({js_node});");
        }

    }
}
