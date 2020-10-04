using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveRestrictedParticleSystem : MonoBehaviour
{
    public int index = 2;
    private ParticleSystem _system;

    void Awake()
    {
        _system = GetComponent<ParticleSystem>();
        if (SaveLoadManager.Instance.Data.CompletedMainObjectiveIndex >= index)
        {
            _system.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_system && !_system.isPlaying && SaveLoadManager.Instance.Data.CompletedMainObjectiveIndex >= index)
        {
            _system.Play();
        }
    }

}
