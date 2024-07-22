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
    DisplayItemHolder itemHolder;
    [SerializeField] Image itemDisplay;
    [SerializeField] TextMeshProUGUI quantityDisplay;

    private void Awake()
    {
        itemHolder = GetComponentInParent<DisplayItemHolder>();
    }

    public void Display()
    {
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
