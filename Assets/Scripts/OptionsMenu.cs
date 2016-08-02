using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour 
{
    public static OptionsMenu optionsMenu;

    GameObject source;

    public GameObject qMenu;
    public GameObject gMenu;
    public GameObject aMenu;

    public Toggle musicToggle;
    float musicVolume = 0.15f;

    public Slider qualitySlider;
    int qualityValue;

    void Awake()
    {
        if (optionsMenu == null)
        {
            optionsMenu = this;
        }
        else if (optionsMenu != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        source = GameObject.FindGameObjectWithTag("Music");
        qualityValue = QualitySettings.GetQualityLevel();
        qualitySlider.value = qualityValue;
    }

    void Update()
    {
        source.GetComponent<AudioSource>().volume = musicVolume;
        QualitySettings.SetQualityLevel(qualityValue);
    }

    public void ToggleMusic(bool state)
    {
        state = musicToggle.isOn;

        if(state)
        {
            source.GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            source.GetComponent<AudioSource>().enabled = false;
        }
    }

    public void AdjustMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
    }

    public void AdjustQualityLevel(float newValue)
    {
        qualityValue = (int)newValue;
    }

    void EnableMenu(GameObject target)
    {
        target.SetActive(true);
    }

    void DisableMenu(GameObject target)
    {
        target.SetActive(false);
    }

    public void EnableQuality()
    {
        EnableMenu(qMenu);
        DisableMenu(aMenu);
        DisableMenu(gMenu);
    }

    public void EnableAudio()
    {
        EnableMenu(aMenu);
        DisableMenu(qMenu);
        DisableMenu(gMenu);
    }

    public void EnableGame()
    {
        EnableMenu(gMenu);
        DisableMenu(aMenu);
        DisableMenu(qMenu);
    }
}
