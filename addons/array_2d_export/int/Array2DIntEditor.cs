#if TOOLS
using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public partial class Array2DIntEditor : EditorProperty
{
    private static readonly StringName SceneName = "res://addons/array_2d_export/int/array_2d_int.tscn";

    private SpinBox _gridSizeSpinBox;
    private Array2DIntGrid _grid;
    private Button _resetBtn;

    private Array<Array<int>> _currentValue;
    private int _currentSize;

    public override void _Ready()
    {
        _currentValue = GetPropertyValue();
        _currentSize = _currentValue.Count;
        
        var scene = ResourceLoader.Load<PackedScene>(SceneName).Instantiate<VBoxContainer>();
        
        _gridSizeSpinBox = scene.GetNode<SpinBox>("GridSizeContainer/GridSizeSpinBox");
        _gridSizeSpinBox.Value = _currentValue.Count;
        _gridSizeSpinBox.ValueChanged += OnGridSizeChanged;

        _grid = scene.GetNode<Array2DIntGrid>("Array2DGrid");
        _grid.Columns = _currentValue.Count;
        _grid.CellValueChanged += OnCellValueChanged;
        
        _resetBtn = scene.GetNode<Button>("ResetButton");
        _resetBtn.Pressed += OnResetPressed;
        
        AddChild(scene);
        SetBottomEditor(scene);
        AddFocusable(_grid);
    }

    public override void _ExitTree() => _grid.CellValueChanged -= OnCellValueChanged;

    public override void _UpdateProperty()
    {
        _currentValue = GetPropertyValue();
        if (_currentValue.Count < 1)
        {
            _currentValue = Utils.Default2DArray();
            _gridSizeSpinBox.Value = Utils.DefaultArraySize;
        }
        _grid.UpdateGrid(_currentValue);
    }

    private Array<Array<int>> GetPropertyValue()
    {
        return GetEditedObject().Get(GetEditedProperty()).AsGodotArray<Array<int>>();
    }

    private void OnGridSizeChanged(double value)
    {
        int newSize = (int)value;
        if (newSize % 2 == 0)
        {
            _currentSize = newSize > _currentSize ? newSize + 1 : newSize - 1;
            _gridSizeSpinBox.Value = _currentSize;
        }
        _currentValue = Utils.Default2DArray(_currentSize);
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    private void OnCellValueChanged(Vector2I cell, int value)
    {
        _currentValue[cell.X][cell.Y] = value;
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    private void OnResetPressed()
    {
        _currentValue = Utils.Default2DArray((int)_gridSizeSpinBox.Value);
        EmitChanged(GetEditedProperty(), _currentValue);
    }
}
#endif