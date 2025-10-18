using System.Collections.Generic;
using GardenPuzzle.Grid;
using GardenPuzzle.Ground;
using Godot;

[GlobalClass]
public partial class GameGrid : GridMap, IGrid
{
	[Export] private GameGridConfig _config;

	private readonly Dictionary<Vector2I, ICell> _cells = new();
	
	public override void _Ready()
	{
		base._Ready();
		
		foreach (Vector3I usedCellPosition in GetUsedCells())
		{
			int cellItem = GetCellItem(usedCellPosition);
			string usedCellItemName = MeshLibrary.GetItemName(cellItem);

			GD.Print($"[GameGrid] Cell at {usedCellPosition} uses '{usedCellItemName ?? "null"}'");
			if (_config.GroundMeshes.TryGetValue(usedCellItemName, out GroundType groundType))
			{
				Vector2I simplifiedCellPosition = new Vector2I(usedCellPosition.X, usedCellPosition.Z);
				_cells[simplifiedCellPosition] = new Cell(simplifiedCellPosition, groundType);
				GD.Print($"[GameGrid] GridMap cell at {usedCellPosition} registered as '{groundType.Name}' at {simplifiedCellPosition}'");
			}
		}

		GD.Print($"[GameGrid] GridMap initialized with {_cells.Count} cells.");
	}

	public IReadOnlyCell GetReadOnlyCell(Vector2I position) => GetCell(position);
	public ICell GetCell(Vector2I position) => _cells.GetValueOrDefault(position);
	public ICell GetCell(Vector3 worldPosition)
	{
		Vector3I mapPosition = LocalToMap(ToLocal(worldPosition));
		return GetCell(new Vector2I(mapPosition.X, mapPosition.Z));
	}
}
