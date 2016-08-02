using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler
{
    public Item item;
    public int slotNumber;
    Inventory inventory = Inventory.inventory;

    Text itemAmount;
    Image itemIcon;

    void Start()
    {
        itemAmount = transform.GetChild(1).GetComponent<Text>();
        itemIcon = transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if (inventory.Items[slotNumber].itemName != null)
        {
            itemAmount.enabled = false;
            itemIcon.enabled = true;
            itemIcon.sprite = inventory.Items[slotNumber].itemIcon;

            if (inventory.Items[slotNumber].itemType == Item.ItemType.Consumable)
            {
                itemAmount.enabled = true;
                itemAmount.text = "" + inventory.Items[slotNumber].itemQuantity;
            }
        }
        else
        {
            itemIcon.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (inventory.Items[slotNumber].itemName != null && inventory.Items[slotNumber].itemType == Item.ItemType.Consumable && !inventory.draggingItem)
        {
            if (data.button == PointerEventData.InputButton.Right)
            {
                inventory.Items[slotNumber].itemQuantity--;
                if (inventory.Items[slotNumber].itemQuantity == 0)
                {
                    inventory.Items[slotNumber] = new Item();
                    itemAmount.enabled = false;
                    inventory.HideTooltip();
                }
            }
        }
        else if(inventory.Items[slotNumber].itemName == null && inventory.draggingItem)
        {
            inventory.Items[slotNumber] = inventory.draggedItem;
            inventory.HideDraggedItem();
        }
        else if (inventory.Items[slotNumber].itemName != null && inventory.draggingItem)
        {
            try
            {
                if (inventory.Items[slotNumber].itemName != null && inventory.draggingItem)
                {
                    inventory.Items[inventory.draggedIndex] = inventory.Items[slotNumber];
                    inventory.Items[slotNumber] = inventory.draggedItem;
                    inventory.HideDraggedItem();
                }
            }
            catch { }
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(inventory.Items[slotNumber].itemName != null && !inventory.draggingItem)
        {
            inventory.ShowTooltip(inventory.Slots[slotNumber].GetComponent<RectTransform>().localPosition, inventory.Items[slotNumber]);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (inventory.Items[slotNumber].itemName != null)
        {
            inventory.HideTooltip();
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (!inventory.draggingItem && data.button == PointerEventData.InputButton.Left)
        {
            if (inventory.Items[slotNumber].itemName != null)
            {
                inventory.ShowDraggedItem(inventory.Items[slotNumber], slotNumber);
                inventory.Items[slotNumber] = new Item();
                PlayerInventory.playerInv.Items[slotNumber] = new Item();
                inventory.HideTooltip();

                itemAmount.enabled = false;
            }
        }
    }

    public void OnDrop(PointerEventData data)
    {
        if (inventory.Items[slotNumber].itemName == null && inventory.draggingItem)
        {
            inventory.Items[slotNumber] = inventory.draggedItem;
            PlayerInventory.playerInv.Items[slotNumber] = inventory.draggedItem;
            inventory.HideDraggedItem();
        }
        else
        {
            try
            {
                if (inventory.Items[slotNumber].itemName != null && inventory.draggingItem)
                {
                    inventory.Items[inventory.draggedIndex] = inventory.Items[slotNumber];
                    inventory.Items[slotNumber] = inventory.draggedItem;
                    inventory.HideDraggedItem();
                }
            }
            catch { }
        }
    }
}
