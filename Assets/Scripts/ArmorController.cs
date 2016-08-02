using UnityEngine;
using System.Collections;

public class ArmorController : MonoBehaviour 
{
    public float armorValue;
    public ArmorType type;
    ArmorManager aM;

    void Start()
    {
        aM = GetComponentInParent<ArmorManager>();
        aM.items.Add(this.gameObject);
    }

    public enum ArmorType
    {
        Head,
        Chest,
        Legs
    }
}
