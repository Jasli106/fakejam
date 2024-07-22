using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : ICloneable
{
    public static string itemFolder = "Items/";
    public static int maxAmount = 99;

    public string type = "";
    public int amount = 0;

    public Item() { }

    public Item(string type, int amount)
    {
        this.type = type;
        this.amount = amount;
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
}