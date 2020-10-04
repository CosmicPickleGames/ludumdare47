using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Crossfade Settings")]
    public float fadeDuration = .4f;

    protected List<AudioSource> loopedSounds;
    protected AudioSource backgroundMusicSource;

    protected override void Awake()
    {
        base.Awake();
        loopedSounds = new List<AudioSource>();
    }

    public IEnumerator FadeOut(AudioSource src = null, float duration = 0)
    {
        float volume = 1;
        if (src == null)
        {
            yield break;
        }

        if (duration <= 0)
        {
            duration = fadeDuration;
        }

        float speed = 1 / duration;
        float t = 0;
        while (t < 1)
        {
            t += speed * Time.deltaTime;
            src.volume = Mathf.Lerp(volume, 0, t);
            yield return null;
        }

        src.volume = 0;
    }

    public IEnumerator FadeIn(AudioSource src = null, float duration = 0)
    {
        float volume = 1;
        if (src == null)
        {
            yield break;
        }

        if (duration <= 0)
        {
            duration = fadeDuration;
        }

        src.volume = 0;

        float speed = 1 / duration;
        float t = 0;
        while (t < 1)
        {
            t += speed * Time.deltaTime;
            src.volume = Mathf.Lerp(0, volume, t);
            yield return null;
        }

        src.volume = volume;
    }


    public virtual AudioSource PlaySound(AudioClip sound, Vector3 position, Transform parent = null, bool looping = false, float volume = 1)
    {
        AudioSource tmpSource = CreateSound(sound, position, parent, volume);

        tmpSource.Play();

        if (looping)
        {
            tmpSource.loop = true;
            loopedSounds.Add(tmpSource);
        }
        else
        {
            Destroy(tmpSource.gameObject, sound.length);
        }

        return tmpSource;
    }

    protected virtual AudioSource CreateSound(AudioClip sound, Vector3 position, Transform parent, float volume)
    {
        GameObject tmpObject = new GameObject("[Audio] SFX");
        tmpObject.transform.parent = parent != null ? parent : transform;
        tmpObject.transform.position = position;

        AudioSource tmpSource = tmpObject.AddComponent<AudioSource>();
        tmpSource.clip = sound;
        tmpSource.volume = Mathf.Clamp01(volume);
        tmpSource.loop = false;

        return tmpSource;
    }

    public void StopLoopingSound(AudioSource source, float fadeOutTime = 0f)
    {
        if (source != null && loopedSounds.Contains(source))
        {
            if (fadeOutTime > 0)
            {
                StartCoroutine(FadeOutLoopingSound(source, fadeOutTime));
            }
            else
            {
                source.Stop();
                loopedSounds.Remove(source);
                Destroy(source.gameObject);
            }
        }
    }

    protected IEnumerator FadeOutLoopingSound(AudioSource source, float duration)
    {
        yield return StartCoroutine(FadeOut(source, duration));

        source.Stop();
        loopedSounds.Remove(source);
        Destroy(source.gameObject);
    }
}