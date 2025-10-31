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
        int newMoneyGained = moneyGained;
        for (int i = 0; i < iteration; i++)
            newMoneyGained = IncrementMoneyGained(newMoneyGained);
        
        GardenLogger.Log(this, $"Result: {newMoneyGained}\t(offset: {newMoneyGained - moneyGained})");
        return newMoneyGained;
    }
}