using Godot;

namespace GardenPuzzle;

[GlobalClass]
public partial class HelloWorldNode : Node
{
    public override void _Ready()
    {
        base._Ready();
        GD.Print("Hello World!");
    }
}