using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileObject : MonoBehaviour
{
    public static Dictionary<Vector2, TileObject> objectPositions;

    public static TileObject GetTileAtPosition(Vector2 position)
    {
        if (objectPositions.TryGetValue(position, out TileObject tile) {
            return tile;
        }
        else
        {
            return null;
        }
    }

    public static bool PositionEmpty(Vector2 position)
    {
        return objectPositions.ContainsKey(position);
    }

    public static List<(TileObject, Vector2)> GetNeighbors(Vector2 position)
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        List<(TileObject, Vector2)> neighbors = new();
        foreach (Vector2 dir in directions)
        {
            if (objectPositions.TryGetValue(position, out TileObject neighbor) {
                neighbors.Add((neighbor, dir));
            }
        }
        return neighbors;
    }

    public void Place(Vector2 position)
    {
        objectPositions[position] = this;
        UpdateNeighbors();
    }

    public void UpdateNeighbors()
    {

    }

    public void Update(TileObject object, Vector2 direction)
    {
        
    }
}
