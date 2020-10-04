using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool destroyOnPickup = true;
    public bool persist = true;

    public bool triggerPromptOnInteract = false;
    public Prompt prompt;

    public LayerMask playerMask;

    protected CharacterHealth _health;

    protected virtual void Awake()
    {
        if(SaveLoadManager.Instance.Data.DestroyedInteractables.Contains(name))
        {
            Destroy(gameObject);
        }
        _health = GetComponent<CharacterHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerMask == (playerMask | (1 << other.gameObject.layer)))
        {
            InteractionController ctrl = other.GetComponent<InteractionController>();
            if(ctrl)
            {
                ctrl.UnsetCurrentInteractable();
                ctrl.SetCurrentInteractable(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerMask == (playerMask | (1 << other.gameObject.layer)))
        {
            InteractionController ctrl = other.GetComponent<InteractionController>();
            if (ctrl)
            {
                ctrl.UnsetCurrentInteractable();
            }
        }
    }

    public void Interact()
    {
        OnInteract();
        if (destroyOnPickup)
        {
            if (_health)
            {
                _health.Damage(int.MaxValue);
            }
            else
            {
                Destroy(gameObject);
            }

            if (persist)
            {
                SaveLoadManager.Instance.Data.DestroyedInteractables.Add(name);
                SaveLoadManager.Instance.Save();
            }
        }

        if(triggerPromptOnInteract)
        {
            prompt.Show();
        }
    }

    protected virtual void OnInteract()
    {
        
    }
}
