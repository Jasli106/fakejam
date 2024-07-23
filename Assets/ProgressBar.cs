using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform mask;

    public void SetProgressBar(float percentage)
    {
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 350 * (1-percentage));
    }
}
