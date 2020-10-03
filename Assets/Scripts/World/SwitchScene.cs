using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string playerTag;
    public string scene;
    public FadeToBlack fader;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(playerTag))
        {
            StartCoroutine(SwitchSceneCrt());
        }
    }

    private IEnumerator SwitchSceneCrt()
    {
        yield return fader.FadeOut();
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
