using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public float amplitude = 0;
    public float period = 1;
    public float offset = 0;

    public Wave(float amplitude, float period, float offset)
    {
        this.amplitude = amplitude;
        this.period = period;
        this.offset = offset;
    }

    public float Evaluate(float t)
    {
        return amplitude * Mathf.Sin(2 * Mathf.PI * ((t + offset) / period));
    }
}
public class Wobble : MonoBehaviour
{
    public Wave xPos;
    public Wave yPos;
    public Wave zPos;
    public Wave xRot;
    public Wave yRot;
    public Wave zRot;
    public Wave xScale;
    public Wave yScale;
    public Wave zScale;

    void Update()
    {
        float t = Time.time;
        transform.localPosition = new Vector3(xPos.Evaluate(t), yPos.Evaluate(t), zPos.Evaluate(t));
        transform.localRotation = Quaternion.Euler(new Vector3(xRot.Evaluate(t), yRot.Evaluate(t), zRot.Evaluate(t)));
        transform.localScale = new Vector3(Mathf.Exp(xScale.Evaluate(t)), Mathf.Exp(yScale.Evaluate(t)), Mathf.Exp(zScale.Evaluate(t)));
    }
}
