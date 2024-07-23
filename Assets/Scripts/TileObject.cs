using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BoundingBox // Centered around (0,0)
{
    public int top;
    public int left;
    public int bottom;
    public int right;
    public static readonly BoundingBox singleTile = new BoundingBox(0, 0, 0, 0);

    public BoundingBox(int top, int left, int bottom, int right)
    {
        this.top = top;
        this.left = left;
        this.bottom = bottom;
        this.right = right;
    }

    public BoundingBox(BoundingBox b, Vector2 translation)
    {
        this.top = b.top + (int)translation.y;
        this.left = b.left + (int)translation.x;
        this.bottom = b.bottom + (int)translation.y;
        this.right = b.right + (int)translation.x;
    }

    public Vector3 Center()
    {
        return new Vector3((left + right) / 2, (top + bottom) / 2, 0);
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

public abstract class TileObject : MonoBehaviour
{
    [SerializeField] GameObject floorItem;
    [SerializeField] string itemName = "";
    [SerializeField] GameObject breakingEffect;
    public SpriteRenderer spriteRenderer;

    public static Dictionary<Vector2, TileObject> objectPositions = new Dictionary <Vector2, TileObject>();
    BoundingBox boundingBox = BoundingBox.singleTile;
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
        boundingBox = new BoundingBox(boundingBox, Round(transform.position));
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
            GameObject drop = Instantiate(floorItem);
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
        if (breakingEffect == null) return;
        breakingEffect.SetActive(breaking);
    }

    public virtual void TileUpdate(TileObject neighborObject, Vector2 direction, bool neighborRemoved) { }

    public virtual void ClickDown(MouseInteractor mouse) { }
    public virtual void ClickHeld(MouseInteractor mouse) { }
}
