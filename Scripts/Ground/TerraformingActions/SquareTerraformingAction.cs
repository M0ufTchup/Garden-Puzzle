using GardenPuzzle.Grid;
using Godot;

namespace GardenPuzzle.Ground;

[GlobalClass]
public partial class SquareTerraformingAction : TerraformingAction
{
    [Export] private int _squareOffset = 1;
    [Export] private GroundType _wantedGroundType;    

    public override void Apply(IGrid grid, Rect2I gridRectSource)
    {
        if (_wantedGroundType is null)
        {
            GardenLogger.LogError(this, "[SquareTerraformingAction] wanted ground type is null. Can't apply terraforming");
            return;
        }

        if (_squareOffset <= 0)
        {
            GardenLogger.LogError(this, "[SquareTerraformingAction] square offset is negative or zero. Can't apply terraforming.");
            return;
        }
        
        Rect2I targetRect = new Rect2I(gridRectSource.Position - Vector2I.One * _squareOffset, gridRectSource.Size + Vector2I.One * _squareOffset * 2);
        grid.SetCellsGroundType(targetRect, _wantedGroundType);
    }
}