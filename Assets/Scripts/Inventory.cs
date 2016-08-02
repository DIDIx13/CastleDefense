using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour 
{
    public static Inventory inventory;

    public List<GameObject> Slots = new List<GameObject>();
    public List<Item> Items = new List<Item>();
    public GameObject slots;
    ItemDatabase database = ItemDatabase.itemDatabase;
    PlayerInventory playerInv;

    int x = -170;
    int y = 165;

    public GameObject tooltip;
    public GameObject dragItemIcon;
    public bool draggingItem = false;
    public Item draggedItem;
    public int draggedIndex;

    public GameObject canvas;

    void Awake()
    {
        if (inventory == null)
        {
            DontDestroyOnLoad(gameObject);
            inventory = this;
        }
        else if (inventory != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int slotAmount = 0;
        database = ItemDatabase.itemDatabase;
        playerInv = PlayerInventory.playerInv;

        for(int i = 1; i < 8; i++)
        {
            for(int k = 1; k < 8; k++)
            {
                GameObject slot = (GameObject)Instantiate(slots);
                slot.GetComponent<SlotScript>().slotNumber = slotAmount;
                Slots.Add(slot);
                Items.Add(new Item());
                slot.name = "Slot " + i + "." + k;
                slot.transform.SetParent(this.gameObject.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                x = x + 55;
                if(k == 7)
                {
                    x = -170;
                    y = y - 55;
                }

                slotAmount++;
            }
        }

        for (int i = 0; i < playerInv.Items.Count; i++)
        {
            AddItem(playerInv.Items[i].itemId);
        }
    }

    void FixedUpdate()
    {
        if (draggingItem)
        {
            Vector3 mousePos = (Input.mousePosition - canvas.GetComponent<RectTransform>().position);
            dragItemIcon.GetComponent<RectTransform>().position = new Vector3(mousePos.x + 600, mousePos.y + 300, mousePos.z);
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

                break;
            }
        }
    }

    public void ShowTooltip(Vector3 toolPos, Item item)
    {
        tooltip.SetActive(true);
        tooltip.GetComponent<RectTransform>().localPosition = new Vector3(toolPos.x + 400, toolPos.y + 120, toolPos.z);

        tooltip.transform.GetChild(0).GetComponent<Text>().text = item.displayName;
        tooltip.transform.GetChild(1).GetComponent<Text>().text = item.itemDesc;
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void ShowDraggedItem(Item item, int slotNumber)
    {
        draggedIndex = slotNumber;
        dragItemIcon.SetActive(true);
        draggedItem = item;
        draggingItem = true;
        dragItemIcon.GetComponent<Image>().sprite = item.itemIcon;
    }

    public void HideDraggedItem()
    {
        draggingItem = false;
        dragItemIcon.SetActive(false);
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

    public void AddExistingItem(Item item)
    {
        if (item.itemType == Item.ItemType.Consumable)
        {
            CheckConsumable(item.itemId, item);
        }
        else
        {
            AddItemAtEmptySlot(item);
        }
    }
}
