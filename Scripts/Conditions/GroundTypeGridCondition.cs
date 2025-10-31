using GardenPuzzle.Grid;
using GardenPuzzle.Ground;
using Godot;

namespace GardenPuzzle.Conditions;

[GlobalClass]
public partial class GroundTypeGridCondition : GridCondition
{
    [Export] private GroundType _expectedGroundType;
    
    public override bool IsRespected(IGrid grid, Rect2I sourceGridRect)
    {
        return grid.All(sourceGridRect, (_, cell) => cell is null || cell.GroundType == _expectedGroundType);
    }
}