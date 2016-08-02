using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponCollision : MonoBehaviour 
{
    bool enemy;
    public bool hitAlready;
    PlayerController _PC;
    EnemyController _EC;
    PlayerStats pStats;
    GameManager gM;
    float damageMultiplier = 0;

    [Header("Blood")]
    public GameObject bloodDrop;

    void Start()
    {
        pStats = GetComponentInParent<PlayerStats>();
        if(GameObject.FindGameObjectWithTag("GameManager"))
        {
            gM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        if(pStats.enemy)
        {
            enemy = true;
        }
        else
        {
            enemy = false;
        }

        if(!enemy)
        {
            _PC = GetComponentInParent<PlayerController>();
        }
        else
        {
            _EC = GetComponentInParent<EnemyController>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        #region weaponhit
        if (other.gameObject.tag == "Weapon")
        {
            if(!enemy)
            {
                if(other.GetComponentInParent<EnemyController>())
                {
                    EnemyController enemyC = other.GetComponentInParent<EnemyController>();

                    if (_PC.holdAttack || _PC.attack)
                    {
                        if (enemyC.isBlocking && enemyC.transform.GetComponent<PlayerStats>().characterID != pStats.characterID)
                        {
                            hitAlready = true;
                            _PC.blockedAttack = true;
                        }
                        else if (enemyC.isBlocking && _PC.attack)
                        {
                            hitAlready = true;
                            _PC.blockedAttack = true;
                        }
                    }
                }
                else
                {
                    PlayerController enemyC = other.GetComponentInParent<PlayerController>();
                    if (enemyC != null && enemyC.isBlocking && _PC.attack && enemyC.transform.GetComponent<PlayerStats>().characterID != pStats.characterID)
                    {
                        _PC.blockedAttack = true;
                        hitAlready = true;
                    }
                }
            }
            else
            {
                if (other.GetComponentInParent<EnemyController>())
                {
                    EnemyController enemyC = other.GetComponentInParent<EnemyController>();

                    if (enemyC.isBlocking && _EC.holdAttack && _EC.attackType == 0 && enemyC.transform.GetComponent<PlayerStats>().characterID != pStats.characterID)
                    {
                        _EC.blockedAttack = true;
                        hitAlready = true;
                    }
                    else if (enemyC.isBlocking && _EC.attack && enemyC.transform.GetComponent<PlayerStats>().characterID != pStats.characterID)
                    {
                        _EC.blockedAttack = true;
                        hitAlready = true;
                    }
                }
                else
                {
                    PlayerController enemyC = other.GetComponentInParent<PlayerController>();
                    if (enemyC != null)
                    {
                        if (enemyC.isBlocking && _EC.attack)
                        {
                            _EC.blockedAttack = true;
                            hitAlready = true;
                        }
                    }
                }
            }
        }
        #endregion
        else if(other.transform.GetComponentInParent<PlayerStats>() && !hitAlready)
        {
            if(!enemy)
            {
                if (_PC != null)
                {
                    if (_PC.attack && !_PC.blockedAttack)
                    {
                        PlayerStats eS = other.transform.GetComponentInParent<PlayerStats>();

                        if (eS != pStats && !hitAlready && !_PC.blockedAttack && eS.characterID != pStats.characterID && !eS.dead && !hitAlready)
                        {
                            if (other.name == "mixamorig:Head")
                            {
                                damageMultiplier = 1.25f;
                            }
                            else if (other.name == "mixamorig:Spine1")
                            {
                                damageMultiplier = 0.85f;
                            }
                            else if (other.name == "mixamorig:Hips")
                            {
                                damageMultiplier = 0.95f;
                            }
                            else if (other.name == "mixamorig:LeftArm")
                            {
                                damageMultiplier = 0.75f;
                            }
                            else if (other.name == "mixamorig:RightArm")
                            {
                                damageMultiplier = 0.75f;
                            }
                            else if (other.name == "mixamorig:LeftForeArm")
                            {
                                damageMultiplier = 0.7f;
                            }
                            else if (other.name == "mixamorig:RightForeArm")
                            {
                                damageMultiplier = 0.7f;
                            }
                            else if (other.name == "mixamorig:LeftUpLeg")
                            {
                                damageMultiplier = 0.75f;
                            }
                            else if (other.name == "mixamorig:RightUpLeg")
                            {
                                damageMultiplier = 0.75f;
                            }
                            else if (other.name == "mixamorig:LeftLeg")
                            {
                                damageMultiplier = 0.7f;
                            }
                            else if (other.name == "mixamorig:RightLeg")
                            {
                                damageMultiplier = 0.7f;
                            }
                            eS.Damage(transform.GetComponentInParent<WeaponManager>().activeWeapon.damage, damageMultiplier, other.name);
                            other.GetComponentInParent<Animator>().SetTrigger("Hit");
                            GameObject blood = Instantiate(bloodDrop, other.transform.position, other.transform.rotation) as GameObject;
                            if (eS.dead)
                            {
                                eS = null;
                            }
                            hitAlready = true;
                        }
                    }
                }
            }
            else
            {
                if(_EC.attack && !_EC.blockedAttack)
                {
                    PlayerStats eS = other.transform.GetComponentInParent<PlayerStats>();

                    if (eS != pStats && !hitAlready && !_EC.blockedAttack && !eS.dead && pStats.characterID != eS.characterID)
                    {
                        if (other.name == "mixamorig:Head")
                        {
                            damageMultiplier = 1.25f;
                        }
                        else if (other.name == "mixamorig:Spine1")
                        {
                            damageMultiplier = 0.85f;
                        }
                        else if (other.name == "mixamorig:Hips")
                        {
                            damageMultiplier = 0.95f;
                        }
                        else if (other.name == "mixamorig:LeftArm")
                        {
                            damageMultiplier = 0.75f;
                        }
                        else if (other.name == "mixamorig:RightArm")
                        {
                            damageMultiplier = 0.75f;
                        }
                        else if (other.name == "mixamorig:LeftForeArm")
                        {
                            damageMultiplier = 0.7f;
                        }
                        else if (other.name == "mixamorig:RightForeArm")
                        {
                            damageMultiplier = 0.7f;
                        }
                        else if (other.name == "mixamorig:LeftUpLeg")
                        {
                            damageMultiplier = 0.75f;
                        }
                        else if (other.name == "mixamorig:RightUpLeg")
                        {
                            damageMultiplier = 0.75f;
                        }
                        else if (other.name == "mixamorig:LeftLeg")
                        {
                            damageMultiplier = 0.7f;
                        }
                        else if (other.name == "mixamorig:RightLeg")
                        {
                            damageMultiplier = 0.7f;
                        }
                        hitAlready = true;
                        eS.Damage(transform.GetComponentInParent<WeaponManager>().activeWeapon.damage, damageMultiplier, other.name);
                        other.GetComponentInParent<Animator>().SetTrigger("Hit");
                        GameObject blood = Instantiate(bloodDrop, other.transform.position, other.transform.rotation) as GameObject;
                        if (eS.dead)
                        {
                            eS = null;
                        }
                    }
                }
            }
        }
    }

}
