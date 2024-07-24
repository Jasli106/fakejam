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

public interface InputInventory
{
    public Inventory GetInputInventory();
}

public interface OutputInventory
{
    public Inventory GetOutputInventory();
}


public class Machine : TileObject, InputInventory, OutputInventory
{
    public static readonly float fps = 8f;
    [SerializeField] MachineType type;
    float timeOfStateChange = 0;
    bool working = false;
    List<Recipe> recipes;
    Inventory input;
    Inventory output;

    Recipe currentRecipe = null;
    float timeOfRecipeStart = 0f;

    [SerializeField] GameObject[] enabledWhileWorking;


    private void Awake()
    {
        input = new Inventory(type.inputSlots);
        output = new Inventory(type.outputSlots);
        recipes = Recipe.MachineRecipes(type);
    }

    private void SetWorking(bool value)
    {
        if (working == value) return;
        working = value;
        timeOfStateChange = Time.time;
        foreach (GameObject go in enabledWhileWorking)
        {
            go.SetActive(value);
        }
    }

    public float Progress()
    {
        if (currentRecipe == null)
        {
            return 0;
        }
        return (Time.time - timeOfRecipeStart) / currentRecipe.time;
    }


    private void Update()
    {
        //TODO: Multiple recipes started and completed per frame using deltaTime
        if (Progress() >= 1) {
            FinishRecipe();
        }
        CheckForInputs();
        if (currentRecipe == null)
        {
            SetWorking(false);
        }
        Animate();
    }

    public void CheckForInputs()
    {
        if (currentRecipe != null) return;
        foreach (Recipe recipe in recipes)
        {
            bool containsInputs = input.ContainsItems(recipe.inputs);
            if (containsInputs)
            {
                bool containsOutputRoom = output.InsertPossible(recipe.outputs);
                if (containsOutputRoom)
                {
                    StartRecipe(recipe);
                }
            }
        }
    }

    public void FinishRecipe()
    {
        output.InsertItems(currentRecipe.outputs, false);
        currentRecipe = null;
    }
    public void StartRecipe(Recipe recipe)
    {
        input.RemoveItems(recipe.inputs);
        currentRecipe = recipe;
        timeOfRecipeStart = Time.time;
        SetWorking(true);
    }
    public void Animate()
    {
        float timeSinceStateChange = Time.time - timeOfStateChange;
        int frame = (int)(timeSinceStateChange * fps);
        if (!working)
        {
            spriteRenderer.sprite = type.idle[frame % type.idle.Length];
        }
        else
        {
            spriteRenderer.sprite = type.working[frame % type.working.Length];
        }
    }

    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        InventoryManager.instance.OpenMachineInventory(input, output, this);
    }

    public Inventory GetInputInventory()
    {
        return input;
    }

    public Inventory GetOutputInventory()
    {
        return output;
    }
}
