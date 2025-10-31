#if TOOLS
using Godot;

namespace Array2DExport.addons.array_2d_export;

public partial class Array2DResourcePicker : EditorResourcePicker
{
    [Signal]
    public delegate void CellValueChangedEventHandler(Vector2I cell, Resource value);

    private readonly Vector2I _cell;
    
    public Array2DResourcePicker() {} // Add this or random crash i guess ?
    public Array2DResourcePicker(Vector2I cell, string baseType)
    {
        CustomMinimumSize = new Vector2(200, 50);
        Editable = true;
        
        if (!string.IsNullOrEmpty(baseType))
        {
            BaseType = baseType;
        }

        _cell = cell;
        this.ResourceChanged += OnResourceChanged;
    }

    private void OnResourceChanged(Resource resource)
    {
        EmitSignalCellValueChanged(_cell, resource);
    }
}
#endif