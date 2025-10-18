using Godot;

namespace GardenPuzzle.Grid;

public interface IGrid : IReadOnlyGrid
{
    ICell GetCell(Vector2I position);
    ICell GetCell(Vector3 worldPosition);

    Vector3 GetCellWorldPosition(ICell cell);
}