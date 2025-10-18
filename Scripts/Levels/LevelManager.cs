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

#if TOOLS // TOOLS is defined when building with the Debug configuration (editor and editor player) (https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_features.html#preprocessor-defines)
        if (_debugLevelData is not null)
        {
            Init(_debugLevelData);
        }
#endif
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
                    TryPlanting(cell);
                }
            }
        }
    }

    private void TryPlanting(ICell cell)
    {
        if (cell.Plant is not null)
            return;
        if (_levelModel.SelectedPlantData is null)
            return;
        if(!_levelModel.SelectedPlantData.AllowedGroundTypes.Contains(cell.GroundType) || _levelModel.Money < _levelModel.SelectedPlantData.Cost)
            return;

        _levelModel.SetMoney(_levelModel.Money - _levelModel.SelectedPlantData.Cost);
        Plant spawnedPlant = PlantManager.Instance.SpawnPlant(_levelModel.SelectedPlantData, Grid.GetCellWorldPosition(cell) + Vector3.Up);
        cell.SetPlant(spawnedPlant);
        spawnedPlant.Cell = cell;
        GD.Print($"Planted '{_levelModel.SelectedPlantData.Name}' at {cell.Position}");
        OnPlantation(spawnedPlant, cell);
    }

    private void OnPlantation(Plant plant, ICell plantCell)
    {
        plant.Data.TerraformingAction?.Apply(Grid, plantCell);

        _levelModel.SetMoney(_levelModel.Money + plant.Data.DefaultMoneyGain);
        
        if (_levelModel.Money <= 0)
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