using System;
using GardenPuzzle.Ground;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Grid;

public interface IGrid : IReadOnlyGrid
{
    public event Action<ICell> CellPlantChanged;
    public event Action<ICell> CellGroundChanged;
    
    ICell GetCell(Vector2I position);
    ICell GetCell(Vector3 worldPosition);

    Vector3 GetCellWorldPosition(ICell cell);
    
    void SetCellGroundType(Vector2I cellPosition, GroundType groundType);
    void SetCellGroundType(ICell cell, GroundType groundType);
    void SetCellPlant(Vector2I cellPosition, Plant plant);
    void SetCellPlant(ICell cell, Plant plant);
}