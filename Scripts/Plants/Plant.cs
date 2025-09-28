using Godot;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class Plant : Node3D
{
    public PlantData Data { get; private set; }

    [Export] private MeshInstance3D _meshInstance;
    
    public void SetData(PlantData data)
    {
        Data = data;
        _meshInstance.Mesh = Data.Mesh;
    }
}