using GardenPuzzle.Grid;
using GardenPuzzle.Ground;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Conditions;

[GlobalClass]
public partial class GroundTypeGridPatternCondition : ResourceGridPatternCondition<GroundType>
{
    protected override Array<Array<GroundType>> ResourceArray2D => _expectedResourceGroundTypeArray2D;
    [Export] private Array<Array<GroundType>> _expectedResourceGroundTypeArray2D;
    
    protected override bool IsRespected(IGrid grid, Vector2I gridPosition, GroundType expectedResource)
    {
        if (expectedResource is null)
            return true;

        ICell cell = grid.GetCell(gridPosition);
        if (cell is null)
            return true;

        return cell.GroundType == expectedResource;
    }
}