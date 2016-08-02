using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour 
{
    public bool hasOwner;
    public WeaponManager.WeaponType weaponType;
    public WeaponManager parentControl;
    public bool equipped;
    
    public float attackSpeed;
    public float damage;

    [Header("Positions")]
    public Vector3 equipPosition;
    public Vector3 equipRotation;
    public Vector3 holsterPosition;
    public Vector3 holsterRotation;

    public enum HolsterBone
    {
        RightHip,
        Chest
    }
    public HolsterBone holsterBone;

    void Start()
    {
        parentControl = GetComponentInParent<WeaponManager>();

        if(parentControl)
        {
            parentControl.WeaponList.Add(this.gameObject);
        }
    }

    void Update()
    {
        if(equipped)
        {
            transform.parent = transform.GetComponentInParent<WeaponManager>().transform.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand);
            transform.localPosition = equipPosition;
            transform.localRotation = Quaternion.Euler(equipRotation);
            GetComponent<WeaponCollision>().enabled = true;
        }
        else
        {
            if(hasOwner)
            {
                switch(holsterBone)
                {
                    case HolsterBone.RightHip:
                        transform.parent = transform.GetComponentInParent<WeaponManager>().transform.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);
                        break;
                    case HolsterBone.Chest:
                        transform.parent = transform.GetComponentInParent<WeaponManager>().transform.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Chest);
                        break;
                }

                GetComponent<WeaponCollision>().enabled = false;
                transform.localPosition = holsterPosition;
                transform.localRotation = Quaternion.Euler(holsterRotation);
            }
        }
    }
}
