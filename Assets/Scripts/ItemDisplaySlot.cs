using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemDisplaySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Item item = new Item();
    [HideInInspector] public bool selected = false;

    [SerializeField] Image img;
    [SerializeField] Image itemDisplay;
    [SerializeField] TextMeshProUGUI quantityDisplay = null;

    public void LeftClick() // Also for r
    {
        // Show recipes to create item
        if (item.Empty()) return;
        List<Recipe> recipes = Recipe.ItemOutRecipes(item.type);
        RecipeBook.instance.DisplayRecipes(recipes);

    }

    public void RightClick() // Also for u
    {
        // Show recipes that use item
        if (item.Empty()) return;
        List<Recipe> recipes = Recipe.ItemInRecipes(item.type);
        RecipeBook.instance.DisplayRecipes(recipes);
    }

    private void Update()
    {
        if(selected)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                LeftClick();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                RightClick();
            }
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
        img.color = new Color(0.8f, 0.8f, 0.8f);
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = new Color(1, 1, 1);
        selected = false;
    }

    public Item Display()
    {
        itemDisplay.sprite = item.LoadSprite();
        itemDisplay.color = new Color(1, 1, 1, 1);
        if(quantityDisplay != null)
        {
            quantityDisplay.text = item.amount.ToString();
        }
        return item;
    }
}
