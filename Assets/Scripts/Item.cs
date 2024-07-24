using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : ICloneable
{
    public static string itemFolder = "Items/";
    public static string placementFolder = "Placeable/";
    public static int maxAmount = 99;

    public string type = "";
    public int amount = 0;
    [HideInInspector] public bool locked = false;

    public Item() { }

    public Item(string type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }

    public override string ToString()
    {
        if (Empty())
        {
            return "Empty";
        }
        return $"{amount} {type}" + (locked ? "(locked)" : "");
    }

    public Sprite LoadSprite()
    {
        //NOTE PROBABLY HORRIBLY INEFFICIENT COMPUTATIONALLY
        if (Empty())
        {
            Debug.LogError("Loading a sprite from an empty item");
        }
        return Resources.Load<Sprite>(itemFolder + type);
    }

    public void SetType(string type)
    {
        this.type = type;
    }

    public bool Empty()
    {
        return (!locked && amount == 0) || (locked && type == "");
    }

    private int AddItems(int quantity)
    {
        if (locked && type == "") return quantity;
        int amountAdded = Mathf.Min(maxAmount - amount, quantity);
        amount += amountAdded;
        int remainder = quantity - amountAdded;
        return remainder;
    }

    public bool AddItems(Item other)
    {
        if (other.Empty()) return false;
        if (!Empty() && other.type != type) return false;
        if (Empty() && !locked)
        {
            SetType(other.type);
        }
        other.amount = AddItems(other.amount);
        return true;
    }

    public void Swap(Item other)
    {
        if (locked && !other.Empty()) return;
        Item temp = (Item)other.Clone();
        other.type = type;
        other.amount = amount;
        if (!locked) this.type = temp.type;
        this.amount = temp.amount;
    }

    public bool AddOneItem(Item other)
    {
        if (other.amount == 0) return false;
        if (!Empty() && other.type != type) return false;
        if (Empty())
        {
            SetType(other.type);
        }
        other.amount = other.amount - 1 + AddItems(1);
        return true;
    }

    public object Clone()
    {
        return new Item(this.type, this.amount);
    }

    public GameObject Placement() 
    {
        if (Empty()) return null;
        return Resources.Load<GameObject>(placementFolder + type);
    }
}