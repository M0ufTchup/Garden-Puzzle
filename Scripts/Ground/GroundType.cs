using Godot;

namespace GardenPuzzle.Ground;

[GlobalClass]
public partial class GroundType : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public Color DebugColor { get; private set; }
}