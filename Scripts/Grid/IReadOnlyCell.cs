using GardenPuzzle.Ground;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Grid;

public interface IReadOnlyCell
{
    Vector2I Position { get; }
    GroundType GroundType { get; }
    Plant Plant { get; }
}