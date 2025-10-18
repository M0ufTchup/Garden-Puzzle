#if TOOLS
using Godot;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public partial class Array2DIntCellButton : Button
{
	[Signal]
	public delegate void CellValueChangedEventHandler(Vector2I cell, int value);

	private Vector2I _cell;

	private Theme _theme;

	private int _value;
	public int Value
	{
		get => _value;
		set
		{
			_value = ClampValue(value, 0, 2);
			Text = _value.ToString();
			Modulate = GetColorForValue(_value);
		}
	}

	public Array2DIntCellButton() {} // Add this or random crash i guess ?
	
	public Array2DIntCellButton(int x, int y)
	{
		_cell = new Vector2I(x, y);
		GuiInput += OnCellValueChanged;
	}

	private void OnCellValueChanged(InputEvent @event)
	{
		if (@event is not InputEventMouseButton inputEvent) return;
		if (inputEvent.Pressed && inputEvent.ButtonIndex == MouseButton.Left)
		{
			Value++;
			EmitSignal(SignalName.CellValueChanged, _cell, Value);
		}
		else if (inputEvent.Pressed && inputEvent.ButtonIndex == MouseButton.Right)
		{
			Value--;
			EmitSignal(SignalName.CellValueChanged, _cell, Value);
		}
	}

	private Color GetColorForValue(int value)
	{
		if (value == 1) return Colors.Red;
		if (value == 2) return Colors.Orange;		
		return Colors.White;
	}

	private int ClampValue(int value, int min, int max)
	{
		if (value < min) return max;
		if (value > max) return min;
		return value;
	}
}
#endif