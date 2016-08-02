using UnityEngine;
using System.Collections;

public class MapMenu : MonoBehaviour 
{
    public static MapMenu mapMenu;

    void Awake()
    {
        if(mapMenu == null)
        {
            DontDestroyOnLoad(gameObject);
            mapMenu = this;
        }
        else if(mapMenu != this)
        {
            Destroy(gameObject);
        }
    }

    public void SelectLevel(int id)
    {
        //Debug.Log(id);
        Application.LoadLevel(id);
    }
}
