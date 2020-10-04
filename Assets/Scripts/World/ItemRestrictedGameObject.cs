using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRestrictedGameObject : MonoBehaviour
{
    public InventoryItem requiredItem;

    // Start is called before the first frame update
    void Awake()
    {
        if (!InventoryManager.Instance.HasItem(requiredItem))
        {
            gameObject.SetActive(false);
        }
    }
}

