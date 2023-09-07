using LINQPad.Controls;
using LINQPad.Controls.Core;
using LINQPadKit.Design;
using System.Text.Json;

namespace LINQPadKit
{
    public partial class TreeGraph : Div, IKit
    {
        public string Instance { get; set; }

        public TreeGraph()
        {
            (this as IKit).InitailizeAsync();
        }

        public void InitailizeInstance()
        {
            var instance = $"kit_{nameof(TreeGraph)}_{HtmlElement.ID}";
            HtmlElement.InvokeScript(false, "eval", $"var {Instance} = new TreeGraph();");
            Instance = instance;
        }

        public void Render()
        {
            (this as IKit).WaitForReady();

            HtmlElement.InvokeScript(false, "eval", $"{Instance}.render(document.getElementById('{HtmlElement.ID}'));");
        }

        public void Refresh(ITreeGraphNode? node = null)
        {
            (this as IKit).WaitForReady();

            var js_node = node != null
                ? JsonSerializer.Serialize(node, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                : "undefined";
            HtmlElement.InvokeScript(false, "eval", $"{Instance}.refresh({js_node});");
        }

    }
}
