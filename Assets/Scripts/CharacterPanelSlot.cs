using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterPanelSlot : MonoBehaviour, IDropHandler, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    public Item item;
    public Inventory inventory;
    PlayerController player;
    PlayerEquipment playerEq;
    

    void Start()
    {
        inventory = Inventory.inventory;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerEq = PlayerEquipment.playerEquipment;

        WhenGameLoaded();
    }

    void Update()
    {
        if (item.itemType != Item.ItemType.None)
        {
            transform.GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }

    public void OnDrop(PointerEventData data)
    {
        Repositioning();
    }

    public void OnPointerDown(PointerEventData data)
    {
        Repositioning();
    }

    public void OnDrag(PointerEventData data)
    {
        if (item.itemType != Item.ItemType.None)
        {
            inventory.draggedItem = item;
            playerEq.RemoveEquippedItem(item);
            playerEq.RemovePlayerEquipment(item);
            inventory.ShowDraggedItem(item, -1);
            item = new Item();
        }
    }

    void WhenGameLoaded()
    {
        if (index == 0)
        {
            CheckIfHeadLoaded();
        }

        if (index == 1 )
        {
            CheckIfChestLoaded();
        }

        if (index == 2)
        {
            CheckIfLegsLoaded();
        }

        if (index == 3)
        {
            CheckIfFeetLoaded();
        }

        if (index == 4)
        {
            CheckIfHandLoaded();
        }

        if (index == 5)
        {
            CheckIfOffhandLoaded();
        }
    }

    void CheckIfHandLoaded()
    {
        if (playerEq.hand.itemId != 0)
        {
            item = playerEq.hand;
        }
    }

    void CheckIfChestLoaded()
    {
        if(playerEq.chest.itemId != 0)
        {
            item = playerEq.chest;
        }
    }

    void CheckIfHeadLoaded()
    {
        if (playerEq.head.itemId != 0)
        {
            item = playerEq.head;
        }
    }

    void CheckIfLegsLoaded()
    {
        if (playerEq.legs.itemId != 0)
        {
            item = playerEq.legs;
        }
    }

    void CheckIfOffhandLoaded()
    {
        if (playerEq.offhand.itemId != 0)
        {
            item = playerEq.offhand;
        }
    }

    void CheckIfFeetLoaded()
    {
        if (playerEq.feet.itemId != 0)
        {
            item = playerEq.feet;
        }
    }

    void Repositioning()
    {
        if(inventory.draggingItem)
        {
            if (index == 0 && inventory.draggedItem.itemType == Item.ItemType.Head)
            {
                if (item.itemType != Item.ItemType.None)
                {
                    Item temp = item;
                    item = inventory.draggedItem;
                    inventory.draggedItem = temp;
                    inventory.ShowDraggedItem(temp, -1);

                }
                else
                {
                    item = inventory.draggedItem;
                    playerEq.EquipItem(item);
                    inventory.HideDraggedItem();
                }
            }

            if (index == 1 && inventory.draggedItem.itemType == Item.ItemType.Chest)
            {
                if (item.itemType != Item.ItemType.None)
                {
                    Item temp = item;
                    item = inventory.draggedItem;
                    inventory.draggedItem = temp;
                    inventory.ShowDraggedItem(temp, -1);

                }
                else
                {
                    item = inventory.draggedItem;
                    playerEq.EquipItem(item);
                    inventory.HideDraggedItem();
                }
            }

            if (index == 2 && inventory.draggedItem.itemType == Item.ItemType.Legs)
            {
                if (item.itemType != Item.ItemType.None)
                {
                    Item temp = item;
                    item = inventory.draggedItem;
                    inventory.draggedItem = temp;
                    inventory.ShowDraggedItem(temp, -1);

                }
                else
                {
                    item = inventory.draggedItem;
                    playerEq.EquipItem(item);
                    inventory.HideDraggedItem();
                }
            }

            if (index == 3 && inventory.draggedItem.itemType == Item.ItemType.Feet)
            {
                if (item.itemType != Item.ItemType.None)
                {
                    Item temp = item;
                    item = inventory.draggedItem;
                    inventory.draggedItem = temp;
                    inventory.ShowDraggedItem(temp, -1);

                }
                else
                {
                    item = inventory.draggedItem;
                    playerEq.EquipItem(item);
                    inventory.HideDraggedItem();
                }
            }

            if (index == 4 && inventory.draggedItem.itemType == Item.ItemType.Hand)
            {
                if (item.itemType != Item.ItemType.None)
                {
                    Item temp = item;
                    item = inventory.draggedItem;
                    inventory.draggedItem = temp;
                    inventory.ShowDraggedItem(temp, -1);

                }
                else
                {
                    item = inventory.draggedItem;
                    playerEq.EquipItem(item);
                    inventory.HideDraggedItem();
                }
            }

            if (index == 5 && inventory.draggedItem.itemType == Item.ItemType.Offhand)
            {
                if (item.itemType != Item.ItemType.None)
                {
                    Item temp = item;
                    item = inventory.draggedItem;
                    inventory.draggedItem = temp;
                    inventory.ShowDraggedItem(temp, -1);

                }
                else
                {
                    item = inventory.draggedItem;
                    playerEq.EquipItem(item);
                    inventory.HideDraggedItem();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(item.itemId != 0)
        {
            inventory.ShowTooltip(new Vector3(this.GetComponent<RectTransform>().localPosition.x-800, this.GetComponent<RectTransform>().localPosition.y, this.GetComponent<RectTransform>().localPosition.z), item);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        inventory.HideTooltip();
    }
}
