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
        return $"{amount} {type}";
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
        return amount == 0;
    }

    private int AddItems(int quantity)
    {
        int amountAdded = Mathf.Min(maxAmount - amount, quantity);
        amount += amountAdded;
        int remainder = quantity - amountAdded;
        return remainder;
    }

    public bool AddItems(Item other)
    {
        if (other.Empty()) return false;
        if (!Empty() && other.type != type) return false;
        if (Empty())
        {
            SetType(other.type);
        }
        other.amount = AddItems(other.amount);
        return true;
    }

    public void Swap(Item other)
    {
        Item temp = (Item)other.Clone();
        other.type = type;
        other.amount = amount;
        this.type = temp.type;
        this.amount = temp.amount;
    }

    public bool AddOneItem(Item other)
    {
        if (other.Empty()) return false;
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