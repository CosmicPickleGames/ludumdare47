using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewInvetoryItemLibrary", menuName = "LudumDare47/InventoryItemLibrary", order = 1)]
public class InventoryItemsLibrary : ScriptableObject
{
    public List<InventoryItem> items;

    public InventoryItem GetItem(string name, InventoryItem.ItemType type)
    {
        return items.Find(i => i.itemName == name && i.type == type);
    }
}
