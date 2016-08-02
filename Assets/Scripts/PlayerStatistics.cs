using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerStatistics : MonoBehaviour 
{
    public static PlayerStatistics playerStatistics;

    [Header("Points")]
    public int statPoints = 5;
    public int strength = 5;
    public int endurance = 5;
    public int agility = 5;

    [Header("Stats")]
    public int skillPoints = 5;
    public float experience;
    public float experienceToNext;
    public int level;
    public float health = 100;
    public int oneHanded = 10;
    public int twoHanded = 10;
    public int shielding = 10;

    void Awake()
    {
        if (playerStatistics == null)
        {
            DontDestroyOnLoad(gameObject);
            playerStatistics = this;
        }
        else if (playerStatistics != this)
        {
            Destroy(gameObject);
        }
    }
}
