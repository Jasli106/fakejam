using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnableableTileObject : TileObject
{
    [SerializeField] float fps = 8f;
    [SerializeField] Sprite[] disabledSprites;
    [SerializeField] Sprite[] enabledSprites;
    [SerializeField] List<GameObject> enabledWhileWorking;
    [SerializeField] List<Toggleable> enableTogglesWhileWorking;
    protected bool turnedOn = false;
    float timeOfStateChange = 0;

    private float TimeSinceStateChange()
    {
        return Time.time - timeOfStateChange;
    }

    public float TimeSinceEnabled()
    {
        return turnedOn ? TimeSinceStateChange() : 0;
    }

    public void SetEnabled(bool value)
    {
        if (turnedOn == value) return;
        turnedOn = value;
        timeOfStateChange = Time.time;
        enabledWhileWorking.ForEach(go => go.SetActive(value));
        enableTogglesWhileWorking.ForEach(to => to.SetActive(value));
    }

    public override void Start()
    {
        base.Start();
        enableTogglesWhileWorking.ForEach(to => to.SetActive(false));
    }
    public virtual void Update()
    {
        Animate();
    }
    public void Animate()
    {
        int frame = (int)(TimeSinceStateChange() * fps);
        if (!turnedOn)
        {
            if (disabledSprites.Length == 0) return;
            spriteRenderer.sprite = disabledSprites[frame % disabledSprites.Length];
        }
        else
        {
            if (enabledSprites.Length == 0) return;
            spriteRenderer.sprite = enabledSprites[frame % enabledSprites.Length];
        }
    }

}
