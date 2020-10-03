using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance && Instance != GetComponent<T>())
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = GetComponent<T>();
        }
    }

    public static T FindEditorInstance()
    {
        return FindObjectOfType<T>();
    }
}