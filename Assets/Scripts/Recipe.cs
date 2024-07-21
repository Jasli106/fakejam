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
            "furnace",
            new List<(string, float)>(),
            new List<Item>() {
                new Item("iron ore sand", 1),
                new Item("oak charcoal", 1)
            },
            new List<Item>() {
                new Item("iron bloom", 1)
            }
        )
    };
}
