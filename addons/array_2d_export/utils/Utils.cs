#if TOOLS
using Godot;
using Godot.Collections;

namespace Array2DExport.addons.array_2d_export;

[Tool]
public static class Utils
{
    public const int DefaultArraySize = 3;

#pragma warning disable GD0302
    public static Array<Array<T>> Default2DArray<T>(int xSize = DefaultArraySize, int ySize = DefaultArraySize)
    {
        var result = new Array<Array<T>>();
        
        for (int x = 0; x < xSize; x++)
        {
            result.Add(new Array<T>());
            for (int y = 0; y < ySize; y++)
            {
                result[x].Add(default);
            }
        }

        return result;
    }
#pragma warning restore GD0302
}
#endif