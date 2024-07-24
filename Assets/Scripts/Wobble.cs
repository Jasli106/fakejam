using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public float amplitude = 0f;
    public float period = 1f;
    public float offset = 0f;
    public AnimationCurve curve = new AnimationCurve(
        new Keyframe(-1, -1, 1, 1),
        new Keyframe(1, 1, 1, 1)
    );

    public float Evaluate(float t)
    {
        return amplitude * curve.Evaluate(Mathf.Sin(2 * Mathf.PI * ((t + offset) / period)));
    }
}
public class Wobble : MonoBehaviour
{
    public Transform transformToWobble = null;
    public Wave xPos;
    public Wave yPos;
    public Wave zPos;
    public Wave xRot;
    public Wave yRot;
    public Wave zRot;
    public Wave xScale;
    public Wave yScale;
    public Wave zScale;

    float timeOfWobbleStart = 0f;

    private void OnEnable()
    {
        timeOfWobbleStart = Time.time;
    }

    void Update()
    {
        if (transformToWobble == null)
        {
            transformToWobble = transform;
        }
        float t = Time.time - timeOfWobbleStart;
        transformToWobble.localPosition = new Vector3(xPos.Evaluate(t), yPos.Evaluate(t), zPos.Evaluate(t));
        transformToWobble.localRotation = Quaternion.Euler(new Vector3(xRot.Evaluate(t), yRot.Evaluate(t), zRot.Evaluate(t)));
        transformToWobble.localScale = new Vector3(Mathf.Exp(xScale.Evaluate(t)), Mathf.Exp(yScale.Evaluate(t)), Mathf.Exp(zScale.Evaluate(t)));
    }
}
