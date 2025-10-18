using System;
using GardenPuzzle.Grid;
using GardenPuzzle.Plants;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Levels;

/// <summary>
/// Level/Game Manager. Handles the logic of a single level/game from start to finish.
/// </summary>
[GlobalClass]
public partial class LevelManager : Node3D
{
    public Action<LevelData> LevelStarted;
    public Action<LevelData> LevelEnded;
    
    public IGrid Grid { get; private set; }
    
    [Export] private LevelModel _levelModel;
    [Export] private Node3D _levelParent;
    [Export] private float _rayLength = 100;
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
        
        GameGrid levelInstance = levelData.LevelScene.Instantiate<GameGrid>();
        _levelParent.AddChild(levelInstance);
        levelInstance.Position = Vector3.Zero;

        Grid = levelInstance;
        _levelModel.Reset(levelData, Grid);
        
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

    public override void _Input(InputEvent inputEvent)
    {
        base._Input(inputEvent);
        
        if (inputEvent.IsActionPressed("interact") && inputEvent is InputEventMouseButton eventMouseButton)
        {
            Camera3D camera3D = GetViewport().GetCamera3D();
            Vector3 from = camera3D.ProjectRayOrigin(eventMouseButton.Position);
            Vector3 to = from + camera3D.ProjectRayNormal(eventMouseButton.Position) * _rayLength;
            
            var query = PhysicsRayQueryParameters3D.Create(from, to);
            query.CollideWithAreas = false;

            Dictionary result = GetWorld3D().GetDirectSpaceState().IntersectRay(query);
            if (result.TryGetValue("collider", out var collider))
            {
                Vector3 collisionPosition = (Vector3)result["position"];
                ICell cell = Grid.GetCell(collisionPosition);
                if (cell is not null)
                {
                    GD.Print($"Clicked on cell at {cell.Position} -> {cell?.GroundType.Name ?? "no ground"}");
                    
                    if (cell.Plant is null && _levelModel.SelectedPlantData is not null)
                    {
                        cell.SetPlant(PlantManager.Instance.SpawnPlant(_levelModel.SelectedPlantData, Grid.GetCellWorldPosition(cell) + Vector3.Up));
                        GD.Print($"Planted '{_levelModel.SelectedPlantData.Name}' at {cell.Position}");
                    }
                }
            }
        }
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