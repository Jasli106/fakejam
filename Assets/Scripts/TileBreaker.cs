using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBreaker : MonoBehaviour
{
    TileObject currentlyBreaking = null;
    float breakStartTime = Mathf.Infinity;

    TileObject ObjectToBreak()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 tilePosition = TileObject.Round(mouseWorldPosition);
        Debug.Log($"pos is {tilePosition}");
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
        if (Input.GetMouseButton(1))
        {
            TileObject previouslyBreaking = currentlyBreaking;
            currentlyBreaking = ObjectToBreak();
            Debug.Log(currentlyBreaking == null);
            if (previouslyBreaking != currentlyBreaking)
            {
                breakStartTime = Time.time;
            }
            Debug.Log(BreakProgress());
            if (BreakProgress() >= 1)
            {
                currentlyBreaking.Break();
                currentlyBreaking = null;
            }
        }
        else
        {
            breakStartTime = Mathf.Infinity;
        }
    }
}
