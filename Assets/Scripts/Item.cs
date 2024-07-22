using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ICloneable
{
    public static string itemFolder = "Items/";
    public static int maxAmount = 99;

    public string type = "";
    public int amount = 0;

    public Sprite sprite;

    public Item(string type, int amount)
    {
        this.type = type;
        this.amount = amount;
        this.sprite = GetSprite();
    }

    public Sprite GetSprite()
    {
        if (amount == 0) return null;
        sprite = Resources.Load<Sprite>(itemFolder + type);
        return sprite;
    }

    public int AddItems(int quantity)
    {
        int amountAdded = Mathf.Min(maxAmount - amount, quantity);
        amount += amountAdded;
        int remainder = quantity - amountAdded;
        return remainder;
    }

    public void AddItems(Item other)
    {
        if (amount == 0)
        {
            type = other.type;
        }
        other.amount = AddItems(other.amount);
    }

    public bool RemoveItem()
    {
        if (amount > 0)
        {
            amount--;
            return true;
        }
        return false;
    }

    public object Clone()
    {
        return new Item(this.type, this.amount);
    }
}