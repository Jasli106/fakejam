using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePlacer : MonoBehaviour
{
    public bool pipeMode = false;

    [SerializeField] GameObject pipePrefab;
    private Vector3 lastPipePos;
    private PipeSegment lastPipeSegment;

    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        lastPipePos = player.currentPosition;
        lastPipeSegment = null;
    }

    public void StartPipeMode()
    {
        lastPipePos = player.currentPosition;
        lastPipeSegment = null;
    }

    void Update()
    {
        if(pipeMode)
        {
            Vector3 currentPos = TileObject.Round(player.currentPosition);
            if (Vector3.Distance(currentPos, lastPipePos) >= 1)
            {
                PlacePipesAlongPath(lastPipePos, currentPos);
            }
        }
    }

    private void PlacePipesAlongPath(Vector3 startPos, Vector3 endPos)
    {
        Vector3 currentPos = startPos;

        while (Vector3.Distance(currentPos, endPos) >= 1)
        {
            PlacePipe(currentPos, GetDirection(currentPos, endPos));
            currentPos = TileObject.Round(Vector3.MoveTowards(currentPos, endPos, 1));
        }
        lastPipePos = currentPos;
    }

    private void PlacePipe(Vector3 position, Quaternion rotation)
    {   
        GameObject newPipe = Instantiate(pipePrefab, position, Quaternion.identity);
        PipeSegment newPipeSegment = newPipe.GetComponent<PipeSegment>();

        if (lastPipeSegment == null)
        {
            // First pipe segment
            newPipeSegment.SetPipeType(PipeSegment.PypeType.Straight);
            newPipeSegment.SetRotation(rotation);
        }
        else
        {
            if (lastPipeSegment.type == PipeSegment.PypeType.Straight || lastPipeSegment.type == PipeSegment.PypeType.Bent)
            {
                float angle = (lastPipeSegment.rotation.eulerAngles.z - rotation.eulerAngles.z + 360) % 360;
                if (Mathf.Approximately(angle, 90))
                {
                    newPipeSegment.SetPipeType(PipeSegment.PypeType.Bent);
                    newPipeSegment.SetRotation(rotation);
                }
                else if (Mathf.Approximately(angle, 270))
                {
                    newPipeSegment.SetPipeType(PipeSegment.PypeType.Bent);
                    newPipeSegment.SetRotation(rotation);
                    newPipeSegment.SetFlipped(true);
                }
                else
                {
                    newPipeSegment.SetPipeType(PipeSegment.PypeType.Straight);
                    newPipeSegment.SetRotation(rotation);
                }
            }
            else
            {
                newPipeSegment.SetPipeType(PipeSegment.PypeType.Straight);
                newPipeSegment.SetRotation(rotation);
            }
        }

        lastPipeSegment = newPipeSegment;
    }

    private Quaternion GetDirection(Vector3 startPos, Vector3 endPos)
    {
        Vector3 direction = (endPos - startPos).normalized;

        if (direction == Vector3.up)
        {
            return Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector3.down)
        {
            return Quaternion.Euler(0, 0, -90);
        }
        else if (direction == Vector3.left)
        {
            return Quaternion.Euler(0, 0, 180);
        }
        else if (direction == Vector3.right)
        {
            return Quaternion.Euler(0, 0, 0);
        }

        return Quaternion.identity;
    }
}
