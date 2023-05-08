using LINQPad;
using LINQPad.Controls;
using LINQPad.Controls.Core;
using LINQPadKit.Design;
using NStandard;
using System.Text.Json;

namespace LINQPadKit
{
    public partial class TileMap : Div, IKit
    {
        private readonly Div _spritesContainer;
        public string Export => $"{HtmlElement.ID}_tileMap";
        public bool Ready { get; set; }

        public TileMap(int tileSize, int tileWidth, int tileHeight)
        {
            _spritesContainer = new Div
            {
                CssClass = "tile-map-sprites",
            };
            Children.Add(_spritesContainer);

            for (int i = 0; i < tileWidth * tileHeight; i++)
            {
                var sprite = new Div
                {
                    CssClass = "tile-map-sprite"
                };
                _spritesContainer.Children.Add(sprite);
            }

            HtmlElement.InvokeScript(false, "eval", $"var {Export} = new TileMap({tileSize}, {tileWidth}, {tileHeight});");

            (this as IKit).StartRenderTask(HtmlElement);
        }

        public void Refresh(int tileSize, int tileWidth, int tileHeight)
        {
            (this as IKit).WaitReady();

            foreach (var sprite in _spritesContainer.Children)
            {
                HtmlElement.InvokeScript(false, "eval", $"{Export}.addSprite(document.getElementById('{sprite.HtmlElement.ID}', true));");
            }
        }

        public void UseMap(Dictionary<int, string> tileHtmlMap)
        {
            (this as IKit).WaitReady();

            var js_tileHtmlMap = $@"{{{tileHtmlMap.Select(x => $"{x.Key}: '{x.Value}'").Join($",")}}}";
            HtmlElement.InvokeScript(false, "eval", $"{Export}.useMap({js_tileHtmlMap});");
        }

        public void LoadMap(int[][] map)
        {
            (this as IKit).WaitReady();

            var js_map = JsonSerializer.Serialize(map, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            HtmlElement.InvokeScript(false, "eval", $"{Export}.loadMap({js_map});");
        }
    }
}
