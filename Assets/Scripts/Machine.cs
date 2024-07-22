using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MachineType
{
    public string type = "Untyped Machine";
    public int inputSlots = 9;
    public int outputSlots = 9;
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
    List<Recipe> recipes;
    Inventory input;
    Inventory output;

    private void Awake()
    {
        input = new Inventory(type.inputSlots);
        output = new Inventory(type.outputSlots);
        sr = GetComponent<SpriteRenderer>();
        recipes = Recipe.MachineRecipes(type);
    }

    private void SetWorking(bool value)
    {
        if (working == value) return;
        working = value;
        timeOfStateChange = Time.time;
    }

    private void Update()
    {
        CheckForInputs();
        Animate();
    }

    public void CheckForInputs()
    {
        foreach (Recipe recipe in recipes)
        {

        }
    }

    public void Animate()
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

    public override void ClickDown(MouseInteractor mouse)
    {
        InventoryManager.instance.OpenMachineInventory(input, output);
    }
}
