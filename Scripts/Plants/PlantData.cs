using GardenPuzzle.Ground;
using GardenPuzzle.MoneyGains;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class PlantData : Resource
{
    [ExportCategory("Plant Identity")]
    [Export] public string Name { get; private set; }
    [Export] public Mesh Mesh { get; private set; }
    [Export] public PlantFamily Family { get; private set; }
    [Export] public Vector2I Size { get; private set; } = Vector2I.One;

    [ExportCategory("Plant grounds")]
    [Export] public Array<GroundType> AllowedGroundTypes { get; private set; }
    [Export] public Array<GroundType> KillGroundTypes { get; private set; }
    [Export] public TerraformingAction TerraformingAction { get; private set; }
    
    [ExportCategory("Plant economy")]
    [Export] public int Cost { get; private set; } = 1;
    [Export] public int DefaultMoneyGain { get; private set; } = 0;
    [Export] public Array<MoneyGain> AdditionalMoneyGains { get; private set; }
}
