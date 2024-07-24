using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovementDisabler : MonoBehaviour
{
    [SerializeField] PlayerController pc;
    [SerializeField] GameObject[] disable;

    bool inventoryPreviouslyOpen = false;
    private void Update()
    {
        bool inInventory = InventoryManager.inventoryOpen;
        bool inventoryOpened = inInventory && !inventoryPreviouslyOpen;
        bool inventoryClosed = !inInventory && inventoryPreviouslyOpen;
        inventoryPreviouslyOpen = inInventory;

        if (inventoryOpened)
        {
            SetMovementEnabled(false);
        }
        if (inventoryClosed)
        {
            SetMovementEnabled(true);
        }
    }

    public void SetMovementEnabled(bool value)
    {
        pc.movementEnabled = value;
        foreach (var obj in disable)
        {
            obj.SetActive(value);
        }
    }
}
