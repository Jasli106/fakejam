using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[System.Serializable]
public class BoundingBox // Centered around (0,0)
{
    public int top;
    public int left;
    public int bottom;
    public int right;

    public BoundingBox()
    {
        this.top = 0;
        this.left = 0;
        this.bottom = 0;
        this.right = 0;
    }

    public BoundingBox(int top, int left, int bottom, int right)
    {
        this.top = top;
        this.left = left;
        this.bottom = bottom;
        this.right = right;
    }

    public BoundingBox(BoundingBox b, Vector2 translation)
    {
        this.top = b.top + (int)Mathf.Round(translation.y);
        this.left = b.left + (int)Mathf.Round(translation.x);
        this.bottom = b.bottom + (int)Mathf.Round(translation.y);
        this.right = b.right + (int)Mathf.Round(translation.x);
    }

    public Vector3 Center()
    {
        return new Vector3((left + right) / 2, (top + bottom) / 2, 0);
    }

    public override string ToString()
    {
        return $"y:{bottom} to {top},  x:{left} to {right}";
    }

    public static TileBase GetTileAtWorldPosition(Vector3 worldPosition, Tilemap tilemap)
    {
        // Convert the world position to the tilemap cell position
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // Retrieve the tile at the cell position
        TileBase tile = tilemap.GetTile(cellPosition);

        return tile;
    }

    public bool Grounded(Tilemap map)
    {
        foreach (var pos in Positions())
        {
            TileBase tile = GetTileAtWorldPosition(pos, map);
            TileBase tileBelow = GetTileAtWorldPosition(pos + Vector2.down, map);
            if (!Ground(tile) || tileBelow == null)
            {
                return false;
            }
        }
        return true;
    }

    public static bool Ground(TileBase tile)
    {
        return !(tile == null || tile.name.Contains("Water"));
    }

    public List<Vector2> Positions()
    {
        List<Vector2> result = new List<Vector2>();
        for (int x = left; x <= right; ++x)
        {
            for (int y = bottom; y <= top; ++y)
            {
                result.Add(new Vector2(x, y));
            }
        }
        return result;
    }

    public List<Vector2> Neighbors()
    {
        List<Vector2> result = new List<Vector2>();
        for (int x = left; x <= right; ++x)
        {
            result.Add(new Vector2(x, bottom - 1));
            result.Add(new Vector2(x, top + 1));
        }
        for (int y = bottom; y <= top; ++y)
        {
            result.Add(new Vector2(left - 1, y));
            result.Add(new Vector2(right + 1, y));
        }
        return result;
    }

    public List<(Vector2, Vector2)> NeighborsWithDirections()
    {
        List<(Vector2, Vector2)> result = new List<(Vector2, Vector2)>();
        for (int x = left; x <= right; ++x)
        {
            result.Add((new Vector2(x, bottom - 1), Vector2.down));
            result.Add((new Vector2(x, top + 1), Vector2.up));
        }
        for (int y = bottom; y <= top; ++y)
        {
            result.Add((new Vector2(left - 1, y), Vector2.left));
            result.Add((new Vector2(right + 1, y), Vector2.right));
        }
        return result;
    }
}


public abstract class Toggleable : MonoBehaviour
{
    //public bool startActive = true;
    protected bool active = true;
    public abstract void SetActive(bool value);

    /*public virtual void Start()
    {
        SetActive(startActive);
    }*/
}

public abstract class TileObject : MonoBehaviour
{
    [SerializeField] string itemName = "";
    [SerializeField] List<Toggleable> breakingEffects = new List<Toggleable>();
    public SpriteRenderer spriteRenderer;

    public static Dictionary<Vector2, TileObject> objectPositions = new Dictionary <Vector2, TileObject>();
    [SerializeField] BoundingBox boundingBox = new BoundingBox();
    public float breakTime = 1f;
    public BoundingBox GetBoundingBox()
    {
        return boundingBox;
    }

    public static TileObject GetTileAtPosition(Vector2 position)
    {
        if (objectPositions.TryGetValue(position, out TileObject tile)) {
            return tile;
        }
        else
        {
            return null;
        }
    }

    public static Vector3 Round(Vector3 value)
    {
        return new Vector3(Mathf.Round(value.x), Mathf.Round(value.y), Mathf.Round(value.z));
    }

    public virtual void Start()
    {
        SetBreaking(false);
    }

    public static bool PositionEmpty(Vector2 position)
    {
        return !objectPositions.ContainsKey(position);
    }

    public static bool BoxEmpty(BoundingBox bb)
    {
        foreach (Vector2 pos in bb.Positions())
        {
            if (!PositionEmpty(pos))
            {
                return false;
            }
        }
        return true;
    }

    public static List<(TileObject, Vector2)> GetNeighbors(BoundingBox b)
    {
        List<(TileObject, Vector2)> neighbors = new();
        foreach (var neighbor in b.NeighborsWithDirections())
        {
            if (objectPositions.TryGetValue(neighbor.Item1, out TileObject obj)) {
                neighbors.Add((obj, neighbor.Item2));
            }
        }
        return neighbors;
    }

    public List<(TileObject, Vector2)> GetNeighbors()
    {
        return GetNeighbors(boundingBox);
    }

    public virtual void Place(Vector2 position)
    {
        position = Round(position);
        transform.position = position;
        boundingBox = new BoundingBox(boundingBox, position);
        foreach (Vector2 pos in boundingBox.Positions())
        {
            objectPositions[pos] = this;
        }
        UpdateSelf();
        UpdateNeighbors(false);
    }

    public virtual void Remove()
    {
        foreach (Vector2 pos in boundingBox.Positions())
        {
            objectPositions.Remove(pos);
        }
        UpdateNeighbors(true);
    }

    public void Break()
    {
        Remove();
        if (itemName != "")
        {
            GameObject drop = Instantiate(FloorItemManager.Prefab());
            drop.transform.position = boundingBox.Center();
            drop.GetComponent<FloorItem>().SetItem(new Item(itemName, 1));
        }
        Destroy(gameObject);
    }

    public void UpdateSelf()
    {
        foreach (var neighbor in GetNeighbors())
        {
            TileUpdate(neighbor.Item1, neighbor.Item2, false);
        }
    }

    public void UpdateNeighbors(bool removed)
    {
        foreach (var neighbor in GetNeighbors())
        {
            neighbor.Item1.TileUpdate(this, -neighbor.Item2, removed);
        }
    }

    public void SetBreaking(bool breaking)
    {
        breakingEffects.ForEach(effect => effect.SetActive(breaking));
    }

    public virtual void TileUpdate(TileObject neighborObject, Vector2 direction, bool neighborRemoved) { }

    public virtual void ClickDown(MouseInteractor mouse, bool firstClick = true) { }
    public virtual void ClickHeld(MouseInteractor mouse) { }

    public virtual void UIClosed() { }
}
