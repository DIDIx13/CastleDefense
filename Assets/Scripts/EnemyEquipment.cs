using UnityEngine;
using System.Collections;

public class EnemyEquipment : MonoBehaviour 
{
    ItemDatabase itemDb = ItemDatabase.itemDatabase;

    public Item head;
    public Item chest;
    public Item legs;
    public Item feet;
    public Item hand;
    public Item offhand;

    EnemyController player;

    void Start()
    {
        itemDb = ItemDatabase.itemDatabase;
        player = GetComponent<EnemyController>();
        LoadItems();
        Equip();
    }

    public void LoadItems()
    {
        head = itemDb.items[head.itemId];
        chest = itemDb.items[chest.itemId];
        legs = itemDb.items[legs.itemId];
        feet = itemDb.items[feet.itemId];
        hand = itemDb.items[hand.itemId];
        offhand = itemDb.items[offhand.itemId];
    }

    public void Equip()
    {
        if(head.itemId != 0)
        {
            GameObject spawnedItem = (GameObject)Instantiate(Resources.Load("Prefabs/Items/" + head.itemType + "/" + head.itemName));
            Vector3 localScale = spawnedItem.transform.localScale;
            spawnedItem.transform.SetParent(player.transform);
            spawnedItem.transform.localScale = localScale;
            spawnedItem.name = head.displayName;
        }

        if (chest.itemId != 0)
        {
            GameObject spawnedItem = (GameObject)Instantiate(Resources.Load("Prefabs/Items/" + chest.itemType + "/" + chest.itemName));
            Vector3 localScale = spawnedItem.transform.localScale;
            spawnedItem.transform.SetParent(player.transform);
            spawnedItem.transform.localPosition = Vector3.zero;
            spawnedItem.name = chest.displayName;
        }

        if (feet.itemId != 0)
        {
            GameObject spawnedItem = (GameObject)Instantiate(Resources.Load("Prefabs/Items/" + feet.itemType + "/" + feet.itemName));
            Vector3 localScale = spawnedItem.transform.localScale;
            spawnedItem.transform.SetParent(player.transform);
            spawnedItem.transform.localPosition = Vector3.zero;
            spawnedItem.name = feet.displayName;
        }

        if (legs.itemId != 0)
        {
            GameObject spawnedItem = (GameObject)Instantiate(Resources.Load("Prefabs/Items/" + legs.itemType + "/" + legs.itemName));
            Vector3 localScale = spawnedItem.transform.localScale;
            spawnedItem.transform.SetParent(player.transform);
            spawnedItem.transform.localPosition = Vector3.zero;
            spawnedItem.name = legs.displayName;
        }

        if (hand.itemId != 0)
        {
            GameObject spawnedItem = (GameObject)Instantiate(Resources.Load("Prefabs/Items/" + hand.itemType + "/" + hand.itemName));
            Vector3 localScale = spawnedItem.transform.localScale;
            spawnedItem.transform.SetParent(player.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand));
            spawnedItem.transform.localPosition = Vector3.zero;
            spawnedItem.name = hand.displayName;
        }

        if (offhand.itemId != 0)
        {
            GameObject spawnedItem = (GameObject)Instantiate(Resources.Load("Prefabs/Items/" + offhand.itemType + "/" + offhand.itemName));
            Vector3 localScale = spawnedItem.transform.localScale;
            spawnedItem.transform.SetParent(player.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftHand));
            spawnedItem.transform.localScale = localScale;
            spawnedItem.name = offhand.displayName;
        }
    }
}
