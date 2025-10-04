using Godot;

namespace GardenPuzzle.Grid;

public interface IReadOnlyGrid
{
    int ColumnsCount { get; }
    int RowsCount { get; }

    IReadOnlyCell GetReadOnlyCell(Vector2I position);
}