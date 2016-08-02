using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
    public int itemId;
    public string itemName;
    public string displayName;
    public string itemDesc;
    [System.NonSerialized] public Sprite itemIcon;
    public GameObject itemModel;
    public int itemQuantity;
    public ItemType itemType;
    
    public enum ItemType
    {
        None,
        Consumable,
        Hand,
        Offhand,
        Head,
        Chest,
        Legs,
        Feet
    }

    public Item(int id, string name, string disp, string desc, int quantity, ItemType type)
    {
        itemId = id;
        itemName = name;
        displayName = disp;
        itemDesc = desc;
        itemQuantity = quantity;
        itemType = type;
        itemIcon = Resources.Load<Sprite>("Icons/" + name);
        itemModel = Resources.Load<GameObject>("Prefabs/Items/" + type + "/" + name);
    }

    public Item()
    {
        
    }
}
