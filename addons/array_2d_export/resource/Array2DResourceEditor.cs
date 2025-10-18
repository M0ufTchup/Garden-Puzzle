#if TOOLS
using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public partial class Array2DResourceEditor : EditorProperty
{
    private static readonly StringName SceneName = "res://addons/array_2d_export/resource/array_2d_resource.tscn";
    private readonly string _resourceBaseType;

    private SpinBox _gridSizeSpinBox;
    private Array2DResourceGrid _grid;
    private Button _resetBtn;

    private Array<Array<Resource>> _currentValue;
    private int _currentSize;

    public Array2DResourceEditor(string resourceBaseType)
    {
        _resourceBaseType = resourceBaseType;
    }

    public override void _Ready()
    {
        _currentValue = GetPropertyValue();
        _currentSize = _currentValue.Count;
        
        var scene = ResourceLoader.Load<PackedScene>(SceneName).Instantiate<VBoxContainer>();
       
        _gridSizeSpinBox = scene.GetNode<SpinBox>("GridSizeContainer/GridSizeSpinBox");
        _gridSizeSpinBox.Value = _currentValue.Count;
        _gridSizeSpinBox.ValueChanged += OnGridSizeChanged;
        
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
            _gridSizeSpinBox.Value = Utils.DefaultArraySize;
        }
        _grid.UpdateGrid(_currentValue);
    }

    private Array<Array<Resource>> GetPropertyValue()
    {
        return GetEditedObject().Get(GetEditedProperty()).AsGodotArray<Array<Resource>>();
    }

    private void OnGridSizeChanged(double value)
    {
        int newSize = (int)value;
        if (newSize % 2 == 0)
        {
            _currentSize = newSize > _currentSize ? newSize + 1 : newSize - 1;
            _gridSizeSpinBox.Value = _currentSize;
        }
        _currentValue = Utils.Default2DArray<Resource>(_currentSize);
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    private void OnCellValueChanged(Vector2I cell, Resource value)
    {
        _currentValue[cell.X][cell.Y] = value;
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    private void OnResetPressed()
    {
        _currentValue = Utils.Default2DArray<Resource>((int)_gridSizeSpinBox.Value);
        EmitChanged(GetEditedProperty(), _currentValue);
    }
}
#endif