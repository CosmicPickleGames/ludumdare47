using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;

public class Key : MonoBehaviour
{
    public SpriteRenderer KeyCenter;
    public Light2D KeyLight;

    public float animateDuration = 1f;
    public float onIntensity = 5.6f;
    public float offIntensity = 0;

    public bool IsTurnedOn { get; private set; }

    public void TurnOn(bool immediate = false)
    {
        StartCoroutine(Animate(false, immediate));
    }

    public void TurnOff(bool immediate = false)
    {
        StartCoroutine(Animate(true, immediate));
    }

    private IEnumerator Animate(bool off = false, bool immediate = false)
    {
        Color startColor = KeyCenter.color;
        Color endColor = KeyCenter.color;

        float startIntensity = off ? onIntensity : offIntensity;
        float endIntensity = off ? offIntensity : onIntensity;
        float startAlpha = off ? 1 : 0;
        float endAlpha = off ? 0 : 1;

        startColor.a = startAlpha;
        endColor.a = endAlpha;

        float t = 0;
        float speed = 1 / animateDuration;

        if (!immediate)
        {
            KeyCenter.color = startColor;
            KeyLight.intensity = startIntensity;

            while (t < 1)
            {
                t += Time.deltaTime * speed;
                KeyCenter.color = Color.Lerp(startColor, endColor, t);
                KeyLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
                yield return null;
            }
        }

        KeyCenter.color = endColor;
        KeyLight.intensity = endIntensity;
        IsTurnedOn = !off;
    }
}
