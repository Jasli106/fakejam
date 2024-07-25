using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Inventory : ICloneable
{
    public List<Item> items = new List<Item>();

    public Inventory() { }

    public Inventory(int slots)
    {
        for (int i = 0; i < slots; i++)
        {
            items.Add(new Item());
        }
    }

    public Inventory(List<InventorySlot> slots)
    {
        foreach (var slot in slots)
        {
            items.Add(slot.item);
        }
    }

    public Inventory(Inventory i1, Inventory i2)
    {
        foreach (var item in i1.items)
        {
            items.Add(item);
        }
        foreach (var item in i2.items)
        {
            items.Add(item);
        }
    }

    public Inventory(List<Inventory> inventories)
    {
        foreach (var inv in inventories)
        {
            foreach (var item in inv.items)
            {
                items.Add(item);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var item in items)
        {
            sb.AppendLine(item.ToString());
        }

        return sb.ToString();
    }

    public int Slots()
    {
        return items.Count;
    }

    public Dictionary<string, int> ListToDict(List<Item> list)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();

        foreach (Item item in list)
        {
            if (item.Empty()) continue;
            if (dict.ContainsKey(item.type))
            {
                dict[item.type] += item.amount;
            }
            else
            {
                dict[item.type] = item.amount;
            }
        }

        return dict;
    }

    public bool InsertPossible(List<Item> inserts)
    {
        Inventory clone = (Inventory)Clone();
        foreach (Item insert in inserts)
        {
            if (clone.InsertItem(insert) != 0)
            {
                return false;
            }
        }
        return true;
    }

    public int InsertItem(Item insert, bool useItem = false)
    {
        foreach (Item slot in items)
        {
            if (!useItem)
            {
                insert = (Item)insert.Clone();
            }
            slot.AddItems(insert);
            if (insert.Empty())
            {
                return 0;
            }
        }
        return insert.amount;
    }

    public void InsertItems(List<Item> inserts, bool useItem = false)
    {
        foreach (Item insert in inserts)
        {
            InsertItem(insert, useItem);
        }
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
        for (int i = items.Count - 1; i >= 0; i--)
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
