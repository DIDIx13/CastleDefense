using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
    Rigidbody rigidbody;
    Animator anim;

    public float speed = 3;
    public float turnSpeed = 5;
    public float jumpPower = 6;

    Vector3 directionPos;
    Vector3 lookPos;

    public Transform cam;

    CapsuleCollider capCol;

    float horizontal;
    float vertical;

    public PhysicMaterial zeroFriction;
    public PhysicMaterial maxFriction;

    public bool isEnabled = true;

    private bool isRunning = false;
    private bool isJumping = false;

    GameManager gm;
    
    WeaponCollision wepCol;
    WeaponManager wepMan;

    public bool canAttack;
    public bool holdAttack;    
    public bool attack;
    float aTimer;
    float targetValue;
    float curValue;
    public bool blockedAttack;
    public float attackTimer = 0.5f;

    bool stunned;
    float stunDuration = 0.5f;
    float currentStun;

    public bool canBlock;
    float blockCancelTimer = 0;
    public bool isBlocking = false;
    public bool hasShield = false;
    float blockSide = 0;
    public float currentBlockSide = 0;

    float mouseX;
    float mouseY;
    public int attackType = 0;
    public int currAttackType = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        capCol = GetComponent<CapsuleCollider>();
        cam = Camera.main.transform;
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
        {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gm.CurrentPlayers.Add(this.transform);
        }
        rigidbody = GetComponent<Rigidbody>();
        wepCol = GetComponentInChildren<WeaponCollision>();
        wepMan = GetComponent<WeaponManager>();
        SetupAnimator();
    }

    void SetupAnimator()
    {
        anim = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if(childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }

    void Update()
    {
        Ray ray = new Ray(cam.position, cam.forward);

        lookPos = ray.GetPoint(100);

        ControlAttackAnim();
        ControlBlockAnim();
        ControlStun();

        #region controlscrollbutton
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < -0.01f)
            {
                wepMan.ChangeWeapon(false);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0.01f)
            {
                wepMan.ChangeWeapon(true);
            }
        }
        #endregion
    }

    void FixedUpdate()
    {
        if (isEnabled)
        {
            rigidbody.isKinematic = false;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            rigidbody.AddForce((((transform.right * horizontal) + (transform.forward * vertical)) * speed / Time.deltaTime));

            if (Input.GetButton("Jump") && !isJumping)
            {
                rigidbody.velocity += jumpPower * Vector3.up;
                isJumping = true;
            }

            anim.SetFloat("ForwardSpeed", vertical, 0.1f, Time.deltaTime);
            anim.SetFloat("SidewardSpeed", horizontal, 0.1f, Time.deltaTime);
        }
        else
        {
            rigidbody.isKinematic = true;
            anim.SetFloat("ForwardSpeed", 0, 0.1f, Time.deltaTime);
            anim.SetFloat("SidewardSpeed", 0, 0.1f, Time.deltaTime);
        }
    }

    void HandleFriction()
    {
        if(horizontal == 0 && vertical == 0)
        {
            capCol.material = maxFriction;
        }
        else
        {
            capCol.material = zeroFriction;
        }
    }

    void ControlAttackAnim()
    {
        if(blockedAttack)
        {
            stunned = true;
            canAttack = false;

            targetValue = 0;
            holdAttack = false;
            attack = false;
            anim.SetBool("Attacking", false);
            aTimer = 0;
        }

        if (!holdAttack)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            
        }

        if(curValue == 0)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        float mouseLB = 0;

        if (isEnabled)
        {
            mouseLB = Input.GetAxis("Fire1");
        }
        else
        {
            mouseLB = 0f;
        }

        if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
        {
            if (mouseX < 0)
            {
                attackType = 1;
                currAttackType = attackType;
            }
            else
            {
                attackType = 2;
                currAttackType = attackType;
            }
        }
        else
        {
            if (mouseY < 0)
            {
                attackType = 0;
                currAttackType = attackType;
            }
            else
            {
                attackType = 3;
                currAttackType = attackType;
            }
            
        }

        anim.SetInteger("AttackType", attackType);

        if(!isRunning && !isBlocking && !anim.GetBool("Attacking") && canAttack)
        {
            if (mouseLB > 0.01f && canAttack)
            {
                holdAttack = true;
            }
        }

        if (mouseLB < 0.01f && canAttack)
        {
            if (holdAttack)
            {
                attack = true;
            }
        }

        if(holdAttack && !blockedAttack)
        {
            anim.SetBool("Attacking", true);

            if(attack)
            {
                aTimer += Time.deltaTime;

                
                targetValue = 1;

                if(aTimer >= attackTimer)
                {
                    holdAttack = false;
                    attack = false;
                    anim.SetBool("Attacking", false);
                    wepCol.hitAlready = false;
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
        float mouseRB = 0f;

        if (isEnabled)
        {
            mouseRB = Input.GetAxis("Fire2");
        }
        else
        {
            mouseRB = 0f;
        }

        if (!isBlocking && !holdAttack && canBlock)
        {
            if(Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
            {
                if(mouseX < 0)
                {
                    blockSide = 1;
                }
                else
                {
                    blockSide = 0.5f;
                }
            }
            else
            {
                if(mouseY < 0)
                {
                    blockSide = -0.5f;
                }
                else
                {
                    blockSide = 0;
                }
            }
        }

        if (!isRunning && !anim.GetBool("Attacking") && canBlock)
        {
            if (mouseRB > 0.01)
            {
                isBlocking = true;

                currentBlockSide = blockSide;
                anim.SetFloat("BlockSide", blockSide);
            }

            if (isBlocking && !anim.GetBool("Attacking"))
            {
                if (mouseRB < 0.01)
                {
                    isBlocking = false;
                }
            }
        }
        else if(!isRunning && anim.GetBool("Attacking"))
        {
            if(mouseRB > 0.01 && curValue < 0.75)
            {
                canBlock = false;
                attack = false;
                anim.SetBool("Attacking", false);
                holdAttack = false;

                /*if (mouseRB > 1)
                {
                    canBlock = true;
                    isBlocking = true;
                    currentBlockSide = blockSide;
                    anim.SetFloat("BlockSide", blockSide);
                }*/
            }
        }
        else
        {
            isBlocking = false;
        }

        if(!hasShield)
        {
            anim.SetBool("BlockingWeapon", isBlocking);
        }
        else
        {
            anim.SetBool("BlockingShield", isBlocking);
        }

        if(!canBlock)
        {
            blockCancelTimer += Time.deltaTime;
        }

        if(blockCancelTimer > 0.25)
        {
            canBlock = true;
            blockCancelTimer = 0;
        }
    }

    void ControlStun()
    {
        if (stunned)
        {
            currentStun += Time.deltaTime;
            blockedAttack = true;

            if (currentStun > stunDuration)
            {
                wepCol.hitAlready = false;
                currentStun = 0;
                blockedAttack = false;
                canAttack = true;
                stunned = false;
            }
        }
    }
}
