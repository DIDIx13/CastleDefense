using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour 
{
    GameObject player;
    GameObject camera;

    [Header("HealthBar")]
    public RectTransform healthTransform;
    private float cachedY;
    private float minXValue;
    private float maxXValue;
    private float currentHealth;
    public float maxHealth;

    [Header("Menu")]
    public GameObject menu;
    public GameObject alert1;
    public GameObject alert2;
    public GameObject alert3;
    public bool menuEnabled = false;
    bool canEnable = true;

    [Header("Other menus")]
    public GameObject lootMenu;
    public GameObject lossMenu;

    private float CurrentHealth 
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            HandleHealth();
        }
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = healthTransform.position.x - healthTransform.rect.width;
        maxHealth = player.GetComponent<PlayerStats>().health;
        currentHealth = maxHealth; 

        camera = GameObject.FindGameObjectWithTag("CameraHolder");
    }

    void Update()
    {
        CurrentHealth = player.GetComponent<PlayerStats>().currentHealth;
        maxHealth = player.GetComponent<PlayerStats>().health;

        if(Input.GetButtonDown("Cancel") && canEnable)
        {
            if (alert1.GetComponent<Image>().isActiveAndEnabled == false && alert2.GetComponent<Image>().isActiveAndEnabled == false && alert3.GetComponent<Image>().isActiveAndEnabled == false)
            { 
                menuEnabled = !menuEnabled;
            }
            else
            {
                if (alert1.GetComponent<Image>().isActiveAndEnabled == true)
                {
                    HideAlert1();
                }
                else if (alert2.GetComponent<Image>().isActiveAndEnabled == true)
                {
                    HideAlert2();
                }
                else
                {
                    HideAlert3();
                }
            }
        }

        if(menuEnabled && canEnable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            DisplayMenu();
            player.GetComponent<PlayerController>().isEnabled = false;
            camera.GetComponent<FreeCameraLook>().isEnabled = false;
        }
        else if(!menuEnabled && canEnable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            HideMenu();
            player.GetComponent<PlayerController>().isEnabled = true;
            camera.GetComponent<FreeCameraLook>().isEnabled = true;
        }
        else if(!canEnable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            HideMenu();
            player.GetComponent<PlayerController>().isEnabled = false;
            camera.GetComponent<FreeCameraLook>().isEnabled = false;
        }
    }

    public void DisplayVictoryScreen(float exp)
    {
        
        PlayerStats pS = player.GetComponent<PlayerStats>();
        Text expGained = transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<Text>();
        Text expRequired = transform.GetChild(2).GetChild(0).GetChild(3).GetComponent<Text>();
        Text lvl = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>();

        pS.experience += exp;
        int beflvl = pS.level;
        pS.CalculateExp();
        pS.CalculateExp();
        int diff = pS.level - beflvl;
        canEnable = false;
        lootMenu.SetActive(true);
        expGained.text = "Exp. gained: " + exp + " (+" + diff + " levels)";
        expRequired.text = "Next level: " + pS.experienceToNext;
        lvl.text = "Your level: " + pS.level;
        pS.SaveExp();
    }

    public void DisplayLossScreen(float exp)
    {
        PlayerStats pS = player.GetComponent<PlayerStats>();
        Text expGained = transform.GetChild(3).GetChild(3).GetComponent<Text>();
        Text expRequired = transform.GetChild(3).GetChild(4).GetComponent<Text>();
        Text lvl = transform.GetChild(3).GetChild(2).GetComponent<Text>();

        pS.experience += (exp / 4);
        int beflvl = pS.level;
        pS.CalculateExp();
        pS.CalculateExp();
        int diff = pS.level - beflvl;
        canEnable = false;
        lossMenu.SetActive(true);
        expGained.text = "Exp. gained: " + (exp/4) + " (+" + diff + " levels)";
        expRequired.text = "Next level: " + pS.experienceToNext;
        lvl.text = "Your level: " + pS.level;
        pS.SaveExp();
    }

    private void HandleHealth()
    {
        float currentXValue = MapValues(currentHealth, 0, maxHealth, minXValue, maxXValue);

        healthTransform.position = new Vector3(currentXValue, cachedY);
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    void DisplayMenu()
    {
        menu.SetActive(true);        
    }

    void HideMenu()
    {
        menu.SetActive(false);
    }

    public void RestartGame()
    {
        GameController.controller.SaveGame();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ExitLevel()
    {
        GameController.controller.SaveGame();
        Application.LoadLevel(0);
    }

    public void ShowAlert1()
    {
        alert1.SetActive(true);
    }

    public void HideAlert1()
    {
        alert1.SetActive(false);
    }

    public void ShowAlert2()
    {
        alert2.SetActive(true);
    }

    public void HideAlert2()
    {
        alert2.SetActive(false);
    }

    public void ShowAlert3()
    {
        alert3.SetActive(true);
    }

    public void HideAlert3()
    {
        alert3.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
