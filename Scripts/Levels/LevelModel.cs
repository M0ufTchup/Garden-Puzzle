using System;
using GardenPuzzle.Grid;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Levels;

/// <summary>
/// Class used to store runtime data about the level. Can be used by UI to have access to UI data without direct access to managers.
/// </summary>
[GlobalClass]
public partial class LevelModel : Resource
{
    public event Action MoneyChanged;
    
    [Export] public LevelData LevelData;
    public IReadOnlyGrid Grid { get; private set; }
    [Export] public int Money { get; private set; }
    [Export] public bool Won;
    public PlantData SelectedPlantData;

    public Action<LevelData> LevelStarted;
    public Action<LevelData> LevelEnded;

    public void Reset(LevelData levelData, IReadOnlyGrid grid)
    {
        LevelData = levelData;
        Grid = grid;
        Money = LevelData.StartMoney;
        Won = false;
        SelectedPlantData = null;
    }

    public void SetMoney(int newMoney)
    {
        Money = newMoney;
        MoneyChanged?.Invoke();
    }
}