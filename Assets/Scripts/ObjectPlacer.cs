using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] float range = 5;
    [SerializeField] GameObject ghostRender;
    SpriteRenderer ghostPlacementRenderer;
    BoundingBox bounds = BoundingBox.singleTile;
    GameObject currentPrefab = null;
    GameObject instantiation = null;

    private void Awake()
    {
        ghostPlacementRenderer = ghostRender.GetComponent<SpriteRenderer>();
    }

    Vector3 PlacementPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;
        return TileObject.Round(mouseWorldPosition);
        //float x = Mathf.Round(mouseWorldPosition.x);
        //float y = Mathf.Round(mouseWorldPosition.y);
        //return new Vector3(x, y, 0);
    }

    void SetGhostSprite(Sprite sprite)
    {
        ghostPlacementRenderer.sprite = sprite;
    }

    private void Update()
    {
        transform.position = PlacementPosition();
        bool placeable = Placeable(transform.position);
        if (placeable)
        {
            ghostPlacementRenderer.color = new Color(1f,1f,1f,0.6f);
            if (Input.GetMouseButtonDown(0))
            {
                (new Item()).AddOneItem(InventoryManager.itemSelected); // Remove one item from selected slot
                PlaceObject(transform.position);
            }
        }
        else
        {
            ghostPlacementRenderer.color = new Color(1f, 0.7f, 0.7f, 0.6f);
        }
    }

    public void CreateGhost(GameObject prefab)
    {
        if (currentPrefab == prefab) return;
        currentPrefab = prefab;
        InstantiatePrefab(currentPrefab);
        SetGhostSprite(instantiation.GetComponent<SpriteRenderer>().sprite);
        bounds = instantiation.GetComponent<TileObject>().GetBoundingBox();
    }

    public void InstantiatePrefab(GameObject prefab)
    {
        if (instantiation != null)
        {
            Destroy(instantiation);
        }
        instantiation = Instantiate(prefab);
        instantiation.SetActive(false);
    }

    public bool Placeable(Vector3 position)
    {
        return TileObject.BoxEmpty(new BoundingBox(bounds, position));
    }

    public void PlaceObject(Vector3 position)
    {
        instantiation.SetActive(true);
        instantiation.transform.position = position;
        TileObject tileObject = instantiation.GetComponent<TileObject>();
        tileObject.Place(position);
        instantiation = null;
        InstantiatePrefab(currentPrefab);
    }
}
