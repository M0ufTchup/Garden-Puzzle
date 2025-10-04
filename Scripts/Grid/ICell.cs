using GardenPuzzle.Ground;
using GardenPuzzle.Plants;

namespace GardenPuzzle.Grid;

public interface ICell : IReadOnlyCell
{
    void SetGroundType(GroundType groundType);
    void SetPlant(Plant plant);
}