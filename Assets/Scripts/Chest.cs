using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : EnableableTileObject, InputInventory, OutputInventory
{

    Inventory inventory = new Inventory(27);

    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        if (!firstClick) return;
        InventoryManager.instance.OpenInventoryOnlyTileUI(this);
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
