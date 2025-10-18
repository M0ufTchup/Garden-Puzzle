using Godot;

namespace GardenPuzzle.Grid;

public interface IReadOnlyGrid
{
    ICell GetReadOnlyCell(Vector2I position);
}