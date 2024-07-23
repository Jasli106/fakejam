using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSegment : TileObject
{
    public enum PypeType
    {
        Straight,
        Bent,
        Tri,
        Quad
    }

    [SerializeField] Sprite straight;
    [SerializeField] Sprite bent;
    [SerializeField] Sprite tri;
    [SerializeField] Sprite quad;

    [SerializeField] SpriteRenderer sr;

    public PypeType type;
    public Quaternion rotation;

    public void SetPipeType(PypeType pipeTipe)
    {
        type = pipeTipe;

        switch (pipeTipe)
        {
            case PypeType.Straight:
                sr.sprite = straight;
                break;
            case PypeType.Bent:
                sr.sprite = bent;
                break;
            case PypeType.Tri:
                sr.sprite = tri;
                break;
            case PypeType.Quad:
                sr.sprite = quad;
                break;
            default:
                sr.sprite = straight;
                break;
        }
    }

    public void SetRotation(Quaternion rot)
    {
        rotation = rot;
        transform.rotation = rot;
    }

    public void SetFlipped(bool isFlipped)
    {
        //sr.flipX = isFlipped;
        sr.flipY = isFlipped;
    }

}
