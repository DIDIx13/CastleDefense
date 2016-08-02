using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WeaponManager : MonoBehaviour 
{
    public List<GameObject> WeaponList = new List<GameObject>();
    public WeaponController activeWeapon;
    int weaponNumber;
    bool enemy;
	
    public enum WeaponType
    {
        OneHandedSword,
        Shield,
    }

    public WeaponType weaponType;
    

    void Start()
    {
        enemy = GetComponent<PlayerStats>().enemy;

        if(WeaponList.Count > 0)
        {
            activeWeapon = WeaponList[weaponNumber].GetComponent<WeaponController>();
            activeWeapon.equipped = true;
        }

        foreach(GameObject go in WeaponList)
        {
            go.GetComponent<WeaponController>().hasOwner = true;
        }
    }

    void Update()
    {
        if (WeaponList.Count > 0)
        {
            activeWeapon = WeaponList[weaponNumber].GetComponent<WeaponController>();
            activeWeapon.equipped = true;
            weaponType = activeWeapon.weaponType;
        }

        if (!enemy)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropWeapon();
            }
        }

        if(activeWeapon == null)
        {
            if (WeaponList.Count > 0)
            {
                activeWeapon = WeaponList[weaponNumber].GetComponent<WeaponController>();
                activeWeapon.equipped = true;
                weaponType = activeWeapon.weaponType;
            }
        }
    }

    public void ChangeWeapon(bool ascending)
    {
        if (transform.GetComponent<PlayerController>().attack)
        {
            if (WeaponList.Count > 1)
            {
                activeWeapon.equipped = false;

                if (ascending)
                {
                    if (weaponNumber < WeaponList.Count - 1)
                    {
                        weaponNumber++;
                    }
                    else
                    {
                        weaponNumber = 0;
                    }
                }
                else
                {
                    if (weaponNumber > 0)
                    {
                        weaponNumber--;
                    }
                    else
                    {
                        weaponNumber = WeaponList.Count - 1;
                    }
                }
            }
        }
    }

    public void DropWeapon()
    {
        if (activeWeapon != null)
        {
            weaponNumber = 0;
            WeaponList.Remove(activeWeapon.gameObject);
            activeWeapon.hasOwner = false;
            activeWeapon.equipped = false;
            activeWeapon.transform.GetComponent<BoxCollider>().isTrigger = false;
            activeWeapon.transform.GetComponent<Rigidbody>().useGravity = true;
            activeWeapon.transform.GetComponent<Rigidbody>().isKinematic = false;
            activeWeapon.transform.parent = null;
            WeaponList.Remove(activeWeapon.gameObject);
        }
    }
}
