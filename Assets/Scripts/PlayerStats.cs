using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour 
{
    public string characterID;
    public bool dead;
    public bool playerControlled = true;

    [Header("Points")]
    public int statPoints = 0;
    public int strength = 5;
    public int endurance = 5;
    public int agility = 5;

    [Header("Multipliers")]
    public float dmgMulti = 1;
    public float spdMulti = 1;

    [Header("Stats")]
    public int skillPoints = 0;
    public float experience;
    public float experienceToNext;
    public int level = 1;
    public float health = 100;
    public int oneHanded = 10;
    public int twoHanded = 10;
    public int shielding = 10;
    public float currentHealth;

    [Header("Enemy")]
    public bool enemy;
    public float attackRange = 3;
    public float experienceDrop;

    PlayerController playerController;
    CapsuleCollider capCol;
    Rigidbody rigidbody;
    Animator anim;
    FreeCameraLook camLook;
    RagdollManager ragMan;
    WeaponManager wepMan;
    ArmorManager arMan;
    GameManager gamMan;

    bool setHealth = true;

    void Awake()
    {
        if (!enemy)
        {
            LoadStats();
        }
        CalculateStats();
        CalculateExp();
    }

    void Start()
    {
        if(GetComponent<PlayerController>())
        {
            if(!GetComponent<PlayerController>().isActiveAndEnabled)
            {
                enemy = true;
                playerControlled = false;
            }
            else
            {
                playerControlled = true;
            }
        }

        playerController = GetComponent<PlayerController>();
        capCol = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ragMan = GetComponent<RagdollManager>();
        wepMan = GetComponent<WeaponManager>();
        arMan = GetComponent<ArmorManager>();
        gamMan = GetComponent<GameManager>();

        if(playerControlled && playerController.isEnabled)
        {
            camLook = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<FreeCameraLook>();
        }

        
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = -10;
            dead = true;
        }

        if(dead)
        {
            ragMan.RagdollPlayer();
            DisableComponents();
        }

        CalculateStats();
        CalculateExp();
    }

    public void LoadStats()
    {
        PlayerStatistics pS = PlayerStatistics.playerStatistics;
        strength = pS.strength;
        endurance = pS.endurance;
        agility = pS.agility;
        statPoints = pS.statPoints;
        oneHanded = pS.oneHanded;
        twoHanded = pS.twoHanded;
        shielding = pS.shielding;
        skillPoints = pS.skillPoints;
        experience = pS.experience;
        experienceToNext = pS.experienceToNext;
        level = pS.level;
    }

    void CalculateStats()
    {
        health = 90 + (endurance * 2);
        if(setHealth)
        {
            currentHealth = health;
            setHealth = false;
        }
        dmgMulti = 0.9f + (strength * 0.02f);
        spdMulti = 0.9f + (agility * 0.02f);
    }

    public void CalculateExp()
    {
        experienceToNext = 1000 + (1000 * level);
        if(experience >= experienceToNext)
        {
            LevelUp();
            SaveExp();
        }
    }

    public void SaveExp()
    {
        PlayerStatistics pS = PlayerStatistics.playerStatistics;
        pS.level = level;
        pS.experienceToNext = experienceToNext;
        pS.experience = experience;
        pS.skillPoints = skillPoints;
        pS.statPoints = statPoints;
    }

    void LevelUp()
    {
        float addExp = experience - experienceToNext;
        level++;
        experience = 0 + addExp;
        statPoints += 5;
        skillPoints += 5;
    }

    public void Damage(float dmg, float mtp, string name)
    {
        float protection = 1;

        if (name == "mixamorig:Head")
        {
            protection = arMan.armorHead;
        }
        else if (name == "mixamorig:Spine1" || name == "mixamorig:LeftArm" || name == "mixamorig:RightForeArm" || name == "mixamorig:LeftForeArm" || name == "mixamorig:RightArm")
        {
            protection = arMan.armorChest;
        }
        else if (name == "mixamorig:LeftLeg" || name == "mixamorig:RightLeg" || name == "mixamorig:RightUpLeg" || name == "mixamorig:LeftUpLeg" || name == "mixamorig:Hips")
        {
            protection = arMan.armorLegs;
        }
        else
        {
            protection = 1;
        }

        currentHealth -= (dmg * mtp) - protection;
    }

    void DisableComponents()
    {
        if(enemy)
        {
            transform.GetComponent<NavMeshAgent>().enabled = false;
            transform.GetComponent<CapsuleCollider>().enabled = false;
            transform.GetComponent<Animator>().enabled = false;
            wepMan.DropWeapon();
            transform.GetComponent<WeaponManager>().enabled = false;
        }
        else
        {
            transform.GetComponent<CapsuleCollider>().enabled = false;
            transform.GetComponent<PlayerController>().enabled = false;
            transform.GetComponent<Animator>().enabled = false;
            wepMan.DropWeapon();
            transform.GetComponent<WeaponManager>().enabled = false;
            FreeCameraLook cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<FreeCameraLook>();
            cameraHolder.enabled = false;
        }
    }
}
