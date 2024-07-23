using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private ItemDB itemDB;

    // Start is called before the first frame update
    void Start()
    {
        PopulateItemDisplay(itemDB.items);
    }

    public void PopulateItemDisplay(List<Item> items)
    {
        int numberOfRows = Mathf.CeilToInt((float)items.Count / 9);
        numberOfRows = Mathf.Max(numberOfRows, 3);

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        int itemIndex = 0;
        for (int rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
        {
            GameObject newRow = Instantiate(rowPrefab, content);

            ItemDisplaySlot[] slots = newRow.GetComponentsInChildren<ItemDisplaySlot>();

            for (int slotIndex = 0; slotIndex < 9; slotIndex++)
            {
                if (itemIndex < items.Count)
                {
                    slots[slotIndex].item = (items[itemIndex]);
                    slots[slotIndex].Display();
                    itemIndex++;
                }
            }
        }
    }
}
