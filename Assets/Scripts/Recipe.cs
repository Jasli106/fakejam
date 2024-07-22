using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public string machine;
    public List<(string, float)> costs;
    public List<Item> inputs;
    public List<Item> outputs;

    public Recipe(string machine, List<(string, float)> costs, List<Item> inputs, List<Item> outputs)
    {
        this.machine = machine;
        this.costs = costs;
        this.inputs = inputs;
        this.outputs = outputs;
    }

    public static List<Recipe> list = new List<Recipe>()
    {
        new Recipe (
            "Furnace",
            new List<(string, float)>(),
            new List<Item>() {
                new Item("Iron Ore Sand", 1),
                new Item("Oak Charcoal", 1)
            },
            new List<Item>() {
                new Item("Iron Bloom", 1)
            }
        )
    };

    public static List<Recipe> MachineRecipes(MachineType machine)
    {
        List<Recipe> validRecipes = new List<Recipe>();
        foreach (var recipe in list)
        {
            if (recipe.machine == machine.type)
            {
                validRecipes.Add(recipe);
            }
        }
        return validRecipes;
    }
}
