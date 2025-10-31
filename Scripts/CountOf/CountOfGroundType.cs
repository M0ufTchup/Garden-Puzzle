using System.Collections.Generic;
using GardenPuzzle.Grid;
using GardenPuzzle.Ground;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.CountOf;

[GlobalClass]
public partial class CountOfGroundType : CountOf
{
    [Export] private int _range = 1;
    [Export] private Array<GroundType> _expectedGroundTypes;
    
    public override int ComputeCount(IGrid grid, Rect2I gridRect)
    {
        if (_expectedGroundTypes is null || _expectedGroundTypes.Count == 0)
            return 0;
        
        Rect2I expandedRect = Utilities.ExpandRect(gridRect, _range);
        IReadOnlyDictionary<Resource, int> plantCounts = grid.GetGroundTypeCount(expandedRect);

        int totalCount = 0;
        foreach (GroundType expectedGroundType in _expectedGroundTypes)
            if (plantCounts.TryGetValue(expectedGroundType, out var count))
                totalCount += count;
        
        return totalCount;
    }
}