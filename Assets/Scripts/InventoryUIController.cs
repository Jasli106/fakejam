using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    InventorySlot[] slots;

    public void RepresentInventory(Inventory other)
    {
        if (slots == null) slots = GetComponentsInChildren<InventorySlot>();
        if (slots.Length != other.Slots())
        {
            Debug.LogError("Inventory display does not have the right number of slots");
        }
        for (int slot = 0; slot < slots.Length; slot++)
        {
            slots[slot].item = other.items[slot];
        }
    }
}
