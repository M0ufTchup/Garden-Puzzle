using GardenPuzzle.Ground;
using GardenPuzzle.Plants;

namespace GardenPuzzle.Grid;

public interface ICell : IReadOnlyCell
{
    internal void SetGroundType(GroundType groundType);
    internal void SetPlant(Plant plant);
}