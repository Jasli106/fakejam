using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Inventory : ICloneable
{
    public List<Item> items = new List<Item>();


    public Inventory() { }
    public Inventory(List<InventorySlot> slots)
    {
        foreach (var slot in slots)
        {
            items.Add(slot.currItem);
        }
    }

    public Dictionary<string, int> ListToDict(List<Item> list)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();

        foreach (Item item in list)
        {
            dict[item.type] = item.amount;
        }

        return dict;
    }

    public bool InsertPossible(List<Item> inserts)
    {
        Inventory clone = (Inventory)Clone();
        return clone.InsertItems(inserts);
    }

    public int InsertItem(Item insert)
    {
        foreach (Item slot in items)
        {
            slot.AddItems(insert);
            if (insert.amount == 0)
            {
                return 0;
            }
        }
        return insert.amount;
    }

    public bool InsertItems(List<Item> inserts)
    {
        bool success = true;
        foreach (Item insert in inserts)
        {
            if (InsertItem(insert) != 0)
            {
                success = false;
            }
        }
        return success;
    }

    public bool ContainsItems(List<Item> reqs)
    {
        Dictionary<string, int> inventoryDict = ListToDict(items);
        Dictionary<string, int> requirementsDict = ListToDict(reqs);

        foreach (var req in requirementsDict)
        {
            if (!inventoryDict.ContainsKey(req.Key) || inventoryDict[req.Key] < req.Value)
            {
                return false;
            }
        }

        return true;
    }

    public void RemoveItems(List<Item> reqs)
    {
        Dictionary<string, int> requirements = ListToDict(reqs);
        for (int i = items.Count; i >= 0; i--)
        {
            if (!requirements.ContainsKey(items[i].type)) {
                continue;
            }
            int itemsRemoved = Mathf.Min(requirements[items[i].type], items[i].amount);
            requirements[items[i].type] -= itemsRemoved;
            items[i].amount -= itemsRemoved;
        }
    }

    public object Clone()
    {
        Inventory clone = new Inventory();
        foreach (Item item in items)
        {
            clone.items.Add((Item)item.Clone());
        }
        return clone;
    }
}
