using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewInvetoryItem", menuName = "LudumDare47/InventoryItem", order = 0)]
public class InventoryItem : ScriptableObject
{
    public enum ItemType
    {
        Key,
        Clue
    }
    public string itemName;
    public Sprite itemIcon;
    public ItemType type;

    [System.Serializable]
    public class InventoryItemSerializable
    {
        public string itemName;
        public ItemType type;
    }

    public virtual InventoryItemSerializable GetSerializableItem()
    {
        return new InventoryItemSerializable
        {
            itemName = itemName,
            type = type,
        };
    }
}
