using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleAll : Toggleable
{
    List<Toggleable> toggles = null;

    public override void SetActive(bool value)
    {
        if (toggles == null)
        {
            toggles = GetComponents<Toggleable>().ToList();
            toggles.Remove(this);
        }
        toggles.ForEach(toggle => toggle.SetActive(value));
    }
}
