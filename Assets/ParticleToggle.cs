using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleToggle : Toggleable
{
    private ParticleSystem system = null;

    public override void SetActive(bool value)
    {
        active = value;
        if (system == null)
        {
            system = GetComponent<ParticleSystem>();
        }
        if (value)
        {
            system.Play();
        }
        else
        {
            system.Stop();
        }
    }
}
