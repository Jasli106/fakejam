using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.CameraUI;
using UnityEngine.Windows;

public abstract class TileUI : MonoBehaviour
{
    public abstract void OpenTileUI(TileObject tileObject);
    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }
}


public class MachineUI : TileUI
{
    public ProgressBar progressBar;
    public Animator gearsAnim;
    public InventoryUIController machineIn;
    public InventoryUIController machineOut;
    Machine machine = null;
    public override void OpenTileUI(TileObject tileObject)
    {
        if (tileObject is Machine m)
        {
            machine = m;
            machineIn.RepresentInventory(m.GetInputInventory());
            machineOut.RepresentInventory(m.GetOutputInventory());
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Opened tile for MachineUI must be Machine");
        }
    }

    void Update()
    {
        if (machine.Progress() == 0)
        {
            gearsAnim.enabled = false;
        }
        else
        {
            gearsAnim.enabled = true;
        }
        progressBar.SetProgressBar(machine.Progress());
    }
}
