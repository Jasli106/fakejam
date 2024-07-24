using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : TileObject, InputInventory, OutputInventory
{

    Inventory inventory = new Inventory(27);
    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        InventoryManager.instance.OpenChestInventory(inventory);
    }

    public Inventory GetInputInventory()
    {
        return inventory;
    }

    public Inventory GetOutputInventory()
    {
        return inventory;
    }
}
