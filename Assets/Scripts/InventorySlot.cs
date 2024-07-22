using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Item currItem = null;
    [HideInInspector] public bool selected = false;

    [SerializeField] Image itemDisplay;
    [SerializeField] TextMeshProUGUI quantityDisplay;
    [SerializeField] bool outputOnly = false;
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        if(currItem == null)
        {
            itemDisplay.color = new Color(1, 1, 1, 0);
        }
    }

    void Update()
    {
        if (currItem == null)
        {
            itemDisplay.color = new Color(1, 1, 1, 0);
            quantityDisplay.text = "";
        } else
        {
            itemDisplay.color = new Color(1, 1, 1, 1);
            itemDisplay.sprite = currItem.sprite;
            quantityDisplay.text = currItem.amount.ToString();
        }
    }

    // Controls
    /*
     * Left-click: pick up/put down stack
     * Right-click: pick up/put down 1
    */

    public void OnPointerDown(PointerEventData eventData)
    {
        if(InventoryManager.inventoryOpen)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                img.color = new Color(0.7f, 0.7f, 0.7f);

                if (outputOnly) {
                    if (currItem == null || (InventoryManager.itemPickedUp != null && currItem.type != InventoryManager.itemPickedUp.type))
                    {
                        return;
                    } else
                    {
                        int remainder = InventoryManager.itemPickedUp.AddItems(currItem.amount);
                        currItem.amount = remainder;
                        if(remainder <= 0)
                        {
                            currItem = null;
                        }
                    }
                }

                
                if(currItem != null && InventoryManager.itemPickedUp != null && currItem.type == InventoryManager.itemPickedUp.type) // Consolidate items
                {
                    int remainder = currItem.AddItems(InventoryManager.itemPickedUp.amount);
                    InventoryManager.itemPickedUp.amount = remainder;
                    if (remainder <= 0)
                    {
                        InventoryManager.itemPickedUp = null;
                    }
                }
                else // Swap items
                {
                    Item tempItem = currItem;
                    currItem = InventoryManager.itemPickedUp;
                    InventoryManager.itemPickedUp = tempItem;
                }
                
                
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
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                img.color = new Color(0.7f, 0.7f, 0.7f);

                if (InventoryManager.itemPickedUp == null)
                {
                    // Pick up one
                    InventoryManager.itemPickedUp = new Item(currItem.type, 1);
                    currItem.RemoveItem();
                    if (currItem.amount == 0)
                    {
                        currItem = null;
                        itemDisplay.color = new Color(1, 1, 1, 0);
                    }
                }
                else
                {
                    if (outputOnly) return;
                    if (InventoryManager.itemPickedUp.type == currItem.type) // Same item in slot
                    {
                        // Try to put down one
                        int remainder = currItem.AddItems(1);
                        if (remainder <= 0)
                        {
                            InventoryManager.itemPickedUp.RemoveItem();
                            if (InventoryManager.itemPickedUp.amount == 0)
                            {
                                InventoryManager.itemPickedUp = null;
                            }
                        }
                    }
                    else if (currItem == null) // No item in slot
                    {
                        // Put down one
                        currItem = new Item(InventoryManager.itemPickedUp.type, 1);
                        InventoryManager.itemPickedUp.RemoveItem();
                        if (InventoryManager.itemPickedUp.amount == 0)
                        {
                            InventoryManager.itemPickedUp = null;
                        }
                    }
                    else
                    {
                        // Swap items
                        Item tempItem = currItem;
                        currItem = InventoryManager.itemPickedUp;
                        InventoryManager.itemPickedUp = tempItem;
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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(InventoryManager.inventoryOpen)
        {
            if(selected)
            {
                img.color = new Color(0.8f, 0.8f, 0.8f);
            } else
            {
                img.color = new Color(1, 1, 1);
            }
            
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InventoryManager.inventoryOpen)
        {
            img.color = new Color(0.8f, 0.8f, 0.8f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventoryManager.inventoryOpen)
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
    }
}
