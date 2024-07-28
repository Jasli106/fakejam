using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceMachine : EnableableTileObject, OutputInventory
{
    [SerializeField] List<string> validSources;
    Inventory inventory = new Inventory(27);
    ItemSource source = null;

    public override void ClickDown(MouseInteractor mouse, bool firstClick)
    {
        if (!firstClick) return;
        InventoryManager.instance.OpenInventoryOnlyTileUI(this);
    }

    public Inventory GetOutputInventory()
    {
        return inventory;
    }

    public override void TileUpdate(TileObject neighborObject, Vector2 direction, bool neighborRemoved)
    {
        if (direction != Vector2.right) return;
        if (neighborRemoved)
        {
            SetSource(null);
            return;
        }
        if (neighborObject is ItemSource s)
        {
            SetSource(s);
        }
    }

    public void SetSource(ItemSource newSource)
    {
        source = newSource;
        if (newSource == null)
        {
            SetEnabled(false);
        }
        else
        {
            SetEnabled(true);
        }
        
    }

    public override void Update()
    {
        base.Update();
        if (source == null) return;
        if (validSources.Contains(source.sourceType))
        {
            source.Use(inventory);
        }
    }
}
