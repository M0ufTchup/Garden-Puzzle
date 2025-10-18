using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

[GlobalClass]
public partial class DebugResource : Resource
{
    [Export] public Array<Array<int>> DebugIntArray2D;
    [Export] public Array<Array<int>> DebugTypoArray2D;
    [Export] public Array<Array<Resource>> PrimaryResourceShortcutArray2D;
    [Export] public Array<Array<Resource>> PrimaryResourceImageTextureArray2D;
}