using System.Collections;
using System.Collections.Generic;
using System.IO;
using TriInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "ScriptableObjects/ItemDB", order = 1)]
[ExecuteInEditMode]
public class ItemDB : ScriptableObject
{
    public List<Item> items;

    [Button(ButtonSizes.Large)]
    public void LoadItemsFromFolder()
    {
        items.Clear();

        string folderPath = "Assets/Resources/Items";
        string[] filePaths = Directory.GetFiles(folderPath);

        foreach (string filePath in filePaths)
        {
            if (!Path.GetExtension(filePath).Equals(".png")) continue;
            string itemName = Path.GetFileNameWithoutExtension(filePath);
            Item newItem = new Item(itemName, 1);
            items.Add(newItem);
        }

        Debug.Log("Items created from folder.");
        EditorUtility.SetDirty(this); // Mark the object as dirty to save the changes
    }
}
