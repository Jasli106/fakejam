using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Item currItem = null;

    [SerializeField] Image itemDisplay;
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        if(currItem == null)
        {
            itemDisplay.color = new Color(1, 1, 1, 0);
        }
    }

    // Controls
    /*
     * Left-click: pick up/put down stack
     * Right-click: pick up/put down 1
    */

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            img.color = new Color(0.7f, 0.7f, 0.7f);

            // Swap items
            Item tempItem = currItem;
            currItem = InventoryManager.itemSelected;
            InventoryManager.itemSelected = tempItem;
            if(currItem == null)
            {
                itemDisplay.color = new Color(1, 1, 1, 0);
            } else
            {
                itemDisplay.sprite = currItem.sprite;
                itemDisplay.color = new Color(1, 1, 1, 1);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            img.color = new Color(0.7f, 0.7f, 0.7f);

            if(InventoryManager.itemSelected == null)
            {
                // Pick up one
                InventoryManager.itemSelected = new Item(currItem.name, 1);
                currItem.RemoveItem();
                if(currItem.amount == 0)
                {
                    currItem = null;
                    itemDisplay.color = new Color(1, 1, 1, 0);
                }
            } else
            {
                if(InventoryManager.itemSelected.name == currItem.name) // Same item in slot
                {
                    // Try to put down one
                    int remainder = currItem.AddItems(1);
                    if(remainder <= 0)
                    {
                        InventoryManager.itemSelected.RemoveItem();
                        if (InventoryManager.itemSelected.amount == 0)
                        {
                            InventoryManager.itemSelected = null;
                        }
                    }
                }
                else if(currItem == null) // No item in slot
                {
                    // Put down one
                    currItem = new Item(InventoryManager.itemSelected.name, 1);
                    InventoryManager.itemSelected.RemoveItem();
                    if (InventoryManager.itemSelected.amount == 0)
                    {
                        InventoryManager.itemSelected = null;
                    }
                }
                else
                {
                    // Swap items
                    Item tempItem = currItem;
                    currItem = InventoryManager.itemSelected;
                    InventoryManager.itemSelected = tempItem;
                    if (currItem == null)
                    {
                        itemDisplay.color = new Color(1, 1, 1, 0);
                    }
                    else
                    {
                        itemDisplay.sprite = currItem.sprite;
                        itemDisplay.color = new Color(1, 1, 1, 1);
                    }
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        img.color = new Color(1, 1, 1);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        img.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = new Color(1, 1, 1);
    }
}
