using GardenPuzzle.Grid;
using Godot;

namespace GardenPuzzle.Conditions;

[GlobalClass]
public abstract partial class GridCondition : Resource
{
    [Export] private string _editorDescription;

    public abstract bool IsRespected(IGrid grid, ICell cellToCheck);
}