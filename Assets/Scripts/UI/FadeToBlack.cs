using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    public bool fadeInOnAwake = true;
    public float fadeDuration = .2f;

    private CanvasGroup _group
    {
        get
        {
            if(__group == null)
            {
                __group = GetComponent<CanvasGroup>();
            }

            return __group;
        }
    }

    private CanvasGroup __group;

    private void Awake()
    {
        if(fadeInOnAwake)
        {
            StartCoroutine(FadeIn());
        }
    }

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(0, 1));
    }

    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        _group.alpha = startAlpha;
        float speed = 1 / fadeDuration;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            _group.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        _group.alpha = endAlpha;
    }
}
