using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Edge
{
    Up,
    Down,
    Left,
    Right,
    None
}

public class MouseInteractor : MonoBehaviour
{
    TileObject objectInteracting = null;
    private void Update()
    {
        var previousInteractable = objectInteracting;
        if (InventoryManager.inventoryOpen)
        {
            objectInteracting = null;
            return;
        }
        bool clickDown = Input.GetMouseButtonDown(0);
        bool clickHeld = Input.GetMouseButton(0);
        if (clickDown || clickHeld)
        {
            objectInteracting = CheckForInteractables();
            if (objectInteracting == null) return;
            
            if (clickDown || previousInteractable != objectInteracting)
            {
                objectInteracting.ClickDown(this, clickDown);
            }

            if (clickHeld)
            {
                objectInteracting.ClickHeld(this);
            }
        }
        else
        {
            objectInteracting = null;
        }
    }

    Vector2 FractionalComponent(Vector2 vector)
    {
        return new Vector2(vector.x - Mathf.Floor(vector.x), vector.y - Mathf.Floor(vector.y));
    }
    public Edge OnTileEdge(float edgeCornerSize)
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 mouseTilePosition = FractionalComponent(mouseWorldPosition);
        float x = mouseTilePosition.x;
        float y = mouseTilePosition.y;
        Vector2 distanceToCorner = new Vector2(0.5f - Mathf.Abs(0.5f - x), 0.5f - Mathf.Abs(0.5f - y));
        if (distanceToCorner.x < edgeCornerSize && distanceToCorner.y < edgeCornerSize) return Edge.None;
        if (y < x && y < 1 - x) return Edge.Down;
        if (y < x && y > 1 - x) return Edge.Right;
        if (y > x && y < 1 - x) return Edge.Left;
        return Edge.Up;
    }

    private TileObject CheckForInteractables()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Interactable"));
        if (hits.Length == 0)
        {
            return null;
        }
        var sortedHits = hits.OrderBy(hit => hit.transform.position.y).ToArray();

        return sortedHits[0].collider.GetComponent<TileObject>();
    }
}
