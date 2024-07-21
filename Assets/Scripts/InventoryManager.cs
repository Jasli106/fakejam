using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static Item itemSelected = null;
    public static Item itemPickedUp = null;
    public static bool inventoryOpen = false;

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
    }

    // Hotbar
    void SelectSlot(int idx)
    {
        DeselectSlot(selectedSlotIndex);
        itemSelected = hotbarSlots[idx].currItem;
        selectedSlotIndex = idx;

        // Highlight
        Image img = hotbarSlots[idx].GetComponent<Image>();
        img.color = img.color = img.color = new Color(0.8f, 0.8f, 0.8f);
    }

    void DeselectSlot(int idx)
    {
        Image img = hotbarSlots[idx].GetComponent<Image>();
        img.color = img.color = img.color = new Color(1, 1, 1);
    }


}
