using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MachineType
{
    public string type = "Untyped Machine";
    public Sprite[] idle;
    public Sprite[] working;
}


[RequireComponent(typeof(SpriteRenderer))]
public class Machine : TileObject
{
    public static readonly float fps = 8f;
    [SerializeField] MachineType type;
    float timeOfStateChange = 0;
    bool working = false;
    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void SetWorking(bool value)
    {
        if (working == value) return;
        working = value;
        timeOfStateChange = Time.time;
    }

    private void Update()
    {
        float timeSinceStateChange = Time.time - timeOfStateChange;
        int frame = (int)(timeSinceStateChange * fps);
        if (working)
        {
            sr.sprite = type.idle[frame % type.idle.Length];
        }
        else
        {
            sr.sprite = type.working[frame % type.working.Length];
        }
    }

    public override void ClickDown()
    {
        InventoryManager.instance.SetMachineInventory(true);
    }
}
