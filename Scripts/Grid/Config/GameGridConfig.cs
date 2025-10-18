using System.Collections.Generic;
using GardenPuzzle.Ground;
using Godot;

namespace GardenPuzzle.Grid;

[GlobalClass]
public partial class GameGridConfig : Resource
{
	public IReadOnlyList<GroundMeshDefinition> GroundMeshDefinitions => _groundMeshDefinitions;
	[Export] private Godot.Collections.Array<GroundMeshDefinition> _groundMeshDefinitions = new();
	
	private bool _internalDictionariesPopulated = false;
	private Dictionary<GroundType, GroundMeshDefinition> _definitionsPerGroundType;
	private Dictionary<string, GroundMeshDefinition> _definitionsPerName;

	public bool TryGetMeshDefinition(GroundType groundType, out GroundMeshDefinition groundMeshDefinition)
	{
		PopuplateInternalDictionaries();
		return _definitionsPerGroundType.TryGetValue(groundType, out groundMeshDefinition);
	}

	public bool TryGetMeshDefinition(string name, out GroundMeshDefinition groundMeshDefinition)
	{
		PopuplateInternalDictionaries();
		return _definitionsPerName.TryGetValue(name,  out groundMeshDefinition);
	}
	
	private void PopuplateInternalDictionaries()
	{
		if (_internalDictionariesPopulated)
			return;

		_definitionsPerGroundType = new Dictionary<GroundType, GroundMeshDefinition>();
		_definitionsPerName = new Dictionary<string, GroundMeshDefinition>();
		foreach (GroundMeshDefinition groundMeshDefinition in _groundMeshDefinitions)
		{
			_definitionsPerGroundType.Add(groundMeshDefinition.GroundType, groundMeshDefinition);
			_definitionsPerName.Add(groundMeshDefinition.Name, groundMeshDefinition);
		}
	}
}