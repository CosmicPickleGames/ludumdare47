using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRestrictedGameObject : MonoBehaviour
{
    public string requiredTrigger;

    void Awake()
    {
        if(!SaveLoadManager.Instance.Data.ShownTriggers.Contains(requiredTrigger))
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf && SaveLoadManager.Instance.Data.ShownTriggers.Contains(requiredTrigger))
        {
            gameObject.SetActive(true);
        }
    }

}
