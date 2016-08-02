using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour 
{
    public static GameController controller;
    public PlayerController player;
    public PlayerEquipment playerEq;
    PlayerStatistics pS;

    PlayerInventory inv;
    ItemDatabase db;

	void Awake()
    {
	    if(controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if(controller != this)
        {
            Destroy(gameObject);
        }
	}

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); ;
        playerEq = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerEquipment>();
        pS = PlayerStatistics.playerStatistics;
        inv = PlayerInventory.playerInv;
        db = ItemDatabase.itemDatabase;

        if (File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            LoadCharacter();
        }
        else
        {
            StartNewGame();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); ;
            playerEq = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerEquipment>();
            pS = PlayerStatistics.playerStatistics;
            inv = PlayerInventory.playerInv;
            db = ItemDatabase.itemDatabase;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); ;
            playerEq = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerEquipment>();
            pS = PlayerStatistics.playerStatistics;
            inv = PlayerInventory.playerInv;
            db = ItemDatabase.itemDatabase;
        }
    }

    void Update()
    {
        inv = PlayerInventory.playerInv;
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/player.dat");

        PlayerData data = new PlayerData();
        data.health = pS.health;
        data.strength = pS.strength;
        data.endurance = pS.endurance;
        data.agility = pS.agility;
        data.oneHanded = pS.oneHanded;
        data.twoHanded = pS.twoHanded;
        data.shielding = pS.shielding;
        data.statPoints = pS.statPoints;
        data.skillPoints = pS.skillPoints;
        data.experience = pS.experience;
        data.experienceToNext = pS.experienceToNext;
        data.level = pS.level;
        data.head = playerEq.head.itemId;
        data.chest = playerEq.chest.itemId;
        data.legs = playerEq.legs.itemId;
        data.feet = playerEq.feet.itemId;
        data.hand = playerEq.hand.itemId;
        data.offhand = playerEq.offhand.itemId;

        for (int i = 0; i < inv.Items.Count; i++)
        {
            data.items.Add(inv.Items[i].itemId);
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public void StartNewGame()
    {
        if (File.Exists(Application.persistentDataPath + "/reset.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/reset.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            pS.health = data.health;
            pS.strength = data.strength;
            pS.endurance = data.endurance;
            pS.agility = data.agility;
            pS.oneHanded = data.oneHanded;
            pS.twoHanded = data.twoHanded;
            pS.shielding = data.shielding;
            pS.statPoints = data.statPoints;
            pS.skillPoints = data.skillPoints;
            pS.experience = data.experience;
            pS.experienceToNext = data.experienceToNext;
            pS.level = data.level;
            playerEq.head.itemId = data.head;
            playerEq.chest.itemId = data.chest;
            playerEq.legs.itemId = data.legs;
            playerEq.feet.itemId = data.feet;
            playerEq.hand.itemId = data.hand;
            playerEq.offhand.itemId = data.offhand;
            playerEq.Reequip();
        }
    }

    public void LoadCharacter()
    {
        if (File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            pS.health = data.health;
            pS.strength = data.strength;
            pS.endurance = data.endurance;
            pS.agility = data.agility;
            pS.oneHanded = data.oneHanded;
            pS.twoHanded = data.twoHanded;
            pS.shielding = data.shielding;
            pS.statPoints = data.statPoints;
            pS.skillPoints = data.skillPoints;
            pS.experience = data.experience;
            pS.experienceToNext = data.experienceToNext;
            pS.level = data.level;
            playerEq.head.itemId = data.head;
            playerEq.chest.itemId = data.chest;
            playerEq.legs.itemId = data.legs;
            playerEq.feet.itemId = data.feet;
            playerEq.hand.itemId = data.hand;
            playerEq.offhand.itemId = data.offhand;
            playerEq.Reequip();
        }
    }

    public void LoadInventory()
    {
        if (File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            for(int i = 0; i < inv.Items.Count; i++)
            {
                inv.Items[i].itemId = data.items[i];
                inv.Items[i] = db.items[data.items[i]];
            }
        }
    }

    public void LoadGame()
    {
        if(File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            pS.health = data.health;
            pS.strength = data.strength;
            pS.endurance = data.endurance;
            pS.agility = data.agility;
            pS.oneHanded = data.oneHanded;
            pS.twoHanded = data.twoHanded;
            pS.shielding = data.shielding;
            pS.statPoints = data.statPoints;
            pS.skillPoints = data.skillPoints;
            pS.experience = data.experience;
            pS.experienceToNext = data.experienceToNext;
            pS.level = data.level;
            playerEq.head.itemId = data.head;
            playerEq.chest.itemId = data.chest;
            playerEq.legs.itemId = data.legs;
            playerEq.feet.itemId = data.feet;
            playerEq.hand.itemId = data.hand;
            playerEq.offhand.itemId = data.offhand;
        }
    }

    public bool IfSaveExists()
    {
        if(File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[Serializable]
class PlayerData
{
    public float health;
    public int strength;
    public int endurance;
    public int agility;
    public int oneHanded;
    public int twoHanded;
    public int shielding;
    public int skillPoints;
    public int statPoints;
    public float experience;
    public float experienceToNext;
    public int level;
    public int head;
    public int chest;
    public int legs;
    public int feet;
    public int hand;
    public int offhand;
    public List<int> items = new List<int>();
}
