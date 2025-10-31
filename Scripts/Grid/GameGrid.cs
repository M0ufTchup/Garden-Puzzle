using System;
using System.Collections.Generic;
using GardenPuzzle.Ground;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Grid;

[GlobalClass]
public partial class GameGrid : GridMap, IGrid
{
	public event Action<IGrid.PlantChangeArgs> CellPlantChanged;
	public event Action<IGrid.GroundChangeArgs> CellGroundChanged;
	
	[Export] private GameGridConfig _config;

	private readonly Dictionary<Vector2I, Cell> _cells = new();
	
	public override void _Ready()
	{
		base._Ready();

		foreach (GroundMeshDefinition groundMeshDefinition in _config.GroundMeshDefinitions)
		{
			int newMeshLibraryItemId = MeshLibrary.GetLastUnusedItemId();
			MeshLibrary.CreateItem(newMeshLibraryItemId);
			MeshLibrary.SetItemName(newMeshLibraryItemId, groundMeshDefinition.Name);
			MeshLibrary.SetItemMesh(newMeshLibraryItemId, groundMeshDefinition.Mesh);
			MeshLibrary.SetItemShapes(newMeshLibraryItemId, MeshLibrary.GetItemShapes(0));
			MeshLibrary.SetItemMeshCastShadow(newMeshLibraryItemId, MeshLibrary.GetItemMeshCastShadow(0));
			MeshLibrary.SetItemMeshTransform(newMeshLibraryItemId, MeshLibrary.GetItemMeshTransform(0));
			groundMeshDefinition.MeshLibraryId = newMeshLibraryItemId;
		}
		
		foreach (Vector3I usedCellPosition in GetUsedCells())
		{
			string usedCellItemName = MeshLibrary.GetItemName(GetCellItem(usedCellPosition));
			GardenLogger.Log(this, $"Cell at {usedCellPosition} uses '{usedCellItemName ?? "null"}'");
			if (_config.TryGetMeshDefinition(usedCellItemName, out GroundMeshDefinition groundMeshDefinition))
			{
				Vector2I simplifiedCellPosition = new Vector2I(usedCellPosition.X, usedCellPosition.Z);
				_cells[simplifiedCellPosition] = new Cell(simplifiedCellPosition, groundMeshDefinition.GroundType);
				SetCellItem(usedCellPosition, groundMeshDefinition.MeshLibraryId);
				
				GardenLogger.Log(this, $"Cell at {usedCellPosition} registered as '{groundMeshDefinition.GroundType.Name}' at {simplifiedCellPosition}'");
			}
		}

		GardenLogger.Log(this, $"GridMap initialized with {_cells.Count} cells.");
	}

	public ICell GetReadOnlyCell(Vector2I position) => GetCell(position);
	public ICell GetCell(Vector2I position) => _cells.GetValueOrDefault(position);
	public ICell GetCell(Vector3 worldPosition)
	{
		Vector3I mapPosition = LocalToMap(ToLocal(worldPosition));
		return GetCell(new Vector2I(mapPosition.X, mapPosition.Z));
	}

	public Vector3 GetCellWorldPosition(ICell cell)
	{
		Vector3I mapPosition = new Vector3I(cell.Position.X, 0, cell.Position.Y);
		return ToGlobal(MapToLocal(mapPosition));
	}

	public void SetCellsGroundType(Rect2I rect, GroundType newGroundType)
	{
		for (int i = 0; i < rect.Size.X; i++)
			for (int j = 0; j < rect.Size.Y; j++)
				SetCellGroundType(rect.Position + new Vector2I(i, j), newGroundType);
	}
	public void SetCellGroundType(Vector2I cellPosition, GroundType newGroundType) => SetCellGroundType(GetCell(cellPosition), newGroundType);
	public void SetCellGroundType(ICell cell, GroundType newGroundType)
	{
		if (cell is null || !_cells.TryGetValue(cell.Position, out Cell internalCell))
			return;
		if (!internalCell.GroundType.AllowTransformation(newGroundType))
			return;
		GroundType oldGroundType = internalCell.GroundType;
		internalCell.SetGroundType(newGroundType);

		if (_config.TryGetMeshDefinition(newGroundType, out GroundMeshDefinition groundMeshDefinition))
		{
			Vector3I mapPosition = new Vector3I(internalCell.Position.X, 0, internalCell.Position.Y);
			SetCellItem(mapPosition, groundMeshDefinition.MeshLibraryId);
		}
		
        GardenLogger.Log(this, $"CellGround changed at '{cell.Position}' to '{newGroundType?.Name ?? "null"}'");
		CellGroundChanged?.Invoke(new IGrid.GroundChangeArgs(internalCell, oldGroundType, newGroundType));
	}

	public void SetCellsPlant(Rect2I rect, Plant newPlant)
	{
		for (int i = 0; i < rect.Size.X; i++)
			for (int j = 0; j < rect.Size.Y; j++)
				SetCellPlant(rect.Position + new Vector2I(i, j), newPlant);
	}
	public void SetCellPlant(Vector2I cellPosition, Plant newPlant) => SetCellPlant(GetCell(cellPosition), newPlant);
	public void SetCellPlant(ICell cell, Plant newPlant)
	{
		if (cell is null || !_cells.TryGetValue(cell.Position, out Cell internalCell))
			return;
		Plant oldPlant = internalCell.Plant;
		internalCell.SetPlant(newPlant);
		
		GardenLogger.Log(this, $"Cell at {cell.Position} has new plant '{newPlant?.Data.Name ?? "null"}'");
		CellPlantChanged?.Invoke(new IGrid.PlantChangeArgs(internalCell, oldPlant, newPlant));
	}
}
