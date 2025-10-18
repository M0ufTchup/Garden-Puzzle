using GardenPuzzle.Ground;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Grid;

public class Cell : ICell
{
    public Cell(Vector2I position, GroundType groundType)
    {
        Position = position;
        GroundType = groundType;
    }

    public Vector2I Position { get; }
    public GroundType GroundType { get; private set; }
    public void SetGroundType(GroundType groundType) => GroundType = groundType;

    public Plant Plant { get; private set; }
    public void SetPlant(Plant plant) => Plant = plant;
}