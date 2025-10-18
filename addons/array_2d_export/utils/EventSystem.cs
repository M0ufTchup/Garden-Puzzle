#if TOOLS
using System;
using Godot;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public class EventSystem
{
    private static readonly Lazy<EventSystem> LazyInstance = new(() => new EventSystem());
    public static EventSystem Instance => LazyInstance.Value;
    
    public delegate void CellValueChangedEventHandler(Vector2I cell, int value);
    public event CellValueChangedEventHandler CellValueChanged;
    public void RaiseCellValueChanged(Vector2I cell, int value) => CellValueChanged?.Invoke(cell, value);
}
#endif