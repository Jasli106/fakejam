using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOnlyTileUI : TileUI
{
    public InventoryUIController inventory;
    public override void OpenTileUI(TileObject tileObject)
    {
        if (tileObject is InputInventory i)
        {
            inventory.RepresentInventory(i.GetInputInventory());
            inventory.SetOutputOnly(false);
        }
        else if (tileObject is OutputInventory o)
        {
            inventory.RepresentInventory(o.GetOutputInventory());
            inventory.SetOutputOnly(true);
        }
        else
        {
            Debug.Log("Opened tile for MachineUI must be Machine");
        }
        gameObject.SetActive(true);
    }
}