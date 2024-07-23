using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorItem : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] SpriteRenderer spriteRenderer;

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.LoadSprite();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInventory>().GetInventory().InsertItem(item, true);

            if (item.Empty())
            {
                Destroy(gameObject);
            }
        }
    }

}
