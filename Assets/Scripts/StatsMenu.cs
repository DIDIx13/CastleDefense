using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour 
{
    Text strengthBar;
    Text enduranceBar;
    Text agilityBar;
    Text statPointsImage;
    Text skillPointsImage;
    Text oneHandedBar;
    Text twoHandedBar;
    Text shieldingBar;
    Text healthTxt;
    Text dmgMultiTxt;
    Text spdMultiTxt;

    public Text playerLevel;

    public GameObject canvas;
    public GameObject tooltip;

    PlayerStats player;
    PlayerStatistics playerStats = PlayerStatistics.playerStatistics;

    private int str;
    private int end;
    private int agi;
    private int ohf;
    private int thf;
    private int shd;
    private int staP;
    private int skiP;

    void OnEnable()
    {
        CacheStats();
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        strengthBar = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>();
        enduranceBar = transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>();
        agilityBar = transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>();
        statPointsImage = transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>();
        skillPointsImage = transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>();
        oneHandedBar = transform.GetChild(5).GetChild(3).GetChild(0).GetComponent<Text>();
        twoHandedBar = transform.GetChild(6).GetChild(3).GetChild(0).GetComponent<Text>();
        shieldingBar = transform.GetChild(7).GetChild(3).GetChild(0).GetComponent<Text>();
        healthTxt = transform.GetChild(8).GetChild(0).GetComponent<Text>();
        dmgMultiTxt = transform.GetChild(9).GetChild(0).GetComponent<Text>();
        spdMultiTxt = transform.GetChild(10).GetChild(0).GetComponent<Text>();
    }

    void Start()
    {
        playerStats = PlayerStatistics.playerStatistics;
        HideTooltip();
    }

    void Update()
    {
        DisplayRemainingPoints();
        DisplayCurrentStats();
    }

    void DisplayRemainingPoints()
    {
        statPointsImage.text = (player.statPoints).ToString();
        skillPointsImage.text = (player.skillPoints).ToString();
    }

    void DisplayCurrentStats()
    {
        strengthBar.text = (player.strength).ToString();
        enduranceBar.text = (player.endurance).ToString();
        agilityBar.text = (player.agility).ToString();
        oneHandedBar.text = (player.oneHanded).ToString();
        twoHandedBar.text = (player.twoHanded).ToString();
        shieldingBar.text = (player.shielding).ToString();
        healthTxt.text = (player.health).ToString();
        dmgMultiTxt.text = (player.dmgMulti).ToString();
        spdMultiTxt.text = (player.spdMulti).ToString();
        playerLevel.text = "Level: " + player.level + " (" + ((player.experience / player.experienceToNext) * 100).ToString("f1") + "%)";
    }

    public void UpStat(string name)
    {
        if(player.statPoints > 0)
        {
            if(name == "STRENGTH")
            {
                player.strength++;
                player.statPoints--;
            }
            else if(name == "ENDURANCE")
            {
                player.endurance++;
                player.statPoints--;
            }
            else if(name == "AGILITY")
            {
                player.agility++;
                player.statPoints--;
            } 
        }
    }

    public void DownStat(string name)
    {
        if (name == "STRENGTH")
        {
            if(player.strength != str)
            {
                player.strength--;
                player.statPoints++;
            }
        }
        else if (name == "ENDURANCE")
        {
            if (player.endurance != end)
            {
                player.endurance--;
                player.statPoints++;
            }
        }
        else if (name == "AGILITY")
        {
            if (player.agility != agi)
            {
                player.agility--;
                player.statPoints++;
            }
        }
    }

    public void UpSkill(string name)
    {
        if (player.skillPoints > 0)
        {
            if (name == "ONEHANDED")
            {
                player.oneHanded += 3;
                player.skillPoints--;
            }
            else if (name == "TWOHANDED")
            {
                player.twoHanded += 3;
                player.skillPoints--;
            }
            else if (name == "SHIELDING")
            {
                player.shielding += 3;
                player.skillPoints--;
            }
        }
    }

    public void DownSkill(string name)
    {
        if (name == "ONEHANDED")
        {
            if (player.oneHanded != ohf)
            {
                player.oneHanded -= 3;
                player.skillPoints++;
            }
        }
        else if (name == "TWOHANDED")
        {
            if (player.twoHanded != thf)
            {
                player.twoHanded -= 3;
                player.skillPoints++;
            }
        }
        else if (name == "SHIELDING")
        {
            if (player.shielding != shd)
            {
                player.shielding -= 3;
                player.skillPoints++;
            }
        }
    }

    public void CacheStats()
    {
        str = player.strength;
        end = player.endurance;
        agi = player.agility;
        staP = player.statPoints;
        ohf = player.oneHanded;
        thf = player.twoHanded;
        shd = player.shielding;
        skiP = player.skillPoints;
    }

    public void ResetStats()
    {
        playerStats.strength = str;
        playerStats.endurance = end;
        playerStats.agility = agi;
        playerStats.oneHanded = ohf;
        playerStats.twoHanded = thf;
        playerStats.shielding = shd;
        playerStats.statPoints = staP;
        playerStats.skillPoints = skiP;
        CacheStats();
    }

    public void SaveStats()
    {
        CacheStats();
        playerStats.strength = str;
        playerStats.endurance = end;
        playerStats.agility = agi;
        playerStats.statPoints = staP;
        playerStats.oneHanded = ohf;
        playerStats.twoHanded = thf;
        playerStats.shielding = shd;
        playerStats.skillPoints = skiP;
    }

    public void ShowTooltip(Vector3 toolPos, string name, string desc)
    {
        tooltip.SetActive(true);
        Vector3 mousePos = Input.mousePosition;
        tooltip.GetComponent<RectTransform>().position = new Vector3(mousePos.x + 50, mousePos.y - 50, mousePos.z);

        tooltip.transform.GetChild(0).GetComponent<Text>().text = name;
        tooltip.transform.GetChild(1).GetComponent<Text>().text = desc;
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
