using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public static string itemFolder = "Items/";
    public string name = "Unnamed Item";
    public int amount = 1;

    public Item(string name, int amount)
    {
        this.name = name;
        this.amount = amount;
    }

    public Sprite GetSprite(string name)
    {
        return Resources.Load<Sprite>(itemFolder + name + ".png");
    }
}