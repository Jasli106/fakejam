using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSource : TileObject
{
    public string resourceName;

    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        Inventory playerInventory = mouse.GetComponentInParent<PlayerInventory>().GetInventory();
        CollectItems(playerInventory);
    }

    public void CollectItems(Inventory inventory)
    {
        inventory.InsertItem(new Item(resourceName, 1));
    }
}
