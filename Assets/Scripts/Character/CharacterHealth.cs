using UnityEngine;
using System.Collections;
using System;

public class CharacterHealth : MonoBehaviour
{
    public int maxHealth;
    public float invulnerableDuration = .3f;
    public bool destroyOnDeath = false;

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
            if(destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }

            onDeath?.Invoke();
        }
        else
        {
            StartCoroutine(MakeInvulnerable());
            onDamage?.Invoke(amount);
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
