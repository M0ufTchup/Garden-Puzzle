using System.Collections.Generic;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Levels;

/// <summary>
/// Represents a Level. Could have config info (flower types available, turn count, etc...)  AND info to show to the player (name, image, etc...)
/// </summary>
[GlobalClass]
[Tool] // tool for the _turnsData setter to work in editor
public partial class LevelData : Resource
{
    [Export] public string Name = "New Level";
    [Export] public PackedScene LevelScene { get; private set; }
    [Export] public Godot.Collections.Array<PlantData> AllowedPlants { get; private set; }
    [Export] public Godot.Collections.Array<LevelTurnData> TurnsData { get; private set; }
    public int TurnCount => TurnsData.Count;
}
