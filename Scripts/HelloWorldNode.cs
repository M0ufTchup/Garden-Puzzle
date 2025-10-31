using Godot;

namespace GardenPuzzle;

[GlobalClass]
public partial class HelloWorldNode : Node
{
    public override void _Ready()
    {
        base._Ready();
        GardenLogger.Log(this, "Hello World!");
    }
}