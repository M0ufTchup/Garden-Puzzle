using GardenPuzzle.Grid;
using GardenPuzzle.Plants;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Conditions;

[GlobalClass]
public partial class PlantFamilyCondition : GridCondition
{
    [Export] private Array<Array<PlantFamily>> _resourcePlantFamilyArray2D;
    
    public override bool IsRespected(IGrid grid, ICell cellToCheck)
    {
        for (var i = 0; i < _resourcePlantFamilyArray2D.Count; i++)
        {
            for (var j = 0; j < _resourcePlantFamilyArray2D[i].Count; j++)
            {
                PlantFamily expectedPlantFamily = _resourcePlantFamilyArray2D[i][j];
                if (expectedPlantFamily is null)
                    continue;
                
                ICell cell = grid.GetCell(cellToCheck.Position + new Vector2I(i, j) - new Vector2I(_resourcePlantFamilyArray2D.Count / 2, _resourcePlantFamilyArray2D[i].Count / 2));
                if (cell is null || cell == cellToCheck)
                    continue;

                if (expectedPlantFamily != cell.Plant.Data.Family)
                    return false;
            }
        }

        return true;
    }
}