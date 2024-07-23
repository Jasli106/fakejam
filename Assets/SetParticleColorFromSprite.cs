using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SetParticleColorFromSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] ParticleSystem ps;
    [SerializeField] float darkenFactorStart = 0.8f;
    [SerializeField] float darkenFactorEnd = 0.2f;

    private void Awake()
    {
        SetColor();
    }

    private void OnEnable()
    {
        SetColor();
    }

    public void SetColor()
    {
        Sprite sprite = sr.sprite;
        Vector2 center = sprite.textureRect.center;
        Vector2 readPoint = center + sprite.textureRect.size * Random.Range(-0.2f, 0.2f);
        Color color = sprite.texture.GetPixel((int)readPoint.x, (int)readPoint.y);
        Debug.Log(color);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(darkenFactorStart * color, 0.0f), new GradientColorKey(darkenFactorEnd * color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        var colorModule = ps.colorOverLifetime;
        colorModule.color = new ParticleSystem.MinMaxGradient(gradient);
    }
}
