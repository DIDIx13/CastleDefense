using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapMarker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    MapMenu mMenu = MapMenu.mapMenu;
    [SerializeField] int id;
    public bool friendly;

    void Start()
    {
        mMenu = MapMenu.mapMenu;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            if(friendly == false)
            {
                mMenu.SelectLevel(id);
            }
        }
    }

    public void SetFriendly()
    {
        this.transform.GetComponent<Image>().color = new Color(0f,190f,1f,200f);
        this.transform.GetChild(0).gameObject.GetComponent<Text>().color = new Color(115f, 255f, 115f, 255f);
        this.friendly = true;        
    }

    public void SetHostile()
    {
        this.transform.GetComponent<Image>().color = new Color(255f,11f,11f,175f);
        this.transform.GetChild(0).gameObject.GetComponent<Text>().color = new Color(255f, 70f, 70f, 255f);
        this.friendly = false;        
    }
}
