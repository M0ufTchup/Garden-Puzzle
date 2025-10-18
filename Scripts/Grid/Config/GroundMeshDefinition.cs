using GardenPuzzle.Ground;
using Godot;

namespace GardenPuzzle.Grid;

[GlobalClass]
public partial class GroundMeshDefinition : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public GroundType GroundType { get; private set; }
    [Export] public Mesh Mesh { get; private set; }
    
    public int MeshLibraryId;
}