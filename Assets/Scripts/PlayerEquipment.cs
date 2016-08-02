using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment playerEquipment;

    public Item head;
    public Item chest;
    public Item legs;
    public Item feet;
    public Item hand;
    public Item offhand;

    ItemDatabase db;
    PlayerController player;

    public List<GameObject> SpawnedItems = new List<GameObject>();

    void Awake()
    {
        if (playerEquipment == null)
        {
            DontDestroyOnLoad(gameObject);
            playerEquipment = this;
        }
        else if (playerEquipment != this)
        {
            Destroy(gameObject);
        }
    }

    void OnLevelWasLoaded(int level)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (level != 0)
        {
            SpawnedItems.Clear();
            Reequip();
        }
        else
        {
            SpawnedItems.Clear();
            Reequip();
        }
    }

    void Start()
    {
        db = ItemDatabase.itemDatabase;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    
    public void Reequip()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (head.itemId != 0)
        {
            head = db.items[head.itemId];
            EquipItem(head);
        }

        if (chest.itemId != 0)
        {
            chest = db.items[chest.itemId];
            EquipItem(chest);
        }

        if (legs.itemId != 0)
        {
            legs = db.items[legs.itemId];
            EquipItem(legs);
        }

        if (feet.itemId != 0)
        {
            feet = db.items[feet.itemId];
            EquipItem(feet);
        }

        if (hand.itemId != 0)
        {
            hand = db.items[hand.itemId];
            EquipItem(hand);
        }

        if (offhand.itemId != 0)
        {
            offhand = db.items[offhand.itemId];
            EquipItem(offhand);
        }
    }

    public void DecidePlayerEquipment(Item item)
    {
        if (item.itemType == Item.ItemType.Head)
        {
            this.head = item;
        }
        else if (item.itemType == Item.ItemType.Chest)
        {
            this.chest = item;
        }
        else if (item.itemType == Item.ItemType.Legs)
        {
            this.legs = item;
        }
        else if (item.itemType == Item.ItemType.Feet)
        {
            this.feet = item;
        }
        else if (item.itemType == Item.ItemType.Hand)
        {
            this.hand = item;
        }
        else if (item.itemType == Item.ItemType.Offhand)
        {
            this.offhand = item;
        }
    }

    public void RemovePlayerEquipment(Item item)
    {
        if (item.itemType == Item.ItemType.Head)
        {
            head = new Item();
        }
        else if (item.itemType == Item.ItemType.Chest)
        {
            chest = new Item();
        }
        else if (item.itemType == Item.ItemType.Legs)
        {
            legs = new Item();
        }
        else if (item.itemType == Item.ItemType.Feet)
        {
            feet = new Item();
        }
        else if (item.itemType == Item.ItemType.Hand)
        {
            hand = new Item();
        }
        else if (item.itemType == Item.ItemType.Offhand)
        {
            offhand = new Item();
        }
    }

    public void EquipItem(Item newItem)
    {
        GameObject spawnedItem = (GameObject)Instantiate(Resources.Load("Prefabs/Items/" + newItem.itemType + "/" + newItem.itemName));

        Vector3 localScale = spawnedItem.transform.localScale;

        if(newItem.itemType == Item.ItemType.Hand)
        {
            spawnedItem.transform.SetParent(player.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand));
            spawnedItem.transform.localScale = localScale;
        }
        else if(newItem.itemType == Item.ItemType.Chest)
        {
            spawnedItem.transform.SetParent(player.transform);
            spawnedItem.transform.localPosition = Vector3.zero;
        }
        else if (newItem.itemType == Item.ItemType.Feet)
        {
            spawnedItem.transform.SetParent(player.transform);
            spawnedItem.transform.localPosition = Vector3.zero;
        }
        else if (newItem.itemType == Item.ItemType.Legs)
        {
            spawnedItem.transform.SetParent(player.transform);
            spawnedItem.transform.localPosition = Vector3.zero;
        }
        else if (newItem.itemType == Item.ItemType.Head)
        {
            spawnedItem.transform.SetParent(player.transform);
            spawnedItem.transform.localPosition = Vector3.zero;
        }
        else if (newItem.itemType == Item.ItemType.Offhand)
        {
            spawnedItem.transform.SetParent(player.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftHand));
            spawnedItem.transform.localScale = localScale;
            
        }

        spawnedItem.name = newItem.displayName;
        SpawnedItems.Add(spawnedItem);
        DecidePlayerEquipment(newItem);
    }

    public void RemoveEquippedItem(Item oldItem)
    {
        for (int i = 0; i < SpawnedItems.Count; i++)
        {
            if (SpawnedItems[i].name == oldItem.displayName)
            {
                if (SpawnedItems[i].GetComponent<WeaponController>())
                {
                    SpawnedItems[i].GetComponent<WeaponController>().parentControl.DropWeapon();
                }
                Destroy(SpawnedItems[i]);
                SpawnedItems.RemoveAt(i);
            }
        }
    }
}
