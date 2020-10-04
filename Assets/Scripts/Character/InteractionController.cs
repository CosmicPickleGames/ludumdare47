using UnityEngine;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    public Interactable CurrentInteractable { get; private set; }

    public void SetCurrentInteractable(Interactable interactable)
    {
        CurrentInteractable = interactable;
    }

    public void UnsetCurrentInteractable()
    {
        CurrentInteractable = null;
    }

    private void Update()
    {
        if (UIMenu.Instance.MenuOpen)
        {
            return;
        }

        if (CurrentInteractable && Input.GetButtonDown("Interact"))
        {
            CurrentInteractable.Interact();
        }
    }
}
