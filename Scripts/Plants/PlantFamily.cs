using Godot;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class PlantFamily : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public Texture Texture { get; private set; }
    [Export] public Color DebugColor { get; private set; }
}