using GardenPuzzle.Ground;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Grid;

public interface ICell
{
    bool AllowPlanting { get; }
    Vector2I Position { get; }
    GroundType GroundType { get; }
    Plant Plant { get; }
}