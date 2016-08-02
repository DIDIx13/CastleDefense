using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public float speed = 3;
    public float turnSpeed = 2;

    Animator anim;
    CapsuleCollider capCol;
    NavMeshAgent agent;
    PlayerStats enemStats;
    PlayerStats charStats;
    Rigidbody rigidbody;
    GameManager gm;
    WeaponCollision wepCol;

    List<Transform> currentEnemies = new List<Transform>();
    Transform currentAttackingEnemy;

    Vector3 lookPos;

    public bool holdAttack;
    public bool attack;
    float aTimer;
    public float attackTimer = 0.5f;
    public bool blockedAttack;
    public float decisionTimer;
    public float attackRate = 3;
    bool decideAttack;
    public int attackType = 0;
    float targetValue;
    float curValue;

    public bool ready = true;
    float currentStun;
    float stunDuration = 2f;
    bool stunned;

    public bool isBlocking = false;
    public bool hasShield = false;
    public float blockSide = 0;
    bool decideBlock;
    float blockingTimer = 0;
    bool blocked;
    float blockDuration;

    float mouseX;
    float mouseY;

    public enum AIState
    {
        Charge,
        Follow,
        Hold
    }
    
    public AIState aiState = AIState.Charge;

    public Transform generalCharacter;
    public Vector3 holdPos;

    void Start()
    {
        anim = GetComponent<Animator>();
        capCol = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        charStats = GetComponent<PlayerStats>();
        SetupAnimator();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        wepCol = GetComponentInChildren<WeaponCollision>();

        agent.stoppingDistance = charStats.attackRange;
        holdPos = generalCharacter.position;        
    }

    void SetupAnimator()
    {
        anim = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }

    void Update()
    {
        if (!GetComponent<PlayerStats>().dead)
        {
            ControlAttackAnim();
            ControlBlockAnim();
            ControlStun();
            CheckEnemies();

            if(currentAttackingEnemy == null)
            {
                if(currentEnemies.Count <= 0)
                {
                    Idle();
                }
                else
                {
                    FindTarget();
                }
            }

            switch (aiState)
            {
                case AIState.Charge:
                    if (currentEnemies.Count <= 0)
                    {
                        Idle();
                    }
                    else
                    {
                        FindTarget();
                    }
                    MoveToTarget();
                    break;
                case AIState.Hold:
                    HoldPosition();
                    break;
                case AIState.Follow:
                    FollowGeneral();
                    break;
            }
        }
    }

    void CheckEnemies()
    {
        for(int i = 0; i < currentEnemies.Count; i++)
        {
            if (currentEnemies[i].GetComponent<PlayerStats>().dead)
            {
                currentEnemies.Remove(currentEnemies[i]);
            }
        }
    }

    void FindTarget()
    {
        if(currentAttackingEnemy == null && gm.CurrentPlayers.Count > 0)
        {
            foreach(Transform tran in gm.CurrentPlayers)
            {
                if (tran.GetComponent<PlayerStats>().characterID != charStats.characterID && !tran.GetComponent<PlayerStats>().dead)
                {
                    if(!currentEnemies.Contains(tran))
                    {
                        currentEnemies.Add(tran);
                    }
                }
            }

            if (currentEnemies.Count > 0)
            {
                int ran = Random.Range(0, currentEnemies.Count - 1);

                currentAttackingEnemy = currentEnemies[ran];
            }
            else
            {
                holdPos = transform.position;
                aiState = AIState.Hold;
            }
        }
        else
        {
            if(enemStats == null)
            {
                enemStats = currentAttackingEnemy.GetComponent<PlayerStats>();
            }
            else
            {
                if(enemStats.health <= 0)
                {
                    enemStats = null;
                    currentAttackingEnemy = null;
                }
            }
        }
    }

    void MoveToTarget()
    {
        if (currentAttackingEnemy != null)
        {
            agent.SetDestination(currentAttackingEnemy.position);

            anim.SetFloat("ForwardSpeed", Mathf.Abs(agent.desiredVelocity.z) * 10, 0.1f, Time.deltaTime);

            float distance = Vector3.Distance(transform.position, currentAttackingEnemy.position);

            if (distance < charStats.attackRange)
            {
                Vector3 dir = currentAttackingEnemy.position - transform.position;
                dir.y = 0;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);

                if (!isEnemyAttacking())
                {
                    if(ready)
                    {
                        Attacking();

                    } 
                    else
                    {
                        Blocking();
                    }
                }
                else if (!decideAttack)
                {
                    Blocking();
                }
                else
                {
                    Attacking();
                }
            }
        }
    }

    bool isEnemyAttacking()
    {
        bool retVal = false;

        if (currentAttackingEnemy && !currentAttackingEnemy.GetComponent<PlayerStats>().dead)
        {
            Animator enAnim = currentAttackingEnemy.GetComponent<Animator>();

            if(currentAttackingEnemy.GetComponent<PlayerController>().isActiveAndEnabled)
            {
                if(currentAttackingEnemy.GetComponent<PlayerController>().holdAttack)
                {
                    PlayerController playContr = currentAttackingEnemy.GetComponent<PlayerController>();

                    retVal = playContr.holdAttack;
                }
                else
                {
                    retVal = enAnim.GetBool("Attacking");
                }
            }
            else if (currentAttackingEnemy.GetComponent<EnemyController>().isActiveAndEnabled)
            {
                retVal = enAnim.GetBool("Attacking");
            }
        }

        return retVal;
    }

    void Attacking()
    {
        isBlocking = false;
        decideBlock = false;
        anim.SetBool("BlockingWeapon", isBlocking);
        anim.SetBool("BlockingShield", isBlocking);

        if(!attack)
        {
            if(!decideAttack)
            {
                decisionTimer += Time.deltaTime;

                if(decisionTimer > (attackRate*2))
                {
                    int ran = Random.Range(0, 3);
                    attackType = ran;
                    anim.SetInteger("AttackType", attackType);
                    anim.SetBool("Attacking", true);
                    decideAttack = true;
                    decideBlock = false;
                    decisionTimer = 0;
                }
            }
            else
            {
                decisionTimer += Time.deltaTime;

                if(decisionTimer > attackRate)
                {
                    attack = true;
                    decisionTimer = 0;
                }
            }
        }
    }

    void Blocking()
    {
        attack = false;
        decideBlock = false;

        if(!attack)
        {
            if(hasShield)
            {
                decideBlock = true;
            }
            else
            {
                if(CheckEnemyAttack() == 0 && ready)
                {
                    blockSide = 0;
                }
                else if (CheckEnemyAttack() == 1 && ready)
                {
                    blockSide = 0.5f;
                }
                else if (CheckEnemyAttack() == 2 && ready)
                {
                    blockSide = 1;
                }
                else if (CheckEnemyAttack() == 3 && ready)
                {
                    blockSide = 0;
                }

                decideBlock = true;
            }
        }
    }

    float CheckEnemyAttack()
    {
        int retVal = 0;

        if(currentAttackingEnemy)
        {
            if(currentAttackingEnemy.GetComponent<PlayerController>().isActiveAndEnabled)
            {
                PlayerController playContr = currentAttackingEnemy.GetComponent<PlayerController>();
                retVal = playContr.currAttackType;
            }
            else if(currentAttackingEnemy.GetComponent<EnemyController>().isActiveAndEnabled)
            {
                EnemyController enemContr = currentAttackingEnemy.GetComponent<EnemyController>();
                retVal = enemContr.attackType;
            }
        }

        return(retVal);
    }

    void ControlAttackAnim()
    {
        if (blockedAttack)
        {
            stunned = true;

            targetValue = 0;
            holdAttack = false;
            anim.SetBool("Attacking", false);
            attack = false;
            decideAttack = false;
            aTimer = 0;
        }

        if (decideAttack && !blockedAttack)
        {
            anim.SetBool("Attacking", true);
            holdAttack = true;

            if (attack && !blockedAttack)
            {
                aTimer += Time.deltaTime;


                targetValue = 1;

                if (aTimer >= attackTimer)
                {
                    holdAttack = false;
                    decideAttack = false;
                    attack = false;
                    wepCol.hitAlready = false;
                    anim.SetBool("Attacking", false);
                    aTimer = 0;
                }
            }
            else
            {
                targetValue = 0;
            }
        }
        else
        {
            targetValue = 0;
        }

        curValue = Mathf.MoveTowards(curValue, targetValue, Time.deltaTime * 5f);
        anim.SetFloat("Attack", curValue);
    }

    void ControlBlockAnim()
    {
        if (currentAttackingEnemy != null)
        {
            if (!anim.GetBool("Attacking"))
            {
                if (decideBlock)
                {
                    isBlocking = true;
                    ready = false;

                    blockDuration += Time.deltaTime;
                    anim.SetFloat("BlockSide", blockSide);
                }

                if (isBlocking && !anim.GetBool("Attacking"))
                {
                    if (currentAttackingEnemy.GetComponent<PlayerController>().blockedAttack)
                    {
                        blocked = true;
                        blockDuration = 0;
                    }
                    else
                    {
                        if (!isEnemyAttacking() && blockDuration > 2)
                        {
                            blocked = true;
                        }
                    }
                }

                if (blocked)
                {
                    float howLong;
                    if (hasShield)
                    {
                        howLong = 1f;
                    }
                    else
                    {
                        howLong = 0.5f;
                    }

                    blockingTimer += Time.deltaTime;

                    if (blockingTimer > howLong)
                    {
                        isBlocking = false;
                        decideBlock = false;
                        ready = true;
                        blockingTimer = 0;
                        blocked = false;
                    }
                }
            }
            else
            {
                isBlocking = false;
                decideBlock = false;
            }

            if (!hasShield)
            {
                anim.SetBool("BlockingWeapon", isBlocking);
            }
            else
            {
                anim.SetBool("BlockingShield", isBlocking);
            }
        }
        else
        {
            FindTarget();
        }
    }

    void ControlStun()
    {
        if(stunned)
        {
            currentStun += Time.deltaTime;
            ready = false;
            blockedAttack = true;

            if(currentStun > stunDuration)
            {
                wepCol.hitAlready = false;
                currentStun = 0;
                ready = true;
                blockedAttack = false;
                stunned = false;
            }
        }
    }

    void FollowGeneral()
    {
        currentAttackingEnemy = null;
        agent.SetDestination(generalCharacter.position);
    }

    void HoldPosition()
    {
        currentAttackingEnemy = null;
        agent.SetDestination(holdPos);
    }

    void Idle()
    {
        currentAttackingEnemy = null;
        isBlocking = false;
        anim.SetBool("Attacking", false);
        anim.SetBool("BlockingWeapon", false);
        anim.SetBool("BlockingShield", false);
    }
}