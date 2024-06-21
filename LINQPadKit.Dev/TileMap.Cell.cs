namespace LINQPadKit;

public partial class TileMap
{
    public class Cell
    {
        private TileMap _map;
        public int X { get; }
        public int Y { get; }

        public Cell(TileMap map, int x, int y)
        {
            _map = map;
            X = x;
            Y = y;
        }

        public string? Image
        {
            get => _map.GetImage(X, Y);
            set => _map.SetImage(X, Y, value);
        }

        public string? Color
        {
            get => _map.GetColor(X, Y);
            set => _map.SetColor(X, Y, value);
        }

        public void Reset()
        {
            _map.ResetCell(X, Y);
        }
    }
}
