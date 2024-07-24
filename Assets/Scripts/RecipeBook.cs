using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeBook : MonoBehaviour
{
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private ItemDB itemDB;
    [SerializeField]
    private List<ItemDisplaySlot> inputSlots;
    [SerializeField]
    private List<ItemDisplaySlot> outputSlots;
    [SerializeField]
    private Image machineImg;
    [SerializeField]
    private TextMeshProUGUI timeLabel;
    [SerializeField]
    private TextMeshProUGUI energyLabel;

    public static RecipeBook instance = null;

    private List<Recipe> currDisplayedRecipes = new List<Recipe>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        machineImg.preserveAspect = true;
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

    public void SearchItems(string searchText)
    {
        searchText = searchText.Replace(" ", "").ToLower();

        if (string.IsNullOrEmpty(searchText))
        {
            PopulateItemDisplay(itemDB.items);
        }
        else
        {
            List<Item> searchedItems = itemDB.items.FindAll(item => item.type.Replace(" ", "").ToLower().Contains(searchText));
            PopulateItemDisplay(searchedItems);
        }
    }

    public void DisplayRecipes(List<Recipe> recipes)
    {
        currDisplayedRecipes = recipes;
        // Put in UI
        if (recipes.Count <= 0) return;
        DisplayRecipe(recipes[0]);

    }

    private void DisplayRecipe(Recipe recipe)
    {
        // Display inputs and outputs
        int inSlotIdx = 0;
        foreach(var item in recipe.inputs)
        {
            if (inSlotIdx >= 9) return;
            inputSlots[inSlotIdx].item = item;
            inputSlots[inSlotIdx].Display();
            ++inSlotIdx;
        }
        int outSlotIdx = 0;
        foreach (var item in recipe.outputs)
        {
            if (outSlotIdx >= 9) return;
            outputSlots[outSlotIdx].item = item;
            outputSlots[outSlotIdx].Display();
        }

        // Display machine
        machineImg.sprite = Resources.Load<Sprite>(Item.itemFolder + recipe.machine);

        // Display other info
        timeLabel.text = recipe.time.ToString();
        energyLabel.text = recipe.costs.ToString();
    }



}
