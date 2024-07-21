using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public abstract class Inventory
{
    public List<Item> items;

    public Dictionary<string, int> ListToDict(List<Item> list)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();

        foreach (Item item in list)
        {
            dict[item.name] = item.amount;
        }

        return dict;
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
            if (!requirements.ContainsKey(items[i].name)) {
                continue;
            }
            int itemsRemoved = Mathf.Min(requirements[items[i].name], items[i].amount);
            requirements[items[i].name] -= itemsRemoved;
            items[i].amount -= itemsRemoved;
        }
    }
}
