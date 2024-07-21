using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static Item itemSelected = null;
    public Image inventoryItemImg;
    public Canvas canvas;

    // Render item selected image at mouse point
    private void Update()
    {
        if(itemSelected != null)
        {
            inventoryItemImg.sprite = itemSelected.sprite;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint);

            inventoryItemImg.rectTransform.localPosition = localPoint;
        } else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
               canvas.transform as RectTransform,
               Input.mousePosition,
               canvas.worldCamera,
               out Vector2 localPoint);

            inventoryItemImg.rectTransform.localPosition = localPoint;
        }
    }
}
