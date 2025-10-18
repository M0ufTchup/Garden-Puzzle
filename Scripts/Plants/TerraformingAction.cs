using GardenPuzzle.Grid;
using GardenPuzzle.Ground;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class TerraformingAction : Resource
{
    [Export] private Array<Array<GroundType>> _resourceGroundTypeArray2D;

    public void Apply(IGrid grid, ICell centerCell)
    {
        for (var i = 0; i < _resourceGroundTypeArray2D.Count; i++)
        {
            for (var j = 0; j < _resourceGroundTypeArray2D[i].Count; j++)
            {
                GroundType wantedGroundType = _resourceGroundTypeArray2D[i][j];
                if (wantedGroundType is null)
                    continue;
                
                ICell cell = grid.GetCell(centerCell.Position + new Vector2I(i, j) - new Vector2I(_resourceGroundTypeArray2D.Count / 2, _resourceGroundTypeArray2D[i].Count / 2));
                if(cell is not null) grid.SetCellGroundType(cell, wantedGroundType);
            }
        }
    }
}