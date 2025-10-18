#if TOOLS
using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public partial class Array2DIntGrid : GridContainer
{
    [Signal]
    public delegate void CellValueChangedEventHandler(Vector2I cell, int newValue);

    public void UpdateGrid(Array<Array<int>> newValue)
    {
        ResetGrid(newValue.Count);
        CreateNewGrid(newValue);
    }

    private void ResetGrid(int newSize)
    {
        foreach (var child in GetChildren())
        {
            var cell = (Array2DIntCellButton)child;
            cell.CellValueChanged -= OnCellValueChanged;
            cell.QueueFree();
        }
        Columns = newSize;
    }

    private void CreateNewGrid(Array<Array<int>> newValue)
    {
        for (int x = 0; x < newValue.Count; x++)
        {
            for (int y = 0; y < newValue[x].Count; y++)
            {
                var cell = new Array2DIntCellButton(x, y);
                cell.Value = newValue[x][y];
                AddChild(cell);
                cell.CellValueChanged += OnCellValueChanged;
            }
        }
    }

    private void OnCellValueChanged(Vector2I cell, int value)
    {
        EmitSignal(SignalName.CellValueChanged, cell, value);
    }
}
#endif