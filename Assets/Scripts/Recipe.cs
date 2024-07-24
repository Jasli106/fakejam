using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public string machine;
    public float time;
    public float costs;
    public List<Item> inputs;
    public List<Item> outputs;

    public Recipe(string machine, float time, float costs, List<Item> inputs, List<Item> outputs)
    {
        this.machine = machine;
        this.time = time;
        this.costs = costs;
        this.inputs = inputs;
        this.outputs = outputs;
    }

    public static List<Recipe> list = new List<Recipe>()
    {
        new Recipe (
            "Furnace",
            3f,
            0,
            new List<Item>() {
                new Item("Iron Ore Sand", 1),
                new Item("Oak Charcoal", 1)
            },
            new List<Item>() {
                new Item("Iron Bloom", 1)
            }
        ),
        new Recipe (
            "Furnace",
            3f,
            0,
            new List<Item>() {
                new Item("Charcoal", 1)
            },
            new List<Item>() {
                new Item("Water", 1)
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

    public static List<Recipe> ItemInRecipes(string itemType)
    {
        List<Recipe> validRecipes = new List<Recipe>();
        foreach (var recipe in list)
        {
            foreach(var item in recipe.inputs)
            {
                if (item.type == itemType)
                {
                    validRecipes.Add(recipe);
                }
            }
        }
        return validRecipes;
    }

    public static List<Recipe> ItemOutRecipes(string itemType)
    {
        List<Recipe> validRecipes = new List<Recipe>();
        foreach (var recipe in list)
        {
            foreach (var item in recipe.outputs)
            {
                if (item.type == itemType)
                {
                    validRecipes.Add(recipe);
                }
            }
        }
        return validRecipes;
    }
}
