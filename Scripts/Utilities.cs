using Godot;
using Godot.Collections;

namespace GardenPuzzle;

public static class Utilities
{
#pragma warning disable GD0302
    public static Rect2I GetCustomRect<T>(Rect2I sourceGridRect, Array<Array<T>> array2D) where T : Resource
    {
        Vector2I array2DSize = new Vector2I(array2D.Count, array2D[0].Count);
        return new Rect2I(sourceGridRect.Position - ((array2DSize - sourceGridRect.Size) / 2), array2DSize);
    }
#pragma warning restore GD0302

    public static Rect2I ExpandRect(Rect2I sourceRect, int expandRange)
    {
        return new Rect2I(sourceRect.Position - Vector2I.One * expandRange, sourceRect.Size + Vector2I.One * expandRange * 2);
    }
}