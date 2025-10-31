using GardenPuzzle.Grid;
using Godot;

namespace GardenPuzzle.CountOf;

[GlobalClass]
public abstract partial class CountOf : Resource
{
    public abstract int ComputeCount(IGrid grid, Rect2I gridRect);
}