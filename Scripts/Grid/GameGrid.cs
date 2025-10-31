using System;
using System.Collections.Generic;
using GardenPuzzle.Ground;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.Grid;

[GlobalClass]
public partial class GameGrid : GridMap, IGrid
{
	public event Action<ICell> CellPlantChanged;
	public event Action<ICell> CellGroundChanged;
	
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
			GD.Print($"[GameGrid] Cell at {usedCellPosition} uses '{usedCellItemName ?? "null"}'");
			if (_config.TryGetMeshDefinition(usedCellItemName, out GroundMeshDefinition groundMeshDefinition))
			{
				Vector2I simplifiedCellPosition = new Vector2I(usedCellPosition.X, usedCellPosition.Z);
				_cells[simplifiedCellPosition] = new Cell(simplifiedCellPosition, groundMeshDefinition.GroundType);
				SetCellItem(usedCellPosition, groundMeshDefinition.MeshLibraryId);
				
				GD.Print($"[GameGrid] GridMap cell at {usedCellPosition} registered as '{groundMeshDefinition.GroundType.Name}' at {simplifiedCellPosition}'");
			}
		}

		GD.Print($"[GameGrid] GridMap initialized with {_cells.Count} cells.");
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

	public void SetCellGroundType(Vector2I cellPosition, GroundType groundType) => SetCellGroundType(GetCell(cellPosition), groundType);
	public void SetCellGroundType(ICell cell, GroundType groundType)
	{
		if (cell is null || !_cells.TryGetValue(cell.Position, out Cell internalCell))
			return;
		internalCell.SetGroundType(groundType);

		if (_config.TryGetMeshDefinition(groundType, out GroundMeshDefinition groundMeshDefinition))
		{
			Vector3I mapPosition = new Vector3I(internalCell.Position.X, 0, internalCell.Position.Y);
			SetCellItem(mapPosition, groundMeshDefinition.MeshLibraryId);
		}
		
        GD.Print($"[GridMap] CellGround changed at '{cell.Position}' to '{groundType?.Name ?? "null"}'");
		CellGroundChanged?.Invoke(internalCell);
	}

	public void SetCellPlant(Vector2I cellPosition, Plant plant) => SetCellPlant(GetCell(cellPosition), plant);
	public void SetCellPlant(ICell cell, Plant plant)
	{
		if (cell is null || !_cells.TryGetValue(cell.Position, out Cell internalCell))
			return;
		internalCell.SetPlant(plant);
		GD.Print($"[GameGrid] Cell at {cell.Position} has new plant '{plant?.Data.Name ?? "null"}'");
		CellPlantChanged?.Invoke(internalCell);
	}
}
