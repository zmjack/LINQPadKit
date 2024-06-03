using LINQPad.Controls;
using LINQPad.Controls.Core;

namespace LINQPadKit;

public partial class TileMap : Div
{
    public static void Import()
    {
        KitUtil.Load("package", "dist", $"fabric@5.3.1.min.js");
        KitUtil.Load("TileMap", "dist", $"tile-map.css");
        KitUtil.Load("TileMap", "dist", $"tile-map.js");
    }

    public enum Shape
    {
        None = 0,
        Rectangle = 1,
        Hexagon = 2,
    }

    public string Instance { get; set; }

    private readonly Div _canvas;
    private readonly Div _bridge;
    private readonly string _shape;
    private readonly int _cellWidth;
    private readonly int _rows;
    private readonly int _cols;

    public TileMap(Shape shape, int cellWidth, int rows, int cols)
    {
        _shape = shape switch
        {
            Shape.Rectangle => "rectangle",
            Shape.Hexagon => "hexagon",
            _ => "",
        };
        _cellWidth = cellWidth;
        _rows = rows;
        _cols = cols;

        _canvas = new Div();
        Children.Add(_canvas);

        _bridge = new Div();
        _bridge.HtmlElement.AddEventListener("cell:mouseDown",
        [
            "x: args.detail.x",
            "y: args.detail.y",
        ], CellMouseDown);
        _bridge.HtmlElement.AddEventListener("cell:colorChange",
        [
            "x: args.detail.x",
            "y: args.detail.y",
            "color: args.detail.color",
        ], ColorChange);
        _bridge.HtmlElement.AddEventListener("cell:imageChange",
        [
            "x: args.detail.x",
            "y: args.detail.y",
            "image: args.detail.image",
        ], ImageChange);
        _bridge.HtmlElement.AddEventListener("ready",
        [
            "rows: args.detail.rows",
            "cols: args.detail.cols",
        ], Ready);
        Children.Add(_bridge);
    }

    public Cell this[int x, int y] => new(this, x, y);

    protected override void OnRendering(EventArgs e)
    {
        var instance = $"kit_{nameof(TileMap)}_{HtmlElement.ID}";
        Instance = instance;

        HtmlElement.InvokeScript(false, "eval", $"var {instance} = new {nameof(TileMap)}('{_shape}', {_cellWidth}, {_rows}, {_cols});");
        HtmlElement.InvokeScript(false, "eval", $"{Instance}.render('{_canvas.HtmlElement.ID}', '{_bridge.HtmlElement.ID}');");
    }

    public void SetColor(int x, int y, string color)
    {
        HtmlElement.InvokeScript(false, "eval", $@"{Instance}.setColor({x}, {y}, '{color}')");
    }

    public string? GetColor(int x, int y)
    {
        return HtmlElement.InvokeScript(true, "eval", $@"{Instance}.getColor({x}, {y})") as string;
    }

    public void SetImage(int x, int y, string image)
    {
        HtmlElement.InvokeScript(false, "eval", $@"{Instance}.setImage({x}, {y}, '{image}')");
    }

    public string? GetImage(int x, int y)
    {
        return HtmlElement.InvokeScript(true, "eval", $@"{Instance}.getImage({x}, {y})") as string;
    }

    public void ResetCell(int x, int y)
    {
        HtmlElement.InvokeScript(false, "eval", $@"{Instance}.resetCell({x}, {y})");
    }

    public struct CellMouseDownEventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public delegate void CellMouseDownEvent(object sender, CellMouseDownEventArgs args);
    public event CellMouseDownEvent OnCellMouseDown;
    private void CellMouseDown(object? sender, PropertyEventArgs args)
    {
        OnCellMouseDown?.Invoke(this, new CellMouseDownEventArgs
        {
            X = int.Parse(args.Properties["x"]),
            Y = int.Parse(args.Properties["y"]),
        });
    }

    public struct CellColorChangeEventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; }
    }
    public delegate void CellColorChangeEvent(object sender, CellColorChangeEventArgs args);
    public event CellColorChangeEvent OnCellColorChange;
    private void ColorChange(object? sender, PropertyEventArgs args)
    {
        OnCellColorChange?.Invoke(this, new CellColorChangeEventArgs
        {
            X = int.Parse(args.Properties["x"]),
            Y = int.Parse(args.Properties["y"]),
            Color = args.Properties["color"],
        });
    }

    public struct CellImageChangeEventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Image { get; set; }
    }
    public delegate void CellImageChangeEvent(object sender, CellImageChangeEventArgs args);
    public event CellImageChangeEvent OnCellImageChange;
    private void ImageChange(object? sender, PropertyEventArgs args)
    {
        OnCellImageChange?.Invoke(this, new CellImageChangeEventArgs
        {
            X = int.Parse(args.Properties["x"]),
            Y = int.Parse(args.Properties["y"]),
            Image = args.Properties["image"],
        });
    }

    public struct ReadyEventArgs
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
    }
    public delegate void ReadyEvent(object sender, ReadyEventArgs args);
    public event ReadyEvent OnReady;
    private void Ready(object? sender, PropertyEventArgs args)
    {
        OnReady?.Invoke(this, new ReadyEventArgs
        {
            Rows = int.Parse(args.Properties["rows"]),
            Cols = int.Parse(args.Properties["cols"]),
        });
    }
}
