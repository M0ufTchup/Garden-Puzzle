using GardenPuzzle.Ground;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class PlantData : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public PlantFamily Family { get; private set; }
    [Export] public Array<GroundType> AllowedGroundTypes { get; private set; }
    [Export] public Mesh Mesh { get; private set; }
}