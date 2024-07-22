using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public static string itemFolder = "Items/";
    public static int maxAmount = 99;

    public string name = "Unnamed Item";
    public int amount = 1;

    public Sprite sprite;

    public Item(string name, int amount)
    {
        this.name = name;
        this.amount = amount;
        this.sprite = GetSprite();
    }

    public Sprite GetSprite()
    {
        sprite = Resources.Load<Sprite>(itemFolder + name);
        return sprite;
    }

    public int AddItems(int quantity)
    {
        int amountAdded = Mathf.Min(maxAmount - amount, quantity);
        amount += amountAdded;
        int remainder = quantity - amountAdded;
        return remainder;
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
    
    public static bool operator ==(Item item1, Item item2)
    {
        if (ReferenceEquals(item1, item2))
        {
            return true;
        }

        if ((object)item1 == null || (object)item2 == null)
        {
            return false;
        }

        return item1.name == item2.name;
    }

    public static bool operator !=(Item item1, Item item2)
    {
        return !(item1 == item2);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Item item = (Item)obj;
        return name == item.name;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}