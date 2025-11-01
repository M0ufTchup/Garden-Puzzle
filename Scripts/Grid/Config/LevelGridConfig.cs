using Godot;
using Godot.Collections;

namespace GardenPuzzle.Grid;

[GlobalClass]
public partial class LevelGridConfig : Resource
{
    [Export] public Array<Array<int>> PartitionsCostIntArray2D { get; private set; }
    [Export] public Vector2I UnlockedPartitionPosition { get; private set; } = Vector2I.Zero;

    public int GetPartitionGridXSize() => PartitionsCostIntArray2D.Count;
    public int GetPartitionGridYSize() => PartitionsCostIntArray2D[0].Count;
}