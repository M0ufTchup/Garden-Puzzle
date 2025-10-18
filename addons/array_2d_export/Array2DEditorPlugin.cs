#if TOOLS
using Godot;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public partial class Array2DEditorPlugin : EditorPlugin
{
	private Array2DInspectorPlugin _inspectorPlugin;
	
	public override void _EnterTree()
	{
		_inspectorPlugin = new Array2DInspectorPlugin();
		AddInspectorPlugin(_inspectorPlugin);
	}

	public override void _ExitTree()
	{
		RemoveInspectorPlugin(_inspectorPlugin);
	}
}
#endif
