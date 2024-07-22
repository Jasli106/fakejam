using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSourceManager : MonoBehaviour
{
/*
    private static List<InventorySlot> playerInventorySlots = new List<InventorySlot>();
    private static List<InventorySlot> hotbarInventorySlots;

    private void Start()
    {
        InventoryManager.instance.expandedInventory.GetComponentsInChildren<InventorySlot>(playerInventorySlots);
        hotbarInventorySlots = InventoryManager.instance.hotbarSlots;
    }

    public static InventorySlot FindAvailableInventorySlot(string itemName = null)
    {
        if(itemName != null)
        {
            foreach (InventorySlot slot in hotbarInventorySlots)
            {
                if(slot.item != null)
                {
                    if (slot.currItem.type == itemName && slot.currItem.amount < Item.maxAmount)
                    {
                        return slot;
                    }
                }
            }
            foreach (InventorySlot slot in playerInventorySlots)
            {
                if (slot.item != null)
                {
                    if (slot.currItem.type == itemName && slot.currItem.amount < Item.maxAmount)
                    {
                        return slot;
                    }
                }
            }
        }
        
        foreach (InventorySlot slot in hotbarInventorySlots)
        {
            if (slot.item == null)
            {
                return slot;
            }
        }
        foreach (InventorySlot slot in playerInventorySlots)
        {
            if (slot.item == null)
            {
                return slot;
            }
        }
        return null;
    }
*/
}
