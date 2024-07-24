using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : TileObject, InputInventory, OutputInventory
{
    [SerializeField] Sprite closed;
    [SerializeField] Sprite open;
    [SerializeField] SpriteRenderer sr;

    Inventory inventory = new Inventory(27);

    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        if (!firstClick) return;
        InventoryManager.instance.OpenChestInventory(inventory, this);
    }

    public void SetSpriteOpen(bool isOpen)
    {
        if (isOpen) sr.sprite = open;
        else sr.sprite = closed;
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
