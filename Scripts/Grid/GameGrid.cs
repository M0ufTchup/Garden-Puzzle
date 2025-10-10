using Godot;
using Godot.Collections;

public partial class GameGrid : Node3D
{
	[Export]
	private PackedScene Cell;

	private const float RayLength = 1000.0f;
	
}
