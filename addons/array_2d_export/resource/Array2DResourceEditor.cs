#if TOOLS
using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public partial class Array2DResourceEditor : EditorProperty
{
    private static readonly StringName SceneName = "res://addons/array_2d_export/resource/array_2d_resource.tscn";
    private readonly string _resourceBaseType;

    private SpinBox _xGridSizeSpinBox;
    private SpinBox _yGridSizeSpinBox;
    private Array2DResourceGrid _grid;
    private Button _resetBtn;

    private Array<Array<Resource>> _currentValue;
    private int _xCurrentSize;
    private int _yCurrentSize;

    public Array2DResourceEditor(string resourceBaseType)
    {
        _resourceBaseType = resourceBaseType;
    }

    public override void _Ready()
    {
        _currentValue = GetPropertyValue();
        if (_currentValue.Count <= 0) _currentValue = Utils.Default2DArray<Resource>();
        _xCurrentSize = _currentValue.Count;
        _yCurrentSize = _currentValue[0].Count;
        
        var scene = ResourceLoader.Load<PackedScene>(SceneName).Instantiate<VBoxContainer>();
        
        _xGridSizeSpinBox = scene.GetNode<SpinBox>("GridSizeContainer/XGridSizeSpinBox");
        _xGridSizeSpinBox.Value = _xCurrentSize;
        _xGridSizeSpinBox.ValueChanged += OnXGridSizeChanged;
        
        _yGridSizeSpinBox = scene.GetNode<SpinBox>("GridSizeContainer/YGridSizeSpinBox");
        _yGridSizeSpinBox.Value = _yCurrentSize;
        _yGridSizeSpinBox.ValueChanged += OnYGridSizeChanged;
        
        _grid = scene.GetNode<Array2DResourceGrid>("Array2DGrid");
        _grid.ResourceBaseType = _resourceBaseType;
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
            _currentValue = Utils.Default2DArray<Resource>();
            _xGridSizeSpinBox.Value = Utils.DefaultArraySize;
            _yGridSizeSpinBox.Value = Utils.DefaultArraySize;
        }
        _grid.UpdateGrid(_currentValue);
    }

    private Array<Array<Resource>> GetPropertyValue()
    {
        return GetEditedObject().Get(GetEditedProperty()).AsGodotArray<Array<Resource>>();
    }

    private void OnXGridSizeChanged(double value)
    {
        int newXSize = (int)value;
        _xGridSizeSpinBox.Value = newXSize;
        _xCurrentSize = newXSize;
        _currentValue = Utils.Default2DArray<Resource>(_xCurrentSize, _yCurrentSize);
        EmitChanged(GetEditedProperty(), _currentValue);
    }
    
    private void OnYGridSizeChanged(double value)
    {
        int newYSize = (int)value;
        _yGridSizeSpinBox.Value = newYSize;
        _yCurrentSize = newYSize;
        _currentValue = Utils.Default2DArray<Resource>(_xCurrentSize, _yCurrentSize);
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    private void OnCellValueChanged(Vector2I cell, Resource value)
    {
        _currentValue[cell.X][cell.Y] = value;
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    private void OnResetPressed()
    {
        _currentValue = Utils.Default2DArray<Resource>((int)_xGridSizeSpinBox.Value, (int)_yGridSizeSpinBox.Value);
        EmitChanged(GetEditedProperty(), _currentValue);
    }
}
#endif