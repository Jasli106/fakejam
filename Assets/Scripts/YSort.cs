using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSort : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    public static readonly int granularity = 100;
    [SerializeField] int offset = 0;

    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        foreach (var renderer in spriteRenderers)
        {
            renderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -granularity + offset);
        }
    }
}
