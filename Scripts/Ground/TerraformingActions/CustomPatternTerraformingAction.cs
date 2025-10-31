using GardenPuzzle.Grid;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Ground;

[GlobalClass]
public partial class CustomPatternTerraformingAction : TerraformingAction
{
    [Export] private Array<Array<GroundType>> _patternResourceGroundTypeArray2D;
    
    public override void Apply(IGrid grid, Rect2I gridRectSource)
    {
        if (gridRectSource.Size.X + 2 > _patternResourceGroundTypeArray2D.Count || gridRectSource.Size.Y + 2 > _patternResourceGroundTypeArray2D[0]?.Count)
        {
            GardenLogger.LogError(this, $"given gridRectSource is too large for the configured action (given grid rect: {gridRectSource}, configured {{xSize={_patternResourceGroundTypeArray2D.Count}, ySize={_patternResourceGroundTypeArray2D[0]?.Count}}})");
            return;
        }
        
        Rect2I customRect = Utilities.GetCustomRect(gridRectSource, _patternResourceGroundTypeArray2D);
        for (int i = 0; i < customRect.Size.X; i++)
        {
            for (int j = 0; j < customRect.Size.Y; j++)
            {
                GroundType wantedGroundType = _patternResourceGroundTypeArray2D[i][j];
                if (wantedGroundType is null)
                    continue;

                grid.SetCellGroundType(customRect.Position + new Vector2I(i, j), wantedGroundType);
            }
        }
    }
}