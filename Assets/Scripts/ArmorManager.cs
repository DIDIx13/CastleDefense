using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorManager : MonoBehaviour 
{
    public List<GameObject> items = new List<GameObject>();
    public float armorHead = 1;
    public float armorChest = 1;
    public float armorLegs = 1;

    void Update()
    {
        CalculateArmor();
    }

    public void CalculateArmor()
    {
        foreach(GameObject item in items)
        {
            ArmorController aC = item.GetComponent<ArmorController>();
            if(aC.type == ArmorController.ArmorType.Head)
            {
                armorHead = aC.armorValue;
            }
            else if(aC.type == ArmorController.ArmorType.Chest)
            {
                armorChest = aC.armorValue;
            }
            else if(aC.type == ArmorController.ArmorType.Legs)
            {
                armorLegs = aC.armorValue;
            }
        }
    }
}
