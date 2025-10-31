using GardenPuzzle.Grid;
using Godot;
using Godot.Collections;

namespace GardenPuzzle.Conditions;

public abstract partial class ResourceGridPatternCondition<T> : GridCondition where T : Resource
{
#pragma warning disable GD0302
    protected abstract Array<Array<T>> ResourceArray2D { get; }
#pragma warning restore GD0302
    
    public override bool IsRespected(IGrid grid, Rect2I sourceGridRect)
    {
        if (sourceGridRect.Size.X > ResourceArray2D.Count || sourceGridRect.Size.Y > ResourceArray2D[0]?.Count)
        {
            GardenLogger.LogError(this,$"[{GetType().Name}]: given sourceGridRect is too large for the configured condition (given grid rect: {sourceGridRect}, configured {{xSize={ResourceArray2D.Count}, ySize={ResourceArray2D[0]?.Count}}})");
            return false;
        }
        
        Rect2I customRect = Utilities.GetCustomRect<T>(sourceGridRect, ResourceArray2D);
        for (int i = 0; i < customRect.Size.X; i++)
        {
            for (int j = 0; j < customRect.Size.Y; j++)
            {
                if (!IsRespected(grid, sourceGridRect.Position + new Vector2I(i, j), ResourceArray2D[i][j]))
                    return false;
            }
        }

        return true;
    }
    
    protected abstract bool IsRespected(IGrid grid, Vector2I gridPosition, T expectedResource);
}