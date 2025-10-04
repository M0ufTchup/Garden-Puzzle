using Godot;

namespace GardenPuzzle.Grid;

public interface IGrid : IReadOnlyGrid
{
    ICell GetCell(Vector2I position);
}