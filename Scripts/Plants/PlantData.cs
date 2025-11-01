using System;
using GardenPuzzle.Ground;
using GardenPuzzle.MoneyGains;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class PlantData : Resource
{
    private enum RotationType { Normal, Random90Degree, SmallRandomVariance, FullRandom }
    
    [ExportCategory("Plant Identity")]
    [Export] public string Name { get; private set; }
    [Export] public PlantFamily Family { get; private set; }
    [Export] public Vector2I Size { get; private set; } = Vector2I.One;
    
    [ExportCategory("Plant Visual")]
    [Export] public PackedScene VisualScene { get; private set; }
    [Export] private RotationType _rotation = RotationType.Normal;
    [Export] public float CustomScaleFactor { get; private set; } = 1f;
    [Export] public Mesh Mesh { get; private set; } //tmp

    [ExportCategory("Plant grounds")]
    [Export] public Array<GroundType> AllowedGroundTypes { get; private set; }
    [Export] public Array<GroundType> KillGroundTypes { get; private set; }
    [Export] public TerraformingAction TerraformingAction { get; private set; }
    
    [ExportCategory("Plant economy")]
    [Export] public int Cost { get; private set; } = 1;
    [Export] public int DefaultMoneyGain { get; private set; } = 0;
    [Export] public Array<MoneyGain> AdditionalMoneyGains { get; private set; }

    public float GetUpRotationInDegrees()
    {
        return _rotation switch
        {
            RotationType.Normal => -90f,
            RotationType.Random90Degree => 90 * (Random.Shared.Next() % 4),
            RotationType.SmallRandomVariance => Mathf.Lerp(-105f, -75f, Random.Shared.NextSingle()),
            RotationType.FullRandom => Random.Shared.Next() % 360,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
