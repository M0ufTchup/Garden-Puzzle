using System;
using Godot;

namespace GardenPuzzle.Levels;

/// <summary>
/// Class used to store runtime data about the level. Can be used by UI to have access to UI data without direct access to managers.
/// </summary>
[GlobalClass]
public partial class LevelModel : Resource
{
    [Export] public LevelData LevelData;
    [Export] public int Money;
    [Export] public int CurrentTurnIndex;
    [Export] public bool Won;

    public Action RequestTurnEnd;

    public Action<LevelData> LevelStarted;
    public Action<LevelData> LevelEnded;

    public void Reset(LevelData levelData)
    {
        LevelData = levelData;
        Money = 0;
        CurrentTurnIndex = 0;
        Won = false;
    }
}