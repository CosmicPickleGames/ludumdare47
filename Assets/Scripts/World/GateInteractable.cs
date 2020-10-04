using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GateInteractable : Interactable
{
    [System.Serializable]
    public class Objective
    {
        public InventoryItem requiredItem;
        public Key keyObject;
        public bool completed;
    }

    public List<Objective> objectives = new List<Objective>();

    [Header("Effects")]
    public AudioClip interactSFX;
    public float interactVolume = .3f;
    public AudioClip completeSFX;
    public float completeVolume = .3f;

    private bool _completed;
    private Objective _currentObjective;

    protected override void Awake()
    {
        base.Awake();
        InitCurrentObjective();
    }

    public void InitCurrentObjective()
    {
        int completedMainObjectiveIndex = SaveLoadManager.Instance.Data.CompletedMainObjectiveIndex;

        Objective nextObjective = null;
        for(int i = 0; i < objectives.Count; i++)
        {
            var objective = objectives[i];
            if(i <= completedMainObjectiveIndex)
            {
                objective.keyObject.TurnOn(true);
            }
            else
            {
                if (nextObjective == null)
                {
                    nextObjective = objective;
                }
                objective.keyObject.TurnOff(true);
            }
        }

        _currentObjective = nextObjective;

        if(_currentObjective == null)
        {
            _completed = true;
        }
    }

    protected override void OnInteract()
    {
        if(_completed)
        {
            return;
        }

        if(InventoryManager.Instance.HasItem(_currentObjective.requiredItem))
        {
            _currentObjective.completed = true;
            _currentObjective.keyObject.TurnOn();
            
            int index = objectives.IndexOf(_currentObjective);
            SaveLoadManager.Instance.Data.CompletedMainObjectiveIndex = index;
            SaveLoadManager.Instance.Save();

            index++;

            if (index >= objectives.Count)
            {
                _currentObjective = null;
                _completed = true;
                AudioManager.Instance.PlaySound(completeSFX, transform.position, transform, false, completeVolume);
            }
            else
            { 
                _currentObjective = objectives[index];
                AudioManager.Instance.PlaySound(interactSFX, transform.position, transform, false, interactVolume);  
            }
        }
    }
}
