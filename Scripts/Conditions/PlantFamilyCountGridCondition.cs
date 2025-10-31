using System.Collections.Generic;
using GardenPuzzle.Grid;
using GardenPuzzle.Plants;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Conditions;

[GlobalClass]
public partial class PlantFamilyCountGridCondition : GridCondition
{
    [Export] private int _range = 1;
    [Export] private int _expectedValidPlantCount = 1;
    [Export] private Array<PlantFamily> _expectedPlantFamilies;

    public override bool IsRespected(IGrid grid, Rect2I sourceGridRect)
    {
        if (_expectedPlantFamilies is null || _expectedPlantFamilies.Count == 0)
            return true;
        
        Rect2I expandedGridRect = Utilities.ExpandRect(sourceGridRect, _range);
        IReadOnlyDictionary<Resource, int> plantCount = grid.GetPlantCount(expandedGridRect, sourceGridRect);

        int totalCount = 0;
        foreach (PlantFamily expectedPlantFamily in _expectedPlantFamilies)
            if (plantCount.TryGetValue(expectedPlantFamily, out int count))
                totalCount += count;
        
        return totalCount >= _expectedValidPlantCount;
    }
}