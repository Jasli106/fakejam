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
    [SerializeField]
    private Sprite closedBook;
    [SerializeField]
    private Sprite openBook;
    [SerializeField]
    private Image bookButton;
    [SerializeField]
    private GameObject inventory;

    public static RecipeBook instance = null;

    private List<(string, List<Recipe>)> currDisplayedRecipes = new List<(string, List<Recipe>)> ();
    private int currMachineIdx = 0;
    private int currRecipeIdx = 0;

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

    public void DisplayRecipes(List<(string, List<Recipe>)> recipes)
    {
        currDisplayedRecipes = recipes;
        // Put in UI
        if (recipes.Count <= 0) return;
        DisplayRecipe(recipes[0].Item2[0]);
        currRecipeIdx = 0;

    }

    private void DisplayRecipe(Recipe recipe)
    {
        // Clear inputs and outputs
        foreach(var slot in inputSlots)
        {
            slot.ClearDisplay();
        }
        foreach (var slot in outputSlots)
        {
            slot.ClearDisplay();
        }

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

    public void DisplayNextRecipe()
    {
        if (currDisplayedRecipes.Count <= 0) return;
        currRecipeIdx++;
        if(currRecipeIdx >= currDisplayedRecipes[currMachineIdx].Item2.Count)
        {
            currMachineIdx++;
            if (currMachineIdx >= currDisplayedRecipes.Count)
            {
                currMachineIdx = 0;
            }
            currRecipeIdx = 0;
        }
        DisplayRecipe(currDisplayedRecipes[currMachineIdx].Item2[currRecipeIdx]);
    }

    public void DisplayPreviousRecipe()
    {
        if (currDisplayedRecipes.Count <= 0) return;
        currRecipeIdx--;
        if (currRecipeIdx <= 0)
        {
            currMachineIdx--;
            if (currMachineIdx <= 0)
            {
                currMachineIdx = currDisplayedRecipes.Count - 1;
            }
            currRecipeIdx = currDisplayedRecipes[currMachineIdx].Item2.Count - 1;
        }
        DisplayRecipe(currDisplayedRecipes[currMachineIdx].Item2[currRecipeIdx]);
    }

    public void DisplayNextMachine()
    {
        if (currDisplayedRecipes.Count <= 0) return;
        currMachineIdx++;
        if (currMachineIdx >= currDisplayedRecipes.Count)
        {
            currMachineIdx = 0;
        }
        currRecipeIdx = 0;
        DisplayRecipe(currDisplayedRecipes[currMachineIdx].Item2[currRecipeIdx]);
    }

    public void DisplayPreviousMachine()
    {
        if (currDisplayedRecipes.Count <= 0) return;
        currMachineIdx--;
        if (currMachineIdx <= 0)
        {
            currMachineIdx = currDisplayedRecipes.Count - 1;
        }
        currRecipeIdx = currDisplayedRecipes[currMachineIdx].Item2.Count - 1;
        DisplayRecipe(currDisplayedRecipes[currMachineIdx].Item2[currRecipeIdx]);
    }

    public void ToggleRecipeBook()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        inventory.SetActive(!gameObject.activeSelf);
        if(gameObject.activeSelf)
        {
            bookButton.sprite = openBook;
        } else
        {
            bookButton.sprite = closedBook;
        }
    }
}
