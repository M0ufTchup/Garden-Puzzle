using System;
using System.Collections.Generic;
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
        Grid.CellGroundChanged += OnCellGroundChanged;
        _levelModel.Reset(levelData, Grid);
        
        LevelStarted?.Invoke(LevelData);
        _levelModel.LevelStarted?.Invoke(LevelData);
    }

    private void OnCellGroundChanged(ICell cell)
    {
        Plant cellPlant = cell.Plant;
        if (cellPlant is not null)
        {
            // kill plant if on "kill ground"
            if (cellPlant.Data.KillGroundTypes?.Contains(cell.GroundType) ?? false)
            {
                for (int i = 0; i < cellPlant.GridRect.Size.X; i++)
                {
                    for (int j = 0; j < cellPlant.GridRect.Size.Y; j++)
                    {
                        Grid.SetCellPlant(cellPlant.GridRect.Position + new Vector2I(i, j), null);
                    }
                }
                
                PlantManager.Instance.KillPlant(cellPlant);
                GD.Print($"Removed plant '{cellPlant.Data.Name}'");
            }
        }
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

    private void TryPlanting(ICell inputCell)
    {
        if (inputCell.Plant is not null)
            return;
        if (_levelModel.SelectedPlantData is null)
            return;
        if(_levelModel.Money < _levelModel.SelectedPlantData.Cost)
            return;

        Rect2I plantGridRect = new Rect2I(inputCell.Position, _levelModel.SelectedPlantData.Size);
        for (int i = 0; i < plantGridRect.Size.X; i++)
        {
            for (int j = 0; j < plantGridRect.Size.Y; j++)
            {
                Vector2I gridPos = plantGridRect.Position + new Vector2I(i, j);
                ICell cell = Grid.GetCell(gridPos);
                if (cell is null || cell.Plant is not null || !_levelModel.SelectedPlantData.AllowedGroundTypes.Contains(cell.GroundType))
                    return;
            }
        }

        ICell startCell = Grid.GetCell(plantGridRect.Position);
        ICell endCell =  Grid.GetCell(plantGridRect.Position + plantGridRect.Size - Vector2I.One);
        Vector3 centeredPlantWorldPos = (Grid.GetCellWorldPosition(startCell) + Grid.GetCellWorldPosition(endCell)) * 0.5f + Vector3.Up;
        
        Plant spawnedPlant = PlantManager.Instance.SpawnPlant(_levelModel.SelectedPlantData, centeredPlantWorldPos);
        spawnedPlant.SetGridRect(plantGridRect);
        for (int i = 0; i < spawnedPlant.GridRect.Size.X; i++)
            for (int j = 0; j < spawnedPlant.GridRect.Size.Y; j++)
                Grid.SetCellPlant(spawnedPlant.GridRect.Position + new Vector2I(i, j), spawnedPlant);
        
        GD.Print($"Planted '{_levelModel.SelectedPlantData.Name}' at {{{plantGridRect}}}");
        _levelModel.SetMoney(_levelModel.Money - _levelModel.SelectedPlantData.Cost);
        OnPlantation(spawnedPlant);
    }

    private void OnPlantation(Plant plant)
    {
        plant.Data.TerraformingAction?.Apply(Grid, plant.GridRect);

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