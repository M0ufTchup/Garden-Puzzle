using System.Collections.Generic;
using GardenPuzzle.Grid;
using GardenPuzzle.Plants;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.CountOf;

[GlobalClass]
public partial class CountOfPlantFamily : CountOf
{
    [Export] private int _range = 1;
    [Export] private Array<PlantFamily> _expectedPlantFamilies;
    
    public override int ComputeCount(IGrid grid, Rect2I gridRect)
    {
        if (_expectedPlantFamilies is null || _expectedPlantFamilies.Count == 0)
            return 0;
        
        Rect2I expandedRect = Utilities.ExpandRect(gridRect, _range);
        IReadOnlyDictionary<Resource, int> plantCounts = grid.GetPlantCount(expandedRect, gridRect);

        int totalCount = 0;
        foreach (PlantFamily expectedPlantFamily in _expectedPlantFamilies)
            if (plantCounts.TryGetValue(expectedPlantFamily, out var count))
                totalCount += count;
        
        return totalCount;
    }
}