using Godot;

namespace GardenPuzzle.Grid;

public interface IReadOnlyGrid
{
    IReadOnlyCell GetReadOnlyCell(Vector2I position);
}