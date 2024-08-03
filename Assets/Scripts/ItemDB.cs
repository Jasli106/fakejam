using System.Collections.Generic;
using TriInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "ScriptableObjects/ItemDB", order = 1)]
public class ItemDB : ScriptableObject
{
    public List<Item> items;

    [Button(ButtonSizes.Large)]
    public void LoadItemsFromFolder()
    {
        items.Clear();

        // Load all textures in the Resources/Items folder
        Object[] loadedItems = Resources.LoadAll("Items", typeof(Texture2D));

        foreach (Object obj in loadedItems)
        {
            Texture2D texture = obj as Texture2D;
            if (texture == null) continue;

            string itemName = texture.name;
            Item newItem = new Item(itemName, 1); // Assuming itemName and quantity 1
            items.Add(newItem);
        }

        Debug.Log("Items created from folder.");
        // Mark the object as dirty to save the changes
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}