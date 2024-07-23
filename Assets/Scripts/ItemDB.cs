using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "ScriptableObjects/ItemDB", order = 1)]
public class ItemDB : ScriptableObject
{
    public List<Item> items;
}
