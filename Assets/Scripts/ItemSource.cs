using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSource : TileObject
{
    public string resourceName;

    public override void ClickDown()
    {
        CollectItems();
    }

    public void CollectItems()
    {
        InventorySlot nextAvailableSlot = ItemSourceManager.FindAvailableInventorySlot(resourceName);
        if(nextAvailableSlot != null)
        {
            if (nextAvailableSlot.currItem != null)
            {
                nextAvailableSlot.currItem.AddItems(1);
            }
            else
            {
                nextAvailableSlot.currItem = new Item(resourceName, 1);
            }
        }
    }
}
