using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class StatsTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string name;
    public string description;
    StatsMenu stats;

    void Awake()
    {
        stats = GetComponentInParent<StatsMenu>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        stats.ShowTooltip(this.GetComponent<RectTransform>().position, name, description);
    }

    public void OnPointerExit(PointerEventData data)
    {
        stats.HideTooltip();
    }
}
