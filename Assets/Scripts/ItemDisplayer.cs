using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemDisplayer : MonoBehaviour
{
    public Item item = new Item();
    [SerializeField] Image itemDisplay;
    [SerializeField] TextMeshProUGUI quantityDisplay;

    public void Display()
    {
        if (item.Empty())
        {
            itemDisplay.color = new Color(1, 1, 1, 0);
            quantityDisplay.text = "";
        }
        else
        {
            itemDisplay.color = new Color(1, 1, 1, 1);
            itemDisplay.sprite = item.LoadSprite();
            quantityDisplay.text = item.amount.ToString();
        }
    }

    private void Update()
    {
        Display();
    }
}
