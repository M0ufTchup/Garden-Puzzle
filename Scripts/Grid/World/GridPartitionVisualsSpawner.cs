using GardenPuzzle.Levels;
using Godot;

namespace GardenPuzzle.Grid;

[GlobalClass]
public partial class GridPartitionVisualsSpawner : Node3D
{
    [Export] private LevelModel _levelModel;
    [Export] private PackedScene _gridPartitionVisualScene;

    public override void _EnterTree()
    {
        base._EnterTree();
        _levelModel.LevelStarted += LevelStarted;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _levelModel.LevelStarted -= LevelStarted;
    }

    private void LevelStarted(LevelData levelData)
    {
        foreach (IGridPartition gridPartition in _levelModel.Grid.ReadOnlyGridPartitions)
        {
            GridPartitionVisual gridPartitionVisual = _gridPartitionVisualScene.Instantiate<GridPartitionVisual>();
            AddChild(gridPartitionVisual);
            gridPartitionVisual.Init(_levelModel, gridPartition);
        }
    }
}