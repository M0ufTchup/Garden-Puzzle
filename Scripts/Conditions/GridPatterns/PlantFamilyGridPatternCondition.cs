using GardenPuzzle.Grid;
using GardenPuzzle.Plants;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Conditions;

[GlobalClass]
public partial class PlantFamilyGridPatternCondition : ResourceGridPatternCondition<PlantFamily>
{
    protected override Array<Array<PlantFamily>> ResourceArray2D => _resourcePlantFamilyArray2D;
    
    [Export] private Array<Array<PlantFamily>> _resourcePlantFamilyArray2D;

    protected override bool IsRespected(IGrid grid, Vector2I gridPosition, PlantFamily expectedResource)
    {
        if (expectedResource is null)
            return true;
        
        ICell cell = grid.GetCell(gridPosition);
        if (cell is null)
            return true;

        return expectedResource == cell.Plant?.Data.Family;
    }
}