using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour 
{
    public static PlayerInventory playerInv;
    public List<Item> Items = new List<Item>();
    ItemDatabase database = ItemDatabase.itemDatabase;

    void Awake()
    {
        if (playerInv == null)
        {
            DontDestroyOnLoad(gameObject);
            playerInv = this;
        }
        else if (playerInv != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        database = ItemDatabase.itemDatabase;

        for (int i = 1; i < 8; i++)
        {
            for (int k = 1; k < 8; k++)
            {
                Items.Add(new Item());
            }
        }

        GameController.controller.LoadInventory();
    }

    public void AddItem(int id)
    {
        for (int k = 0; k < database.items.Count; k++)
        {
            if (database.items[k].itemId == id)
            {
                Item item = database.items[k];

                if (database.items[k].itemType == Item.ItemType.Consumable)
                {
                    CheckConsumable(id, item);

                    break;
                }
                else
                {
                    AddItemAtEmptySlot(item);
                }
            }
        }
    }

    public void CheckConsumable(int itemId, Item item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].itemId == itemId)
            {
                Items[i].itemQuantity = Items[i].itemQuantity + item.itemQuantity;
                break;
            }
            else if (i == Items.Count - 1)
            {
                AddItemAtEmptySlot(item);
            }
        }
    }

    void AddItemAtEmptySlot(Item item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].itemName == null)
            {
                Items[i] = item;

                break;
            }
        }
    }
}
