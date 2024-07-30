using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public static readonly float returnTime = 0.1f;
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
public class Wobble : Toggleable
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

    [SerializeField] float stopTime = 0.4f;

    float timeOfWobbleStart = 0f;
    float timeOfWobbleStop = 0f;

    public override void SetActive(bool value)
    {
        active = value;
        if (value)
        {
            timeOfWobbleStart = Time.time;
        }
        else
        {
            timeOfWobbleStop = Time.time;
        }
    }

    void Update()
    {
        if (transformToWobble == null)
        {
            transformToWobble = transform;
        }
        float t = Time.time - timeOfWobbleStart;
        float m = active ? 1 : Mathf.Lerp(1, 0, (Time.time - timeOfWobbleStop) / stopTime);
        if (m == 0) return;
        transformToWobble.localPosition = new Vector3(m * xPos.Evaluate(t), m * yPos.Evaluate(t), m * zPos.Evaluate(t));
        transformToWobble.localRotation = Quaternion.Euler(new Vector3(m * xRot.Evaluate(t), m * yRot.Evaluate(t), m * zRot.Evaluate(t)));
        transformToWobble.localScale = new Vector3(Mathf.Exp(m * xScale.Evaluate(t)), Mathf.Exp(m * yScale.Evaluate(t)), Mathf.Exp(m * zScale.Evaluate(t)));
    }
}
