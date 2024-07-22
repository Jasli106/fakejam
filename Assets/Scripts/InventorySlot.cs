using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Item item = new Item();
    [HideInInspector] public bool selected = false;

    [SerializeField] Image img;
    [SerializeField] ItemDisplayer itemDisplay;
    [SerializeField] bool outputOnly = false;


    public void SetItem(Item item)
    {
        this.item = item;
        itemDisplay.item = item;
    }

    public Item GetItem()
    {
        return item;
    }

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
            if (!InventoryManager.instance.HeldItem.Empty() && item.type != InventoryManager.instance.HeldItem.type) return;
            InventoryManager.instance.HeldItem.AddItems(item);
            return;
        }


        bool consolidated = item.AddItems(InventoryManager.instance.HeldItem);
        if (!consolidated) //Swap
        {
            SwapWithHeldItem();
        }
    }

    public void SwapWithHeldItem()
    {
        Item tempItem = item;
        SetItem(InventoryManager.instance.HeldItem);
        InventoryManager.instance.HeldItem = tempItem;
    }
    public void RightClick()
    {
        if (InventoryManager.instance.HeldItem.Empty())
        {
            // Pick up one
            InventoryManager.instance.HeldItem.AddOneItem(item);
            return;
        }

        if (outputOnly) return;

        bool consolidated = item.AddOneItem(InventoryManager.instance.HeldItem);
        if (!consolidated)
        {
            SwapWithHeldItem();
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
        if (!InventoryManager.inventoryOpen) return;
        img.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
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
}
