using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorItemManager : MonoBehaviour
{
    public static FloorItemManager instance = null;
    [SerializeField] GameObject floorItemPrefab;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    public static GameObject Prefab()
    {
        return instance.floorItemPrefab;
    }
}
