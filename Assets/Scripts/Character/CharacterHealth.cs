using UnityEngine;
using System.Collections;
using System;

public class CharacterHealth : MonoBehaviour
{
    public int maxHealth;
    public float invulnerableDuration = .3f;
    public bool destroyOnDeath = false;

    [Header("Effects")]
    public AudioClip deathSFX;
    public float deathVolume = .1f;
    public ParticleSystem deathVFX;

    public int CurrentHealth { get; private set; }

    public delegate void OnDamage(int amount);
    public delegate void OnDeath();
    public delegate void OnHeal();

    public OnDamage onDamage;
    public OnDeath onDeath;
    public OnHeal onHeal;

    private bool _invulnerable;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void Damage(int amount)
    {
        if(_invulnerable)
        {
            return;
        }

        if(amount <= 0)
        {
            amount = 1;
        }

        CurrentHealth -= amount;
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        else
        {
            StartCoroutine(MakeInvulnerable());
            onDamage?.Invoke(amount);
        }
    }

    public void Die()
    {
        float waitTime = 0;
        if (deathSFX)
        {
            AudioManager.Instance.PlaySound(deathSFX, transform.position, null, false, deathVolume);
            waitTime = deathSFX.length;
        }

        if(deathVFX)
        {
            ParticleSystem instance = Instantiate(deathVFX, transform.position, Quaternion.identity);
            float duration = instance.main.duration;

            Destroy(instance, duration);
            waitTime = waitTime < duration ? duration : waitTime;
        }

        StartCoroutine(WaitEffectsToFinish(waitTime));
    }

    private IEnumerator WaitEffectsToFinish(float waitTime)
    {
        HideChildren();
        _invulnerable = true;

        yield return new WaitForSeconds(waitTime);

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            ShowChildren();
            gameObject.SetActive(false);
        }

        onDeath?.Invoke();
    }

    private void HideChildren()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void ShowChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void Heal(int amount)
    {
        if(amount <= 0)
        {
            amount = 1;
        }

        CurrentHealth += amount;
        if(CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
    }

    private IEnumerator MakeInvulnerable()
    {
        _invulnerable = true;
        yield return new WaitForSeconds(invulnerableDuration);
        _invulnerable = false;
    }
}
