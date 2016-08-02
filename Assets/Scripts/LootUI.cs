using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LootUI : MonoBehaviour 
{
    public List<GameObject> Slots = new List<GameObject>();
    public List<Item> Items = new List<Item>();
    public GameObject slots;
    ItemDatabase database = ItemDatabase.itemDatabase;
    public int amountOfItems;
    public int maxOfItems;
    GameController gC;

    public List<int> DroppableItems = new List<int>();

    int x = -315;
    int y = 90;

    public GameObject tooltip;

    void Start()
    {
        GenerateAmount();
        int slotAmount = 0;
        gC = GameController.controller;
        database = ItemDatabase.itemDatabase;

        for (int i = 1; i < amountOfItems + 1; i++)
        {
                GameObject slot = (GameObject)Instantiate(slots);
                slot.GetComponent<LootSlot>().slotNumber = slotAmount;
                Slots.Add(slot);
                Items.Add(new Item());
                slot.name = "Slot " + i;
                slot.transform.SetParent(this.gameObject.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                x = x + 70;
                if (i == 10)
                {
                    x = -315;
                    y = y - 70;
                }

                slotAmount++;
        }

        SelectDrop();
    }

    void GenerateAmount()
    {
        amountOfItems = Random.Range(1, maxOfItems);
    }

    void SelectDrop()
    {
        List<int> itemIndex = new List<int>();
        for (int i = 0; i < amountOfItems; i++)
        {
            itemIndex.Add(Random.Range(1, DroppableItems.Count));
            AddItem(DroppableItems[itemIndex[i]-1]);
        }
    }

    void AddItem(int id)
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

    void AddItemAtEmptySlot(Item item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].itemName == null)
            {
                Items[i] = item;
                PlayerInventory.playerInv.AddItem(item.itemId);

                break;
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

    public void ShowTooltip(Vector3 toolPos, Item item)
    {
        tooltip.SetActive(true);
        tooltip.GetComponent<RectTransform>().localPosition = new Vector3(toolPos.x + 180, toolPos.y - 70, toolPos.z);

        tooltip.transform.GetChild(0).GetComponent<Text>().text = item.displayName;
        tooltip.transform.GetChild(1).GetComponent<Text>().text = item.itemDesc;
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
