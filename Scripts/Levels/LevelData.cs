using GardenPuzzle.Grid;
using GardenPuzzle.Plants;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Levels;

/// <summary>
/// Represents a Level. Could have config info (flower types available, turn count, etc...)  AND info to show to the player (name, image, etc...)
/// </summary>
[GlobalClass]
public partial class LevelData : Resource
{
    [Export] public string Name = "New Level";
    [Export] public int StartMoney = 50;
    [Export] public PackedScene LevelScene { get; private set; }
    [Export] public Array<PlantData> AllowedPlants { get; private set; }
    [Export] public LevelGridConfig GridConfig { get; private set; }
}
