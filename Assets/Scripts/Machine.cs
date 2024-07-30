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
    public float speedMultiplier = 1f;
}

public interface InputInventory
{
    public Inventory GetInputInventory();
}

public interface OutputInventory
{
    public Inventory GetOutputInventory();
}


public class Machine : EnableableTileObject, InputInventory, OutputInventory
{
    [SerializeField] MachineType type;
    List<Recipe> recipes;
    Inventory input;
    Inventory output;
    float timeRecipeStart = Mathf.Infinity;

    Recipe currentRecipe = null;


    private void Awake()
    {
        input = new Inventory(type.inputSlots);
        output = new Inventory(type.outputSlots);
        recipes = Recipe.MachineRecipes(type);
    }


    public float Progress()
    {
        if (currentRecipe == null)
        {
            return 0;
        }
        return (Time.time - timeRecipeStart) * type.speedMultiplier / currentRecipe.time;
    }


    public override void Update()
    {
        //TODO: Multiple recipes started and completed per frame using deltaTime
        if (Progress() >= 1) {
            FinishRecipe();
        }
        CheckForInputs();
        if (currentRecipe == null)
        {
            SetEnabled(false);
        }
        base.Update();
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
        SetEnabled(true);
        timeRecipeStart = Time.time;
    }
    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        if (!firstClick) return;
        InventoryManager.instance.OpenMachineInventory(this);
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
