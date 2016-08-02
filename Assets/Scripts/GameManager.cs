using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour 
{
    public GameObject Player;
    public GameObject AiPrefab;
    public Transform enemySpawnPoint;
    public int totalPlayers;
    public int totalEnemies;
    Transform enemyGeneral;
    public PlayerUI playerUI;

    public List<Transform> CurrentPlayers = new List<Transform>();

    public List<Transform> CurrentAllies = new List<Transform>();

    public List<Transform> CurrentEnemies = new List<Transform>();

    bool finished = false;
    public float expGained = 0;

    void Awake()
    {
        CreatePlayerArmy();
        CreateInvasion();
    }

    void Update()
    {
        for (int i = 0; i < CurrentAllies.Count; i++)
        {
            if (CurrentAllies[i].GetComponent<PlayerStats>().dead)
            {
                CurrentAllies.Remove(CurrentAllies[i]);
            }
        }

        for (int i = 0; i < CurrentPlayers.Count - 1; i++)
        {
            if (CurrentPlayers[i].GetComponent<PlayerStats>().dead)
            {
                CurrentPlayers.Remove(CurrentPlayers[i]);
            }
        }

        for (int i = 0; i < CurrentEnemies.Count; i++)
        {
            if(CurrentEnemies[i].GetComponent<PlayerStats>().dead)
            {
                expGained += CurrentEnemies[i].GetComponent<PlayerStats>().experienceDrop;
                CurrentEnemies.Remove(CurrentEnemies[i]);
            }
        }

        if(Player.GetComponent<PlayerStats>().dead && !finished)
        {
            LevelFinished(true);
            finished = true;
        }

        if(CurrentEnemies.Count <= 0 && !Player.GetComponent<PlayerStats>().dead && !finished)
        {
            LevelFinished(false);
            finished = true;
        }
    }

    void CreatePlayerArmy()
    {
        int rows = 2;
        int columns = 0;

        for(int i = 0; i < totalPlayers - totalEnemies; i++)
        {
            Vector3 pos = Player.transform.position;
            pos.x += columns * 2;
            pos.z -= rows;

            GameObject aiPla = Instantiate(AiPrefab, pos, Player.transform.rotation) as GameObject;

            if(columns < 5)
            {
                columns++;
            }
            else
            {
                columns = 0;
                rows++;
            }

            aiPla.GetComponent<PlayerStats>().characterID = Player.GetComponent<PlayerStats>().characterID;
            aiPla.GetComponent<EnemyController>().generalCharacter = Player.transform;
            CurrentPlayers.Add(aiPla.transform);
            CurrentAllies.Add(aiPla.transform);
        }
    }

    void CreateInvasion()
    {
        int rows = 0;
        int columns = 0;

        for (int i = 0; i < totalEnemies; i++)
        {
            Vector3 pos = enemySpawnPoint.position;
            pos.x += columns * 2;
            pos.z -= rows;

            GameObject aiPla = Instantiate(AiPrefab, pos, enemySpawnPoint.rotation) as GameObject;

            if (columns < 5)
            {
                columns++;
            }
            else
            {
                columns = 0;
                rows++;
            }

            if(i == 0)
            {
                enemyGeneral = aiPla.transform;
            }

            aiPla.GetComponent<PlayerStats>().characterID = "Enemy";
            aiPla.GetComponent<EnemyController>().generalCharacter = enemyGeneral;
            CurrentPlayers.Add(aiPla.transform);
            CurrentEnemies.Add(aiPla.transform);
            aiPla.name = "Enemy ";
        }
    }

    public void RemoveCharacter(Transform ch)
    {
        if(CurrentPlayers.Contains(ch))
        {
            CurrentPlayers.Remove(ch);
        }
    }

    void LevelFinished(bool defeat)
    {
        if (!defeat)
        {
            playerUI.DisplayVictoryScreen(expGained);
        }
        else
        {
            playerUI.DisplayLossScreen(expGained);
        }
    }
}
