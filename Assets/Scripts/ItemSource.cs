using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSource : TileObject
{
    public string resourceName;
    public float delay = 1f;

    private float timer = 0;

    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        timer = 0;
    }

    public override void ClickHeld(MouseInteractor mouse)
    {
        Inventory playerInventory = mouse.GetComponentInParent<PlayerInventory>().GetInventory();

        timer += Time.deltaTime;

        if(timer >= delay)
        {
            CollectItems(playerInventory);
            timer = 0;
        }
    }

    public void CollectItems(Inventory inventory)
    {
        inventory.InsertItem(new Item(resourceName, 1));
    }
}
