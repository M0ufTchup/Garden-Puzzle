using Godot;

namespace GardenPuzzle.Grid;

public class DummyGrid : IGrid
{
    public int ColumnsCount => _cells.GetLength(1);
    public int RowsCount => _cells.GetLength(0);

    private readonly ICell[,] _cells;
    
    public DummyGrid(int columns, int rows)
    {
        _cells = new ICell[rows, columns];
        for (var i = 0; i < _cells.Length; i++)
        {
            int x = i / rows;
            int y = i % rows;
            _cells[x, y] = new DummyCell(new Vector2I(x, y));
        }
    }
    
    public IReadOnlyCell GetReadOnlyCell(Vector2I position) => GetCell(position);
    public ICell GetCell(Vector2I position)
    {
        if (position.X < 0 || RowsCount <= position.X)
            return null;
        if (position.Y < 0 || ColumnsCount <= position.Y)
            return null;
        
        return _cells[position.X, position.Y];
    }
}