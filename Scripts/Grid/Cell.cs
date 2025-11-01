using GardenPuzzle.Ground;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Grid;

public class Cell : ICell
{
    public Cell(Vector2I position, GroundType groundType)
    {
        AllowPlanting = false;
        Position = position;
        GroundType = groundType;
    }

    public bool AllowPlanting { get; set; }
    public Vector2I Position { get; }
    public GroundType GroundType { get; set; }
    public Plant Plant { get; set; }
}