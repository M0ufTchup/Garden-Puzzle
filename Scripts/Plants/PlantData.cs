using GardenPuzzle.Ground;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class PlantData : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public Mesh Mesh { get; private set; }
    [Export] public PlantFamily Family { get; private set; }
    [Export] public int Cost { get; private set; } = 1;
    [Export] public Array<GroundType> AllowedGroundTypes { get; private set; }
    [Export] public int DefaultMoneyGain { get; private set; } = 0;
    [Export] public TerraformingAction TerraformingAction { get; private set; }
}