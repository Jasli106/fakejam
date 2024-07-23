using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBreaker : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    TileObject currentlyBreaking = null;
    float breakStartTime = Mathf.Infinity;

    TileObject ObjectToBreak()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 tilePosition = TileObject.Round(mouseWorldPosition);
        if (!TileObject.objectPositions.ContainsKey(tilePosition)) return null;
        return TileObject.objectPositions[tilePosition];
    }

    private float BreakProgress()
    {
        if (!CurrentlyBreaking())
        {
            return 0;
        }
        return (Time.time - breakStartTime) / currentlyBreaking.breakTime;
    }

    private bool CurrentlyBreaking()
    {
        return currentlyBreaking != null;
    }
    private void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            StopBreaking();
            return;
        }

        TileObject newBreakTarget = ObjectToBreak();


        if (newBreakTarget == null)
        {
            StopBreaking();
            return;
        }

        if (newBreakTarget != currentlyBreaking)
        {
            StopBreaking();
            StartBreaking(newBreakTarget);
        }

        if (BreakProgress() >= 1)
        {
            currentlyBreaking.Break();
            currentlyBreaking = null;
            breakStartTime = Mathf.Infinity;
        }
    }

    public void StopBreaking()
    {
        if (currentlyBreaking != null)
        {
            currentlyBreaking.SetBreaking(false);
            currentlyBreaking = null;
        }
        breakStartTime = Mathf.Infinity;
        playerController.movementEnabled = true;
    }

    public void StartBreaking(TileObject obj)
    {
        currentlyBreaking = obj;
        currentlyBreaking.SetBreaking(true);
        breakStartTime = Time.time;
        playerController.movementEnabled = false;
    }
}
