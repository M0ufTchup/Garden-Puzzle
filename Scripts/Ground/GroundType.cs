using Godot;
using Godot.Collections;

namespace GardenPuzzle.Ground;

[GlobalClass]
public partial class GroundType : Resource
{
    [ExportCategory("Ground Definition")]
    [Export] public string Name { get; private set; }
    [Export] public Color DebugColor { get; private set; }
    
    [ExportCategory("Ground Transformation")]
    [Export] private bool _allowTransformations = true;
    [Export] private Array<GroundType> _acceptableTransformationOrigins;
    
    public bool AllowTransformation(GroundType transformationTarget)
    {
        return _allowTransformations && (transformationTarget._acceptableTransformationOrigins is null || transformationTarget._acceptableTransformationOrigins.Count == 0 || transformationTarget._acceptableTransformationOrigins.Contains(this));
    }
}