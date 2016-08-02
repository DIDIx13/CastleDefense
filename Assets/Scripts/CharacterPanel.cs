using UnityEngine;
using System.Collections;

public class CharacterPanel : MonoBehaviour 
{
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(i).GetComponent<CharacterPanelSlot>().index = i;
        }
    }
}
