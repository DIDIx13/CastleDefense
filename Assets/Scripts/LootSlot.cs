using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public int slotNumber;
    public LootUI lootScript;

    Text itemAmount;
    Image itemIcon;

    void Start()
    {
        lootScript = GameObject.FindGameObjectWithTag("LootUI").GetComponent<LootUI>();
        itemAmount = transform.GetChild(1).GetComponent<Text>();
        itemIcon = transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if (lootScript.Items[slotNumber].itemName != null)
        {
            itemAmount.enabled = false;
            itemIcon.enabled = true;
            itemIcon.sprite = lootScript.Items[slotNumber].itemIcon;

            if (lootScript.Items[slotNumber].itemType == Item.ItemType.Consumable)
            {
                itemAmount.enabled = true;
                itemAmount.text = "" + lootScript.Items[slotNumber].itemQuantity;
            }
        }
        else
        {
            itemIcon.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (lootScript.Items[slotNumber].itemName != null)
        {
            lootScript.ShowTooltip(lootScript.Slots[slotNumber].GetComponent<RectTransform>().localPosition, lootScript.Items[slotNumber]);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (lootScript.Items[slotNumber].itemName != null)
        {
            lootScript.HideTooltip();
        }
    }
}
