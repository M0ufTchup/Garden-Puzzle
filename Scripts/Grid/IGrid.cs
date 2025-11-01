using System;
using GardenPuzzle.Ground;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Grid;

public interface IGrid : IReadOnlyGrid
{
    public readonly record struct PlantChangeArgs(ICell Cell, Plant OldPlant, Plant NewPlant);
    public readonly record struct GroundChangeArgs(ICell Cell, GroundType OldGround, GroundType NewGround);
    
    public event Action<PlantChangeArgs> CellPlantChanged;
    public event Action<GroundChangeArgs> CellGroundChanged;
    
    ICell GetCell(Vector3 worldPosition);
    
    void SetCellsGroundType(Rect2I rect, GroundType newGroundType);
    void SetCellGroundType(Vector2I cellPosition, GroundType newGroundType);
    void SetCellGroundType(ICell cell, GroundType newGroundType);
    
    void SetCellsPlant(Rect2I rect, Plant newPlant);
    void SetCellPlant(Vector2I cellPosition, Plant newPlant);
    void SetCellPlant(ICell cell, Plant newPlant);
    
    bool TryUnlockGridPartition(IGridPartition gridPartition);
}