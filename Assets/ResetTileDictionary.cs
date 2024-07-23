using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTileDictionary : MonoBehaviour
{
    void Start()
    {
        TileObject.objectPositions = new Dictionary<Vector2, TileObject>();
    }
}
