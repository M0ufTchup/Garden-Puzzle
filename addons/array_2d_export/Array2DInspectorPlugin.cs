#if TOOLS
using System.Text.RegularExpressions;
using Godot;

namespace Array2DExport.addons.array_2d_export;

public partial class Array2DInspectorPlugin : EditorInspectorPlugin
{
    private static readonly string RegexPattern = @"[Rr]esource(.*?)Array2D";
    
    public override bool _CanHandle(GodotObject @object) => true;

    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString,
        PropertyUsageFlags usageFlags, bool wide)
    {
        if (type == Variant.Type.Array && name.Contains("Array2D"))
        {
            if (name.Contains("IntArray2D"))
            {
                AddPropertyEditor(name, new Array2DIntEditor());
                return true;
            }

            Match resourceRegexMatch = Regex.Match(name, RegexPattern);
            if (resourceRegexMatch.Success)
            {
                AddPropertyEditor(name, new Array2DResourceEditor(resourceRegexMatch.Groups[1].Value));
                return true;
            }
        }
        return false;
    }
}
#endif