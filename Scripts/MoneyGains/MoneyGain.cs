using System;
using GardenPuzzle.Grid;
using Godot;

namespace GardenPuzzle.MoneyGains;

[GlobalClass]
public abstract partial class MoneyGain : Resource
{
    private enum GainType { Add, Multiply }
    [Export] private GainType _type;
    [Export] private float _amount = 1;
    
    public abstract int ApplyMoneyGained(int moneyGained, IGrid grid, Rect2I sourceGridRect);

    protected int IncrementMoneyGained(int startMoney)
    {
        return _type switch
        {
            GainType.Add => startMoney + (int)_amount,
            GainType.Multiply => Mathf.RoundToInt(startMoney * _amount),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}