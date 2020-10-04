using UnityEngine;
using System.Collections;

public class ItemPickupInteractable : Interactable
{
    public InventoryItem item;

    protected override void OnInteract()
    {
        InventoryManager.Instance.Add(item);
    }
}
