using GardenPuzzle.Conditions;
using GardenPuzzle.Grid;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.MoneyGains;

[GlobalClass]
public partial class ConditionalMoneyGain : MoneyGain
{
    [Export] private Array<GridCondition> _conditions;
    
    public override int ApplyMoneyGained(int moneyGained, IGrid grid, Rect2I sourceGridRect)
    {
        if(_conditions is null || _conditions.Count == 0)
            return moneyGained;

        foreach (GridCondition gridCondition in _conditions)
        {
            if (!gridCondition.IsRespected(grid, sourceGridRect))
                return moneyGained;
        }

        return IncrementMoneyGained(moneyGained);
    }
}