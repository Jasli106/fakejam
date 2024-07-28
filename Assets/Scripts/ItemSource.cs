using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomItem
{
    public float weight;
    public Item item;
}
public class ItemSource : TileObject
{
    public string sourceType = "";
    [SerializeField] GameObject enableWhileUsing;
    public List<RandomItem> possibleItems = new List<RandomItem>();
    public float delay = 1f;

    private float timer = 0;

    float timeLastUsed = Mathf.NegativeInfinity;

    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        timer = 0;
    }

    private void Update()
    {
        if (Time.time - timeLastUsed > 2 * Time.deltaTime)
        {
            enableWhileUsing.SetActive(false);
            timer = 0;
        }
        else
        {
            enableWhileUsing.SetActive(true);
        }
    }

    private Item RandomItem()
    {
        return possibleItems[RandomItemIndex()].item;
    }
    private int RandomItemIndex()
    {
        float totalWeight = 0f;
        List<int> eligibleIndices = new List<int>();
        for (int i = 0; i < possibleItems.Count; i++)
        {
            if (true)
            {
                totalWeight += possibleItems[i].weight;
                eligibleIndices.Add(i);
            }
        }

        float randomValue = Random.value * totalWeight;
        for (int i = 0; i < eligibleIndices.Count; i++)
        {
            int index = eligibleIndices[i];
            randomValue -= possibleItems[index].weight;
            if (randomValue <= 0f)
            {
                return index;
            }
        }

        Debug.LogError("ItemSource: Failed to choose an item");
        return 0;
    }

    public override void ClickHeld(MouseInteractor mouse)
    {
        Inventory playerInventory = mouse.GetComponentInParent<PlayerInventory>().GetInventory();
        Use(playerInventory);
    }

    public void Use(Inventory inv)
    {
        timeLastUsed = Time.time;

        timer += Time.deltaTime;

        if (timer >= delay)
        {
            CollectItems(inv);
            timer -= delay;
        }
    }

    public void CollectItems(Inventory inventory)
    {
        inventory.InsertItem((Item)RandomItem().Clone());
    }
}
