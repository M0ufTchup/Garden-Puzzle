using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

public partial class DebugScript : Node
{
	// [Export] public Array<Array<int>> DebugGodotArray2D { get; private set; }
	[Export] public DebugResource ShapeArray2D { get; private set; }
}