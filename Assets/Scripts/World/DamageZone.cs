using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageZone : MonoBehaviour
{
    public int damage = 1;
    public LayerMask damageMask;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Damage(other);
    }

    private void Damage(Collider2D other)
    {
        if (damageMask == (damageMask | (1 << other.gameObject.layer)))
        {
            CharacterHealth health = other.GetComponent<CharacterHealth>();
            if (health)
            {
                health.Damage(damage);
            }
        }
    }
}
