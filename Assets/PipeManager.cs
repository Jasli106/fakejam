using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    public List<PipeSystem> systems = new List<PipeSystem>();

    public static PipeManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Update()
    {
        //Debug.Log($"transfering from {systems.Count} systems");
        foreach (PipeSystem system in systems)
        {
            system.Transfer();
        }
    }
}
