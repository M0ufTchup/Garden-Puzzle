using System.Collections.Generic;
using GardenPuzzle.Levels;
using Godot;

namespace GardenPuzzle;

[GlobalClass]
public partial class GameConfig : Resource
{
    public IReadOnlyList<LevelData> AvailableLevels => _availableLevels;
    [Export] private Godot.Collections.Array<LevelData> _availableLevels;
}