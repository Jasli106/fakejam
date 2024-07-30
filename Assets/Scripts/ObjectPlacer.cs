using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] Tilemap placementMap;
    [SerializeField] Transform player;
    [SerializeField] float range = 5;
    [SerializeField] GameObject ghostRender;
    [SerializeField] PlayerController playerController;
    [SerializeField] LayerMask groundLayer;
    SpriteRenderer ghostPlacementRenderer;
    BoundingBox bounds = new BoundingBox();
    GameObject currentPrefab = null;
    GameObject instantiation = null;
    [SerializeField] Color canPlace = new Color(1f, 1f, 1f, 0.6f);
    [SerializeField] Color cantPlace = new Color(1f, 0.7f, 0.7f, 0.7f);

    private void Awake()
    {
        ghostPlacementRenderer = ghostRender.GetComponent<SpriteRenderer>();
    }

    public TileBase GetTileAtWorldPosition(Vector3 worldPosition)
    {
        // Convert the world position to the tilemap cell position
        Vector3Int cellPosition = placementMap.WorldToCell(worldPosition);

        // Retrieve the tile at the cell position
        TileBase tile = placementMap.GetTile(cellPosition);

        return tile;
    }

    Vector3 PlacementPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Vector3 placementPosition = mouseWorldPosition;
        placementPosition.z = 0;
        placementPosition -= player.position;
        float distanceToPlayer = placementPosition.magnitude;
        float distanceToPlacement = Mathf.Min(distanceToPlayer, range);
        placementPosition = distanceToPlacement * placementPosition.normalized;
        placementPosition += player.position;


        return TileObject.Round(placementPosition);
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
            ghostPlacementRenderer.color = canPlace;
            if (Input.GetMouseButton(0))
            {
                (new Item()).AddOneItem(InventoryManager.itemSelected); // Remove one item from selected slot
                PlaceObject(transform.position);
            }
        }
        else
        {
            ghostPlacementRenderer.color = cantPlace;
        }
    }

    public void CreateGhost(GameObject prefab)
    {
        if (currentPrefab == prefab) return;
        currentPrefab = prefab;
        InstantiatePrefab(currentPrefab);
        TileObject obj = instantiation.GetComponent<TileObject>();
        SetGhostSprite(obj.spriteRenderer.sprite);
        bounds = obj.GetBoundingBox();
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
        BoundingBox placementBB = new BoundingBox(bounds, position);
        bool canPlace = TileObject.BoxEmpty(placementBB)
            && placementBB.Grounded(placementMap);
        return canPlace;
    }

    public void PlaceObject(Vector3 position)
    {
        instantiation.SetActive(true);
        TileObject tileObject = instantiation.GetComponent<TileObject>();
        tileObject.Place(position);
        instantiation = null;
        InstantiatePrefab(currentPrefab);
    }
}
