using GardenPuzzle.Grid;
using Godot;

namespace GardenPuzzle.MoneyGains;

[GlobalClass]
public partial class IncrementalMoneyGain : MoneyGain
{
    [Export] private CountOf.CountOf _countOf;
    
    public override int ApplyMoneyGained(int moneyGained, IGrid grid, Rect2I sourceGridRect)
    {
        if (_countOf is null)
            return moneyGained;

        int iteration = _countOf.ComputeCount(grid, sourceGridRect);
        for (int i = 0; i < iteration; i++)
            moneyGained = IncrementMoneyGained(moneyGained);
        
        return moneyGained;
    }
}