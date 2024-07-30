using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


public class Type
{
    public static Dictionary<string, List<List<Ingredient>>> options = new Dictionary<string, List<List<Ingredient>>>
    {
        {
            "fuel", new List<List<Ingredient>>
            {
                new List<Ingredient> { new Ingredient("Wood", 8),  new Ingredient("Bark", 3)},
                new List<Ingredient> { new Ingredient("Charcoal", 1) }
            }
        }
    };
    public static Dictionary<string, List<string>> replacements = new Dictionary<string, List<string>>()
    {
        { "metal", new List<string> { "Copper" } }
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

    private static Item Replace(Ingredient ingredient, List<string> types, List<string> replacement)
    {
        Item i = new Item(ingredient.type, ingredient.amount);
        types.Zip(replacement, (t, r) => new { t, r })
                    .ToList()
                    .ForEach(x => i.type = i.type.Replace($"<{x.t}>", x.r));
        return i;
    }

    private static void AddOptionToDict(Ingredient ingredient, Dictionary<string, int> ingredients, List<string> types, List<List<Ingredient>> replacement)
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

    private static List<Ingredient> DictToList(Dictionary<string, int> ingredients)
    {
        return ingredients.Select(kvp => new Ingredient(kvp.Key, kvp.Value)).ToList();
    }

    private static void AddToDictValue(Dictionary<string, int> dict, string str, int val)
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


    private static List<string> UniqueTypes(List<Ingredient> inputs, List<Ingredient> outputs)
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

    public static List<Recipe> Convert(string machine, float time, float costs, List<Ingredient> inputs, List<Ingredient> outputs)
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

    public static List<Recipe> DoNameReplacements(string machine, float time, float costs, List<Ingredient> inputs, List<Ingredient> outputs)
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

    public static List<Recipe> AllRecipes()
    {
        List<Recipe> allRecipes = new List<Recipe>();

        // Furnace
        {
            allRecipes.AddRange(Ingredient.Convert(
                "Furnace",
                2f,
                0,
                new List<Ingredient>() {
                new Ingredient("<fuel>", 1),
                new Ingredient("Sand", 3),
                new Ingredient("Wood", 5)
                },
                new List<Ingredient>() {
                new Ingredient("Charcoal", 3),
                new Ingredient("Sand", 2)
                }
            ));

            allRecipes.AddRange(Ingredient.Convert(
                "Furnace",
                3f,
                0,
                new List<Ingredient>() {
                new Ingredient("<fuel>", 1),
                new Ingredient("Blank Mold", 1),
                new Ingredient("Sand", 1)
                },
                new List<Ingredient>() {
                new Ingredient("Glass", 1),
                new Ingredient("Blank Mold", 1)
                }
            ));

            allRecipes.AddRange(Ingredient.Convert(
                "Furnace",
                3f,
                0,
                new List<Ingredient>() {
                new Ingredient("<fuel>", 1),
                new Ingredient("Charcoal", 1),
                new Ingredient("<metal> Grit", 1)
                },
                new List<Ingredient>() {
                new Ingredient("<metal> Ingot", 1)
                }
            ));

            allRecipes.AddRange(Ingredient.Convert(
                "Furnace",
                5f,
                0,
                new List<Ingredient>() {
                    new Ingredient("<fuel>", 1),
                    new Ingredient("Sand", 5),
                    new Ingredient("Clay", 3)
                },
                new List<Ingredient>() {
                    new Ingredient("Blank Mold", 1)
                }
             ));
        }

        // Crafter
        {
            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                4f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Bark", 2),
                    new Ingredient("Stone", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("String", 1),
                    new Ingredient("Stone", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                4f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Bark", 1),
                    new Ingredient("Wood", 1),
                    new Ingredient("String", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Primative Bellow", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                2f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Copper Ingot", 2),
                    new Ingredient("Glass", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Pipe", 8)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                6f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Stone", 1),
                    new Ingredient("String", 1),
                    new Ingredient("Wood", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Primative Hammer", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                20f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Copper Ore", 5),
                    new Ingredient("Primative Hammer", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Copper Grit", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                2f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Blank Mold", 1),
                    new Ingredient("Wood", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Rod Mold", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                3f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Rod Mold", 2)
                },
                new List<Ingredient>() {
                    new Ingredient("Gear Mold", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                3f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Copper Rod", 2),
                    new Ingredient("Copper Ingot", 1),
                    new Ingredient("String", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Copper Pickaxe", 1)
                }
             ));
        }

        // Crafter (Machine)
        {
            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                10f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Clay", 5),
                    new Ingredient("Copper Ingot", 3),
                    new Ingredient("Primative Bellow", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Furnace", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                15f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Copper Ingot", 4),
                    new Ingredient("String", 6),
                    new Ingredient("Primative Hammer", 2)
                },
                new List<Ingredient>() {
                    new Ingredient("Metal Press", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                8f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Copper Ingot", 3),
                    new Ingredient("String", 3),
                    new Ingredient("Copper Gear", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Crafter", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                10f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Clay", 15),
                    new Ingredient("Stone", 5),
                    new Ingredient("Primative Bellow", 2)
                },
                new List<Ingredient>() {
                    new Ingredient("Clay Furnace", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                5f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Copper Ingot", 1),
                    new Ingredient("Wood", 10)
                },
                new List<Ingredient>() {
                    new Ingredient("Chest", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                25f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Copper Pickaxe", 1),
                    new Ingredient("Copper Gear", 3),
                    new Ingredient("Copper Ingot", 3),
                    new Ingredient("Copper Rod", 5),
                    new Ingredient("Chest", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("Auto Pickaxe", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Crafter",
                18f,
                0,
                new List<Ingredient>() {
                    new Ingredient("Copper Gear", 15),
                    new Ingredient("Copper Rod", 20),
                    new Ingredient("Copper Ingot", 25),
                    new Ingredient("Pipe", 30)
                },
                new List<Ingredient>() {
                    new Ingredient("Steam Engine", 1)
                }
             ));
        }

        // Metal Press
        {
            allRecipes.AddRange(Ingredient.Convert(
                "Metal Press",
                1f,
                0,
                new List<Ingredient>() {
                    new Ingredient("<metal> Ore", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("<metal> Grit", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Metal Press",
                3f,
                0,
                new List<Ingredient>() {
                    new Ingredient("<metal> Ingot", 4),
                    new Ingredient("Gear Mold", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("<metal> Gear", 1),
                    new Ingredient("Gear Mold", 1)
                }
             ));

            allRecipes.AddRange(Ingredient.Convert(
                "Metal Press",
                3f,
                0,
                new List<Ingredient>() {
                    new Ingredient("<metal> Ingot", 2),
                    new Ingredient("Rod Mold", 1)
                },
                new List<Ingredient>() {
                    new Ingredient("<metal> Rod", 1),
                    new Ingredient("Rod Mold", 1)
                }
             ));
        }

        return allRecipes;
    }



    public static List<Recipe> list = AllRecipes();

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
            foreach (var item in recipe.inputs)
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
