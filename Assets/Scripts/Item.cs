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
        sprite = Resources.Load<Sprite>(itemFolder + name + ".png");
        return sprite;
    }

    public int AddItems(int quantity)
    {
        int sum = amount + quantity;
        int remainder = 0;
        if(sum > maxAmount)
        {
            remainder = sum - maxAmount;
            amount = maxAmount;
        } else
        {
            amount = sum;
        }
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
}