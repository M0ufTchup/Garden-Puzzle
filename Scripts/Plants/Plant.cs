using GardenPuzzle.Grid;
using GardenPuzzle.MoneyGains;
using Godot;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class Plant : Node3D
{
    public PlantData Data { get; private set; }
    public Rect2I GridRect { get; private set; }
    public int GainedMoney { get; private set; }

    [Export] private MeshInstance3D _meshInstance;
    
    public void SetData(PlantData data)
    {
        Data = data;
        _meshInstance.Mesh = Data.Mesh;
    }

    public void SetGridRect(Rect2I gridRect)
    {
        GridRect = gridRect;
    }

    public int UpdateGainedMoney(IGrid grid)
    {
        GainedMoney = Data.DefaultMoneyGain;
        if(Data.AdditionalMoneyGains is null || Data.AdditionalMoneyGains.Count == 0) 
            return GainedMoney;
        
        foreach (MoneyGain additionalMoneyGain in Data.AdditionalMoneyGains)
            GainedMoney = additionalMoneyGain.ApplyMoneyGained(GainedMoney, grid, GridRect);

        return GainedMoney;
    }

    public void ClearGainedMoney()
    {
        GainedMoney = 0;
    }
}