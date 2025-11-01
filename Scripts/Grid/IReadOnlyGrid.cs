using System.Collections.Generic;
using Godot;

namespace GardenPuzzle.Grid;

public interface IReadOnlyGrid
{
    ICell GetCell(Vector2I position);
    Vector3 GetCellWorldPosition(ICell cell);
    
    IEnumerable<IGridPartition> ReadOnlyGridPartitions { get; }
    IGridPartition GetReadOnlyGridPartition(Vector2I gridPartitionPosition);
    Rect2 GetGridPartitionWorldRect(IGridPartition gridPartition);
}