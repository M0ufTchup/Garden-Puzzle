#if TOOLS
using System;
using Godot;

namespace Array2DExport.addons.array_2d_export;

public partial class Array2DIntCellButton : SpinBox
{
	[Signal] public delegate void CellValueChangedEventHandler(Vector2I cell, int value);
	
	private readonly Vector2I _cell;
	public Array2DIntCellButton() {} // Add this or random crash i guess ?
	
	public Array2DIntCellButton(int x, int y)
	{
		_cell = new Vector2I(x, y);
		this.ValueChanged += OnValueChanged;
		MinValue = 0;
		MaxValue = Double.MaxValue;
	}

	private void OnValueChanged(double value)
	{
		EmitSignal(SignalName.CellValueChanged, _cell, value);
	}
}
#endif