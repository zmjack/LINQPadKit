using LINQPad.Controls;
using LINQPad.Controls.Core;
using LINQPadKit.Design;

namespace LINQPadKit;

[Obsolete("Designing.")]
public partial class TileMap : Div, IKit
{
    public enum Shape
    {
        None,
        Rectangle,
        Hexagon,
    }

    public string Instance { get; set; }

    private readonly Div _canvas;
    private readonly Div _bridge;
    private readonly string _shape;
    private readonly int _tileSize;
    private readonly int _tileWidth;
    private readonly int _tileHeight;

    public class CellMouseDownEventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public delegate void CellMouseDownEvent(object sender, CellMouseDownEventArgs eventArgs);
    public event CellMouseDownEvent OnCellMouseDown;

    public TileMap(Shape shape, int tileSize, int tileWidth, int tileHeight)
    {
        _shape = shape switch
        {
            Shape.Rectangle => "rectangle",
            Shape.Hexagon => "hexagon",
            _ => "",
        };
        _tileSize = tileSize;
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;

        _canvas = new Div();
        Children.Add(_canvas);

        _bridge = new Div();
        _bridge.HtmlElement.AddEventListener("cell:mousedown", new[]
        {
            "x: args.detail.x",
            "y: args.detail.y",
        }, (sender, args) =>
        {
            var x = int.Parse(args.Properties["x"]);
            var y = int.Parse(args.Properties["y"]);

            OnCellMouseDown?.Invoke(this, new CellMouseDownEventArgs
            {
                X = x,
                Y = y,
            });
        });
        Children.Add(_bridge);

        (this as IKit).InitailizeAsync();
    }

    public void InitailizeInstance()
    {
        var instance = $"kit_{nameof(TileMap)}_{HtmlElement.ID}";
        HtmlElement.InvokeScript(false, "eval", $"var {instance} = new {nameof(TileMap)}('{_shape}', {_tileSize}, {_tileWidth}, {_tileHeight});");
        Instance = instance;
    }

    public void Render()
    {
        (this as IKit).WaitForReady();

        HtmlElement.InvokeScript(false, "eval", $"{Instance}.render('{_canvas.HtmlElement.ID}', {{ bridge: '{_bridge.HtmlElement.ID}' }});");
    }

    public string? GetColor(int x, int y)
    {
        return HtmlElement.InvokeScript(true, "eval", $@"{Instance}.getColor({x}, {y})") as string;
    }

    public void SetColor(int x, int y, string color)
    {
        HtmlElement.InvokeScript(false, "eval", $@"{Instance}.setColor({x}, {y}, '{color}')");
    }

    public void ResetColor(int x, int y)
    {
        HtmlElement.InvokeScript(false, "eval", $@"{Instance}.resetColor({x}, {y})");
    }
}
