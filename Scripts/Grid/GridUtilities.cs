using System;
using System.Collections.Generic;
using Godot;

namespace GardenPuzzle.Grid;

public static class GridUtilities
{
    private static readonly HashSet<Node> _nodeHashSet = new HashSet<Node>();
    private static readonly HashSet<Resource> _resourceHashSet = new();
    private static readonly Dictionary<Resource, int> _countDictionary = new();
    
    public static bool Any(this IGrid grid, Rect2I gridRect, Func<IGrid, ICell, bool> predicate)
    {
        for (int i = 0; i < gridRect.Size.X; i++)
        {
            for (int j = 0; j < gridRect.Size.Y; j++)
            {
                if (predicate(grid, grid.GetCell(gridRect.Position + new Vector2I(i, j))))
                    return true;
            }
        }

        return false;
    }

    public static bool All(this IGrid grid, Rect2I gridRect, Func<IGrid, ICell, bool> predicate)
    {
        for (int i = 0; i < gridRect.Size.X; i++)
        {
            for (int j = 0; j < gridRect.Size.Y; j++)
            {
                if (!predicate(grid, grid.GetCell(gridRect.Position + new Vector2I(i, j))))
                    return false;
            }
        }

        return true;
    }
    
    public static int Count(this IGrid grid, Rect2I gridRect, Func<IGrid, ICell, bool> predicate)
    {
        int result = 0;
        for (int i = 0; i < gridRect.Size.X; i++)
        {
            for (int j = 0; j < gridRect.Size.Y; j++)
            {
                if (predicate(grid, grid.GetCell(gridRect.Position + new Vector2I(i, j))))
                    result++;
            }
        }

        return result;
    }
    
    public static void ForEach(this IGrid grid, Rect2I gridRect, Action<IGrid, ICell> callback)
    {
        for (int i = 0; i < gridRect.Size.X; i++)
        {
            for (int j = 0; j < gridRect.Size.Y; j++)
            {
                callback.Invoke(grid, grid.GetCell(gridRect.Position + new Vector2I(i, j)));
            }
        }
    }

    /// <returns>Dictionary of the count of PlantFamilies and PlantData. Temporary object, do not cache it</returns>
    public static IReadOnlyDictionary<Resource, int> GetPlantCount(this IGrid grid, Rect2I gridRect, Rect2I ignoreRect)
    {
        _countDictionary.Clear();
        _nodeHashSet.Clear();
        
        for (int i = 0; i < gridRect.Size.X; i++)
        {
            for (int j = 0; j < gridRect.Size.Y; j++)
            {
                Vector2I gridPos = gridRect.Position + new Vector2I(i, j);
                if (ignoreRect.HasPoint(gridPos))
                    continue;
                
                ICell cell = grid.GetCell(gridPos);
                if (cell is null || cell.Plant is null)
                    continue;

                if (_nodeHashSet.Add(cell.Plant))
                {
                    if (!_countDictionary.TryAdd(cell.Plant.Data.Family, 1)) _countDictionary[cell.Plant.Data.Family]++;
                    if(!_countDictionary.TryAdd(cell.Plant.Data, 1)) _countDictionary[cell.Plant.Data]++;
                }
            }
        }

        return _countDictionary;
    }
    /// <returns>Dictionary of the count of GroundTypes. Temporary object, do not cache it</returns>
    public static IReadOnlyDictionary<Resource, int> GetGroundTypeCount(this IGrid grid, Rect2I gridRect)
    {
        _countDictionary.Clear();
        
        for (int i = 0; i < gridRect.Size.X; i++)
        {
            for (int j = 0; j < gridRect.Size.Y; j++)
            {
                ICell cell = grid.GetCell(gridRect.Position + new Vector2I(i, j));
                if (cell is null)
                    continue;

                if (!_countDictionary.TryAdd(cell.GroundType, 1)) _countDictionary[cell.GroundType]++;
            }
        }

        return _countDictionary;
    }
}