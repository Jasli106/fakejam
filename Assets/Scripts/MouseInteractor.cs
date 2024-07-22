using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseInteractor : MonoBehaviour
{
    private void Update()
    {
        if (InventoryManager.inventoryOpen) return;
        bool clickDown = Input.GetMouseButtonDown(0);
        bool clickHeld = Input.GetMouseButton(0);
        if (clickDown || clickHeld)
        {
            var interactable = CheckForInteractables();
            if (interactable == null) return;
            
            if (clickDown)
            {
                interactable.ClickDown(this);
            }
            if (clickHeld)
            {
                interactable.ClickHeld(this);
            }
        }
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
