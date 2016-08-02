using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemDatabase : MonoBehaviour 
{
    public static ItemDatabase itemDatabase;
    public List<Item> items = new List<Item>();

    void Awake()
    {
        if (itemDatabase == null)
        {
            DontDestroyOnLoad(gameObject);
            itemDatabase = this;
        }
        else if (itemDatabase != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        items.Add(new Item());
        items.Add(new Item(1, "Sword_01", "Simple sword", "Simple meteal sword", 1, Item.ItemType.Hand));
        items.Add(new Item(2, "Sword_1", "Training sword", "Wooden sword used for training", 1, Item.ItemType.Hand));
        items.Add(new Item(3, "Sword_1", "Test", "Test", 1, Item.ItemType.Hand));
        items.Add(new Item(4, "Sword_1", "Test2", "Test", 1, Item.ItemType.Hand));
        items.Add(new Item(5, "Sword_1", "Test3", "Test", 1, Item.ItemType.Hand));
        items.Add(new Item(6, "Sword_1", "Test4", "Test", 1, Item.ItemType.Hand));
        items.Add(new Item(7, "LinenShirt_02", "Linen Shirt", "Casual linen shirt", 1, Item.ItemType.Chest));
    }

    //items.Add(new Item());
}
