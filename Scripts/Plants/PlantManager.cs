using System.Collections.Generic;
using Godot;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class PlantManager : Node
{
    public static PlantManager Instance { get; private set; }

    public IReadOnlyList<Plant> Plants => _plants;
    private List<Plant> _plants = new();
    
    [Export] private PackedScene _plantScene;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        if (Instance is not null && Instance != this)
        {
            GD.PrintErr("Cannot have multiple instances of PlantManager");
            QueueFree();
            return;
        }

        Instance = this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (Instance is not null && Instance == this)
        {
            Instance = null;
        }
    }

    public void SpawnPlant(Node3D parent, Vector3 globalPosition)
    {
        Plant plant = _plantScene.Instantiate<Plant>();
        plant.AddChild(parent);
        plant.GlobalPosition = globalPosition;
    }

    public void KillPlant(Plant plant)
    {
        plant.QueueFree();
        _plants.Remove(plant);
    }
    
    public void ClearPlants()
    {
        foreach (Plant plant in _plants)
            plant.QueueFree();
        _plants.Clear();
    }
}