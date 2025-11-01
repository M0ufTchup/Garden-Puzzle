using System.Collections.Generic;
using Godot;

namespace GardenPuzzle.Grid;

public interface IReadOnlyGrid
{
    ICell GetReadOnlyCell(Vector2I position);
    
    IGridPartition GetReadOnlyGridPartition(Vector2I gridPartitionPosition);
    IEnumerable<IGridPartition> ReadOnlyGridPartitions { get; }
}