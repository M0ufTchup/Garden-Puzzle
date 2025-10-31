using GardenPuzzle.Grid;
using Godot;

namespace GardenPuzzle.Conditions;

[GlobalClass]
public abstract partial class GridCondition : Resource
{
    [Export] private string _editorDescription;

    public bool IsRespected(IGrid grid, Vector2I sourceGridPosition) => IsRespected(grid, new Rect2I(sourceGridPosition, Vector2I.One));
    public abstract bool IsRespected(IGrid grid, Rect2I sourceGridRect);
}