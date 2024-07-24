using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileRenderer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        foreach (Vector2 pos in TileObject.objectPositions.Keys)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pos, 0.5f);
        }
    }
}
