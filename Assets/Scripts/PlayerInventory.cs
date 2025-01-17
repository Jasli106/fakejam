using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] InventoryUIController hotbarUIController;
    [SerializeField] InventoryUIController hiddenUIController;

    [SerializeField] List<Item> startingItems = new List<Item>();

    Inventory hotbar;
    Inventory hidden;
    Inventory total;

    private void Start()
    {
        hotbar = new Inventory(9);
        hidden = new Inventory(27);
        total = new Inventory(hotbar, hidden);
        hotbarUIController.RepresentInventory(hotbar);
        hiddenUIController.RepresentInventory(hidden);

        total.InsertItems(startingItems);
    }

    public Inventory GetInventory()
    {
        return total;
    }
}
