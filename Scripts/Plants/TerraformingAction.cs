using GardenPuzzle.Grid;
using GardenPuzzle.Ground;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class TerraformingAction : Resource
{
    [Export] private Array<Array<GroundType>> _resourceGroundTypeArray2D;

    public void Apply(IGrid grid, Vector2I gridPositionSource) => Apply(grid, new Rect2I(gridPositionSource, Vector2I.One));
    public void Apply(IGrid grid, Rect2I gridRectSource)
    {
        if (gridRectSource.Size.X + 2 > _resourceGroundTypeArray2D.Count || gridRectSource.Size.Y + 2 > _resourceGroundTypeArray2D[0]?.Count)
        {
            GD.PrintErr($"[TERRAFORMING ACTION]: given gridRectSource is too large for the configured action (given grid rect: {gridRectSource}, configured {{xSize={_resourceGroundTypeArray2D.Count}, ySize={_resourceGroundTypeArray2D[0]?.Count}}})");
            return;
        }

        Vector2I array2DSize = new Vector2I(_resourceGroundTypeArray2D.Count, _resourceGroundTypeArray2D[0].Count);
        Rect2I targetGridRect = new Rect2I(gridRectSource.Position - ((array2DSize - gridRectSource.Size) / 2), array2DSize);
        
        for (int i = 0; i < targetGridRect.Size.X; i++)
        {
            for (int j = 0; j < targetGridRect.Size.Y; j++)
            {
                GroundType wantedGroundType = _resourceGroundTypeArray2D[i][j];
                if (wantedGroundType is null)
                    continue;
                
                Vector2I gridPosition = targetGridRect.Position + new Vector2I(i, j);
                ICell cell = grid.GetCell(gridPosition);
                if(cell is not null) grid.SetCellGroundType(cell, wantedGroundType);
            }
        }
    }
}