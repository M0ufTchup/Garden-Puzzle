#if TOOLS
using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public partial class Array2DResourceGrid : GridContainer
{
    [Signal]
    public delegate void CellValueChangedEventHandler(Vector2I cell, Resource newValue);
    public string ResourceBaseType;

    public void UpdateGrid(Array<Array<Resource>> newValue)
    {
        ResetGrid(newValue.Count);
        CreateNewGrid(newValue);
    }

    private void ResetGrid(int newSize)
    {
        foreach (var child in GetChildren())
        {
            var cell = (Array2DResourcePicker)child;
            cell.CellValueChanged -= OnCellValueChanged;
            cell.QueueFree();
        }
        Columns = newSize;
    }

    private void CreateNewGrid(Array<Array<Resource>> newValue)
    {
        for (int x = 0; x < newValue.Count; x++)
        {
            for (int y = 0; y < newValue[x].Count; y++)
            {
                var cell = new Array2DResourcePicker(new Vector2I(x, y), ResourceBaseType);
                cell.EditedResource = newValue[x][y];
                AddChild(cell);
                cell.CellValueChanged += OnCellValueChanged;
            }
        }
    }

    private void OnCellValueChanged(Vector2I cell, Resource resource)
    {
        EmitSignal(SignalName.CellValueChanged, cell, resource);
    }
}
#endif