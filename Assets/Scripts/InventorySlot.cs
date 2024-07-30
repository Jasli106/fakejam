using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, DisplayItemHolder
{
    public Item item = new Item();
    [HideInInspector] public bool selected = false;

    [SerializeField] Image img;
    [SerializeField] ItemDisplayer itemDisplay;
    [SerializeField] List<GameObject> locked;
    public bool outputOnly = false;

    private bool hovered = false;


    // Controls
    /*
     * Left-click: pick up/put down stack
     * Right-click: pick up/put down 1
    */

    public void LeftClick()
    {
        if (outputOnly)
        {
            if (item.Empty()) return;
            if (!InventoryManager.instance.itemPickedUp.Empty() && item.type != InventoryManager.instance.itemPickedUp.type) return;
            InventoryManager.instance.itemPickedUp.AddItems(item);
            return;
        }

        bool consolidated = item.AddItems(InventoryManager.instance.itemPickedUp);
        if (!consolidated) //Swap
        {
            item.Swap(InventoryManager.instance.itemPickedUp);
        }
    }

    public void RightClick()
    {
        if (InventoryManager.instance.itemPickedUp.Empty())
        {
            // Pick up one
            InventoryManager.instance.itemPickedUp.AddOneItem(item);
            return;
        }

        if (outputOnly) return;

        bool consolidated = item.AddOneItem(InventoryManager.instance.itemPickedUp);
        if (!consolidated)
        {
            item.Swap(InventoryManager.instance.itemPickedUp);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!InventoryManager.inventoryOpen) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            img.color = new Color(0.7f, 0.7f, 0.7f);
            LeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            img.color = new Color(0.7f, 0.7f, 0.7f);
            RightClick();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!InventoryManager.inventoryOpen) return;
        if (selected)
        {
            img.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            img.color = new Color(1, 1, 1);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        if (!InventoryManager.inventoryOpen) return;
        img.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        if (!InventoryManager.inventoryOpen) return;
        if (selected)
        {
            img.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            img.color = new Color(1, 1, 1);
        }
    }

    void Update()
    {
        locked.ForEach(go => go.SetActive(item.locked));
        if (hovered)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Lock item
                if (item.Empty()) item.SetType("");
                item.locked = !item.locked;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Show recipes to create item
                if (item.Empty()) return;
                List<(string, List<Recipe>)> recipes = Recipe.ItemOutRecipes(item.type);
                RecipeBook.instance.ToggleRecipeBook();
                RecipeBook.instance.DisplayRecipes(recipes);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                // Show recipes that use item
                if (item.Empty()) return;
                List<(string, List<Recipe>)> recipes = Recipe.ItemInRecipes(item.type);
                RecipeBook.instance.ToggleRecipeBook();
                RecipeBook.instance.DisplayRecipes(recipes);
            }
        }
    }

    public Item DisplayItem()
    {
        return item;
    }
}
