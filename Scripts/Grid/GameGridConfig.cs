using System.Collections.Generic;
using GardenPuzzle.Ground;
using Godot;

namespace GardenPuzzle.Grid;

[GlobalClass]
public partial class GameGridConfig : Resource
{
	public IReadOnlyDictionary<string, GroundType> GroundMeshes => _groundMeshes;
	[Export] private Godot.Collections.Dictionary<string, GroundType> _groundMeshes = new();
}