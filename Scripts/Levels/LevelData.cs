using System.Collections.Generic;
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
    public IReadOnlyList<LevelTurnData> TurnsData => _turnsData;
    public int TurnCount => TurnsData.Count;

    [Export] private Godot.Collections.Array<LevelTurnData> _turnsData
    {
        get => _internalTurnsData;
        set
        {
            _internalTurnsData = value;
            if (Engine.IsEditorHint() && _internalTurnsData is not null && _internalTurnsData.Count > 0)
            {
                for (var i = 0; i < _internalTurnsData.Count; i++)
                {
                    if (_internalTurnsData[i] is null)
                        _internalTurnsData[i] = new LevelTurnData();
                }
            }
        }
    }
    private Godot.Collections.Array<LevelTurnData> _internalTurnsData;
}
