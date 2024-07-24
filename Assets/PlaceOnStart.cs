using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileObject))]
public class PlaceOnStart : MonoBehaviour
{
    TileObject tile;
    void Start()
    {
        tile = GetComponent<TileObject>();
        tile.Place(transform.position);
    }
}
