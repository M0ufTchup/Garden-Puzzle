using Godot;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class PlantData : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public Mesh Mesh { get; private set; }
}