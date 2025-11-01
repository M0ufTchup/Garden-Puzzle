using Godot;

namespace GardenPuzzle.Grid;

public interface IGridPartition
{
    Vector2I PartitionPosition { get; }
    Rect2I GridRect { get; } 
    int Cost { get; }
    bool Locked { get; }
}

public record GridPartition(Vector2I PartitionPosition, Rect2I GridRect, int Cost) : IGridPartition
{
    public bool Locked { get; set; } = true;
}
