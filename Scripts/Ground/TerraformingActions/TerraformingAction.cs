using GardenPuzzle.Grid;
using Godot;

namespace GardenPuzzle.Ground;

[GlobalClass]
public abstract partial class TerraformingAction : Resource
{
    public void Apply(IGrid grid, Vector2I gridPositionSource) => Apply(grid, new Rect2I(gridPositionSource, Vector2I.One));
    public abstract void Apply(IGrid grid, Rect2I gridRectSource);
}