using System;
using Godot;

namespace GardenPuzzle.Levels;

/// <summary>
/// Level/Game Manager. Handles the logic of a single level/game from start to finish.
/// </summary>
[GlobalClass]
public partial class LevelManager : Node3D
{
    public Action<LevelData> LevelStarted;
    public Action<LevelData> LevelEnded;
    
    [Export] private LevelModel _levelModel;
    [Export] private Node3D _levelParent;
    [ExportGroup("Debug")]
    [Export] private LevelData _debugLevelData; // if not null, will launch this LevelData automatically
    private LevelData LevelData => _levelModel.LevelData;
    
    public void Init(LevelData levelData)
    {
        if (levelData is null)
        {
            GD.PrintErr("LevelData is null");
            return;
        }
        _levelModel.Reset(levelData);

        Node3D levelInstance = LevelData.LevelScene.Instantiate<Node3D>();
        _levelParent.AddChild(levelInstance);
        levelInstance.Position = Vector3.Zero;
        
        LevelStarted?.Invoke(LevelData);
        _levelModel.LevelStarted?.Invoke(LevelData);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        
        if (_levelModel is null)
        {
            GD.PrintErr("LevelModel is null");
            return;
        }
        _levelModel.RequestTurnEnd += EndTurn;

#if TOOLS // TOOLS is defined when building with the Debug configuration (editor and editor player) (https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_features.html#preprocessor-defines)
        if (_debugLevelData is not null)
        {
            Init(_debugLevelData);
        }
#endif
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _levelModel.RequestTurnEnd -= EndTurn;
    }

    private void EndTurn()
    {
        if (_levelModel.CurrentTurnIndex >= LevelData.TurnCount)
            return;
        
        // TODO: do real end turn logic
        _levelModel.Money += 10;
        
        // pass to next turn
        if (_levelModel.CurrentTurnIndex + 1 < LevelData.TurnCount)
        {
            _levelModel.CurrentTurnIndex++;
        }
        else
        {
            EndLevel();
        }
    }

    private void EndLevel()
    {
        bool won = false; // TODO: do real win conditions check
        _levelModel.Won = won;
        _levelModel.LevelEnded?.Invoke(LevelData);
        LevelEnded?.Invoke(LevelData);
    }
}