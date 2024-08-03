using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public RectTransform backgroundRectTransform;
    public Vector2 padding = new Vector2(8f, 8f);

    private void Awake()
    {
        HideTooltip();
    }

    private void Update()
    {
        // Optional: Make the tooltip follow the mouse
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out anchoredPosition);
        transform.localPosition = anchoredPosition;
    }

    public void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);
        tooltipText.text = tooltipString;

        // Force the text to update so we get accurate measurements
        Canvas.ForceUpdateCanvases();

        // Adjust background size to fit text
        Vector2 textSize = tooltipText.GetPreferredValues(tooltipString);
        backgroundRectTransform.sizeDelta = new Vector2(textSize.x + padding.x * 2, textSize.y + padding.y * 2);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}