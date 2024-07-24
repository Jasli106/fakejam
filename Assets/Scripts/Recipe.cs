using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;


public class Type
{
    public static Dictionary<string, List<List<Ingredient>>> options = new Dictionary<string, List<List<Ingredient>>>
    {
        {
            "fuel", new List<List<Ingredient>>
            {
                new List<Ingredient> { new Ingredient("Wood", 10) },
                new List<Ingredient> { new Ingredient("Charcoal", 1) }
            }
        }
    };
    public static Dictionary<string, List<string>> replacements = new Dictionary<string, List<string>>()
    {
        { "material", new List<string> { "Copper", "Iron" } }
    };
}

public class Ingredient
{
    string type;
    int amount;
    public Ingredient(string type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }


    public Ingredient(Ingredient ingredient)
    {
        type = ingredient.type;
        amount = ingredient.amount;
    }

    public Ingredient Clone() => new Ingredient(this);

    public static List<List<T>> GetCartesianProduct<T>(List<List<T>> lists)
    {
        List<List<T>> result = new List<List<T>>();
        CartesianProductHelper(lists, 0, new List<T>(), result);
        return result;
    }

    private static void CartesianProductHelper<T>(List<List<T>> lists, int depth, List<T> current, List<List<T>> result)
    {
        if (depth == lists.Count)
        {
            result.Add(new List<T>(current));
            return;
        }

        foreach (T item in lists[depth])
        {
            current.Add(item);
            CartesianProductHelper(lists, depth + 1, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }

    private Item Replace(Ingredient ingredient, List<string> types, List<string> replacement)
    {
        Item i = new Item(ingredient.type, ingredient.amount);
        types.Zip(replacement, (t, r) => new { t, r })
                    .ToList()
                    .ForEach(x => i.type = i.type.Replace($"<{x.t}>", x.r));
        return i;
    }

    private void AddOptionToDict(Ingredient ingredient, Dictionary<string, int> ingredients, List<string> types, List<List<Ingredient>> replacement)
    {
        if (!ingredient.type.StartsWith("<") || !ingredient.type.EndsWith(">"))
        {
            AddToDictValue(ingredients, ingredient.type, ingredient.amount);
            return;
        }
        string option = ingredient.type.Substring(1, ingredient.type.Length - 2);

        foreach (Ingredient i in replacement[types.IndexOf(option)])
        {
            AddToDictValue(ingredients, i.type, i.amount);
        }
    }

    private List<Ingredient> DictToList(Dictionary<string, int> ingredients)
    {
        return ingredients.Select(kvp => new Ingredient(kvp.Key, kvp.Value)).ToList();
    }

    private void AddToDictValue(Dictionary<string, int> dict, string str, int val)
    {
        if (dict.TryGetValue(str, out int existingValue))
        {
            dict[str] = existingValue + val;
        }
        else
        {
            dict[str] = val;
        }
    }


    private List<string> UniqueTypes(List<Ingredient> inputs, List<Ingredient> outputs)
    {
        // Get all types in inputs and outputs
        HashSet<string> uniqueTypes = new HashSet<string>();

        List<Ingredient> combinedList = inputs.Concat(outputs).ToList();
        Regex regex = new Regex(@"<([^>]+)>");
        foreach (var ingredient in combinedList)
        {
            MatchCollection matches = regex.Matches(ingredient.type);
            foreach (Match match in matches)
            {
                uniqueTypes.Add(match.Groups[1].Value);
            }
        }
        return uniqueTypes.ToList();
    }

    public List<Recipe> Convert(string machine, float time, float costs, List<Ingredient> inputs, List<Ingredient> outputs)
    {
        List<Recipe> ret = new List<Recipe>();
        List<string> types = UniqueTypes(inputs, outputs);

        // Filter to option replacements
        types = types.Where(type => Type.options.ContainsKey(type)).ToList();

        // Iterate through possible replacements
        List<List<List<Ingredient>>> replacements = types.Select(type => Type.options[type]).ToList();
        foreach (List<List<Ingredient>> replacement in GetCartesianProduct(replacements))
        {
            Dictionary<string, int> inputDict = new Dictionary<string, int>();
            Dictionary<string, int> outputDict = new Dictionary<string, int>();
            foreach (var ingredient in inputs)
            {
                AddOptionToDict(ingredient, inputDict, types, replacement);
            }
            foreach (var ingredient in outputs)
            {
                AddOptionToDict(ingredient, outputDict, types, replacement);
            }
            List<Ingredient> generatedInput = DictToList(inputDict);
            List<Ingredient> generatedOutput = DictToList(outputDict);

            ret.AddRange(DoNameReplacements(machine, time, costs, generatedInput, generatedOutput));
        }
        return ret;
    }

    public List<Recipe> DoNameReplacements(string machine, float time, float costs, List<Ingredient> inputs, List<Ingredient> outputs)
    {
        List<Recipe> ret = new List<Recipe>();
        

        List<string> types = UniqueTypes(inputs, outputs);

        // Iterate through possible replacements
        List<List<string>> replacements = types.Select(type => Type.replacements[type]).ToList();
        foreach (List<string> replacement in GetCartesianProduct(replacements))
        {
            List<Item> generatedInput = new List<Item>();
            List<Item> generatedOutput = new List<Item>();
            foreach (var ingredient in inputs)
            {
                generatedInput.Add(Replace(ingredient, types, replacement));
            }
            foreach (var ingredient in outputs)
            {
                generatedOutput.Add(Replace(ingredient, types, replacement));
            }

            ret.Add(new Recipe(machine, time, costs, generatedInput, generatedOutput));
        }
        return ret;
    }
}

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
        ),
        new Recipe (
            "Furnace",
            3f,
            0,
            new List<Item>() {
                new Item("Wood", 1)
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

    public static List<(string, List<Recipe>)> ItemInRecipes(string itemType)
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
        return validRecipes.GroupBy(recipe => recipe.machine).Select(group => (group.Key, group.ToList())).ToList();
    }

    public static List<(string, List<Recipe>)> ItemOutRecipes(string itemType)
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
        return validRecipes.GroupBy(recipe => recipe.machine).Select(group => (group.Key, group.ToList())).ToList();
    }
}
