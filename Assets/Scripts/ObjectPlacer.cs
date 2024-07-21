using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] float range = 5;
    [SerializeField] GameObject ghostRender;
    Transform ghostPlacementTransform;
    SpriteRenderer ghostPlacementRenderer;
    bool ghostEnabled = true;

    private void Awake()
    {
        ghostPlacementTransform = ghostRender.transform;
        ghostPlacementRenderer = ghostRender.GetComponent<SpriteRenderer>();
    }

    Vector3 PlacementPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        return TileObject.Round(mouseWorldPosition);
        //float x = Mathf.Round(mouseWorldPosition.x);
        //float y = Mathf.Round(mouseWorldPosition.y);
        //return new Vector3(x, y, 0);
    }

    void SetGhostEnabled(bool value)
    {
        ghostEnabled = value;
        ghostPlacementRenderer.enabled = ghostEnabled;
    }

    private void Update()
    {
        if (ghostEnabled)
        {
            ghostPlacementTransform.position = PlacementPosition();
        }
    }
}
