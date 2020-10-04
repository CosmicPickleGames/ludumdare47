using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DefaultExecutionOrder(-700)]
public class InventoryManager : Singleton<InventoryManager>
{
    public InventoryItemsLibrary library;
    public List<InventoryItem> items = new List<InventoryItem>();

    public delegate void OnAddItem(InventoryItem item);
    public OnAddItem onAddItem;

    [System.Serializable]
    public class InventorySerializable
    {
        public List<InventoryItem.InventoryItemSerializable> items = new List<InventoryItem.InventoryItemSerializable>();

        public List<InventoryItem> GetItems(InventoryItemsLibrary library)
        {
            List<InventoryItem> deserializedItems = new List<InventoryItem>();
            foreach (var item in items)
            {
                deserializedItems.Add(library.GetItem(item.itemName, item.type));
            }
            return deserializedItems;
        }
    }

    public InventorySerializable GetSerializableInventory()
    {
        InventorySerializable inv = new InventorySerializable
        {
            items = new List<InventoryItem.InventoryItemSerializable>(),
        };

        foreach(var item in items)
        {
            inv.items.Add(item.GetSerializableItem());
        }

        return inv;
    }

    public void Init()
    {
        items = SaveLoadManager.Instance.Data.Inventory.GetItems(library);
    }

    public void Add(InventoryItem item)
    {
        items.Add(item);
        SaveLoadManager.Instance.Data.Inventory = GetSerializableInventory();
        SaveLoadManager.Instance.Save();
        onAddItem?.Invoke(item);
    }

    public bool HasItem(InventoryItem item)
    {
        return items.Contains(item);
    }

    public void ResetInventory()
    {
        items = new List<InventoryItem>();
        SaveLoadManager.Instance.Data.Inventory = new InventorySerializable();
        SaveLoadManager.Instance.Save();
    }
}
