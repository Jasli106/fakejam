using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static Item itemSelected = null;
    public static Item itemPickedUp = null;
    public static bool inventoryOpen = false;

    public GameObject expandedInventory;
    public Image inventoryItemImg;
    public Canvas canvas;

    [SerializeField] private List<InventorySlot> hotbarSlots;
    private int selectedSlotIndex = 0;

    private void Start()
    {
        SelectSlot(0);
    }

    // Render item selected image at mouse point
    private void Update()
    {
        if(itemPickedUp != null)
        {
            inventoryItemImg.sprite = itemPickedUp.sprite;
            inventoryItemImg.color = new Color(1, 1, 1, 1);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint);

            inventoryItemImg.rectTransform.localPosition = localPoint;
        } else
        {
            inventoryItemImg.color = new Color(1, 1, 1, 0);
        }

        for(int i=0; i<9; ++i)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
        HandleScrollInput();
    }
    void HandleScrollInput()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        int newIndex = selectedSlotIndex + (int)Mathf.Sign(scrollDelta);
        newIndex += hotbarSlots.Count();
        newIndex %= hotbarSlots.Count();
        SelectSlot(newIndex);
    }


    // Hotbar
    void SelectSlot(int idx)
    {
        DeselectSlot(selectedSlotIndex);
        itemSelected = hotbarSlots[idx].currItem;
        hotbarSlots[idx].selected = true;
        selectedSlotIndex = idx;

        // Highlight
        Image img = hotbarSlots[idx].GetComponent<Image>();
        img.color = img.color = img.color = new Color(0.8f, 0.8f, 0.8f);
    }

    void DeselectSlot(int idx)
    {
        hotbarSlots[idx].selected = false;
        Image img = hotbarSlots[idx].GetComponent<Image>();
        img.color = img.color = img.color = new Color(1, 1, 1);
    }

    // Open/close inventory
    void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        expandedInventory.SetActive(inventoryOpen);
    }

}
