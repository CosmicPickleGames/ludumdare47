using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    public enum Mode
    {
        Manual,
        Auto
    }

    public enum ManualHideType
    {
        Duration,
        UntilHidden
    }

    [Header("Show settings")]
    public Mode mode;
    public float fadeDuration = .3f;

    [Header("Persistence")]
    public string promptId = "";
    public bool oneTimePersistent = false;

    [Header("Manual settings")]
    public ManualHideType manualHideType;
    public float manualShowDuration;

    [Header("Auto trigger")]
    public LayerMask autoTriggerMask;
    public bool triggerOnAwake;

    CanvasGroup _canvasGroup;
    Collider2D _autoTriggerCollider;

    // Start is called before the first frame update
    void Awake()
    {
        if (oneTimePersistent && promptId.Length != 0)
        {
            if(SaveLoadManager.Instance.Data.ShownTriggers.Contains(promptId))
            {
                Destroy(gameObject);
                return;
            }
            
        }

        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _canvasGroup.alpha = 0;

        _autoTriggerCollider = GetComponent<Collider2D>();

        if(mode == Mode.Auto && triggerOnAwake)
        {
            Show();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(mode == Mode.Manual)
        {
            return;
        }

        if (autoTriggerMask == (autoTriggerMask | (1 << other.gameObject.layer)))
        {
            Show();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (mode == Mode.Manual)
        {
            return;
        }

        if (autoTriggerMask == (autoTriggerMask | (1 << other.gameObject.layer)))
        {
            Hide();
        }
    }

    public void Show()
    {
        StartCoroutine(ShowCrt());
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(HideCrt());
        }
    }

    private IEnumerator ShowCrt()
    {
        yield return StartCoroutine(Fade(0, 1));

        if(mode == Mode.Manual && manualHideType == ManualHideType.Duration)
        {
            yield return new WaitForSeconds(manualShowDuration);
            StartCoroutine(HideCrt());
        }
    }

    private IEnumerator HideCrt()
    {
        yield return StartCoroutine(Fade(1, 0));

        if(oneTimePersistent && promptId.Length != 0)
        {
            SaveLoadManager.Instance.Data.ShownTriggers.Add(promptId);
            SaveLoadManager.Instance.Save();
            Destroy(gameObject);
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        _canvasGroup.alpha = startAlpha;
        float speed = 1 / fadeDuration;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        _canvasGroup.alpha = endAlpha;
    }
}
