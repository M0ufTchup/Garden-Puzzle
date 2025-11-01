#if TOOLS
using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public partial class Array2DIntEditor : EditorProperty
{
    private static readonly StringName SceneName = "res://addons/array_2d_export/int/array_2d_int.tscn";

    private SpinBox _xGridSizeSpinBox;
    private SpinBox _yGridSizeSpinBox;
    private Array2DIntGrid _grid;
    private Button _resetBtn;

    private Array<Array<int>> _currentValue;
    private int _xCurrentSize;
    private int _yCurrentSize;

    public override void _Ready()
    {
        _currentValue = GetPropertyValue();
        if (_currentValue.Count == 0) _currentValue = Utils.Default2DArray<int>();
        _xCurrentSize = _currentValue.Count;
        _yCurrentSize = _currentValue[0].Count;
        
        var scene = ResourceLoader.Load<PackedScene>(SceneName).Instantiate<VBoxContainer>();
        
        _xGridSizeSpinBox = scene.GetNode<SpinBox>("GridSizeContainer/XGridSizeSpinBox");
        _xGridSizeSpinBox.Value = _xCurrentSize;
        _xGridSizeSpinBox.ValueChanged += OnXGridSizeChanged;
        
        _yGridSizeSpinBox = scene.GetNode<SpinBox>("GridSizeContainer/YGridSizeSpinBox");
        _yGridSizeSpinBox.Value = _yCurrentSize;
        _yGridSizeSpinBox.ValueChanged += OnYGridSizeChanged;

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
            _currentValue = Utils.Default2DArray<int>();
            _xGridSizeSpinBox.Value = Utils.DefaultArraySize;
            _yGridSizeSpinBox.Value = Utils.DefaultArraySize;
        }
        _grid.UpdateGrid(_currentValue);
    }

    private Array<Array<int>> GetPropertyValue()
    {
        return GetEditedObject().Get(GetEditedProperty()).AsGodotArray<Array<int>>();
    }

    private void OnXGridSizeChanged(double value)
    {
        int newXSize = (int)value;
        _xGridSizeSpinBox.Value = newXSize;
        _xCurrentSize = newXSize;
        _currentValue = Utils.Default2DArray<int>(_xCurrentSize, _yCurrentSize);
        EmitChanged(GetEditedProperty(), _currentValue);
    }
    
    private void OnYGridSizeChanged(double value)
    {
        int newYSize = (int)value;
        _yGridSizeSpinBox.Value = newYSize;
        _yCurrentSize = newYSize;
        _currentValue = Utils.Default2DArray<int>(_xCurrentSize, _yCurrentSize);
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    private void OnCellValueChanged(Vector2I cell, int value)
    {
        _currentValue[cell.X][cell.Y] = value;
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    private void OnResetPressed()
    {
        _currentValue = Utils.Default2DArray<int>((int)_xGridSizeSpinBox.Value, (int)_yGridSizeSpinBox.Value);
        EmitChanged(GetEditedProperty(), _currentValue);
    }
}
#endif