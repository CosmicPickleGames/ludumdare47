using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string CurrentScene;
    public int LastExitDirection;
    public InventoryManager.InventorySerializable Inventory = new InventoryManager.InventorySerializable();
    public List<string> DestroyedInteractables = new List<string>();
    public List<string> ShownTriggers = new List<string>();

    [Header("Objectives")]
    public int CompletedMainObjectiveIndex = -1;

    [Header("Abilities")]
    public int numAirJumps;
    public bool dashEnabled;
    public int numAirDashes;
}

[DefaultExecutionOrder(-1000)]
public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public SaveData Data;

    private QuickSaveReader _quickSaveReader;
    private QuickSaveWriter _quickSaveWriter;

    public static void ResetSaves()
    {
        var quickSaveWriter = QuickSaveWriter.Create("MainSave");
        quickSaveWriter.Delete("SaveData");
        quickSaveWriter.Commit();
    }

    public void Init()
    {
        _quickSaveWriter = QuickSaveWriter.Create("MainSave");
        _quickSaveWriter.Commit();

        _quickSaveReader = QuickSaveReader.Create("MainSave");
        
        Load();
    }

    public void Load()
    {
        if(_quickSaveReader.TryRead<SaveData>("SaveData", out SaveData savedData))
        {
            Data = savedData;
        }
        else
        {
            Data = new SaveData();
        }
    }

    public void Save()
    {
        _quickSaveWriter.Write<SaveData>("SaveData", Data).Commit();
    }
}
