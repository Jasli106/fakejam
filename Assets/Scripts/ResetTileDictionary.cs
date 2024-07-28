using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTileDictionary : MonoBehaviour
{
    void Awake()
    {
        TileObject.objectPositions = new Dictionary<Vector2, TileObject>();
    }
}
