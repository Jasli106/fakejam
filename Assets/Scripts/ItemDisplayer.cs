using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public interface DisplayItemHolder
{
    public Item DisplayItem();
}

public class ItemDisplayer : MonoBehaviour
{
    DisplayItemHolder itemHolder = null;
    [SerializeField] Image itemDisplay;
    [SerializeField] TextMeshProUGUI quantityDisplay;

    public void Display()
    {
        if (itemHolder == null) {
            itemHolder = GetComponentInParent<DisplayItemHolder>();
        }
        if (itemHolder.DisplayItem().Empty())
        {
            itemDisplay.color = new Color(1, 1, 1, 0);
            quantityDisplay.text = "";
        }
        else
        {
            itemDisplay.color = new Color(1, 1, 1, 1);
            itemDisplay.sprite = itemHolder.DisplayItem().LoadSprite();
            quantityDisplay.text = itemHolder.DisplayItem().amount.ToString();
        }
    }

    private void Update()
    {
        Display();
    }
}
