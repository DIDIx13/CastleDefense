using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
    public Camera camera;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject playMenu;
    public GameObject optionsMenu;
    public GameObject charMenu;
    public GameObject gearMenu;
    public GameObject menuTitle;
    public GameObject mapMenu;
    public GameObject levelMenu;

    [Space(10)]
    Transform lerpedTransform1;
    Transform lerpedTransform2;
    Transform lerpedTransform3;
    Transform lerpedTransform4;
    Transform thePoint1;
    Transform thePoint2;
    Transform thePoint3;
    Transform thePoint4;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform centerPoint;
    public Transform titlePoint;

    //main menu
    [Header("Main Menu")]
    public Button mainLoadButton;
    public GameObject gameSaved;
    public GameObject error01Msg;

    bool inMain;
    bool inPlay;
    bool inOptions;

    bool hide;
    bool qlerp;
    bool mlerp;
    bool lerp = false;

    [Space(10)]
    [Header("Camera Positions")]
    public Transform cameraMainPos;
    public Transform cameraCharPos;
    public Transform cameraGearPos;
    public Transform cameraLevelPos;

    float lerpValue = 0.1f;
	
    void Start()
    {
        EnableMenu(mainMenu);
        HideToTheLeft(playMenu);
        MoveToTheCenter(mainMenu);
    }

    void Update()
    {
        if(GameController.controller.IfSaveExists())
        {
            mainLoadButton.interactable = true;
        }
    }

    void FixedUpdate()
    {
        if(lerp && !hide)
        {
            lerpedTransform1.position = Vector3.Lerp(lerpedTransform1.transform.position, thePoint1.position, lerpValue);
            lerpedTransform2.position = Vector3.Lerp(lerpedTransform2.transform.position, thePoint2.position, lerpValue);
            if (lerpedTransform1.position.x <= thePoint1.position.x + 1 && lerpedTransform1.position.x >= thePoint1.position.x - 1 && 
                lerpedTransform2.position.x <= thePoint2.position.x + 1 && lerpedTransform2.position.x >= thePoint2.position.x - 1)
            {
                lerp = false;
            }
        }

        if(qlerp)
        {
            lerpedTransform3.position = Vector3.Lerp(lerpedTransform3.transform.position, thePoint3.position, lerpValue);
            lerpedTransform3.rotation = Quaternion.Lerp(lerpedTransform3.transform.rotation, thePoint3.rotation, lerpValue);
            if(lerpedTransform3.position.x <= thePoint3.position.x + 0.01 && lerpedTransform3.position.x >= thePoint3.position.x - 0.01)
            {
                qlerp = false;
            }
        }

        if (mlerp)
        {
            lerpedTransform4.position = Vector3.Lerp(lerpedTransform4.transform.position, thePoint4.position, lerpValue);
            if (lerpedTransform4.position.x <= thePoint4.position.x + 1 && lerpedTransform4.position.x >= thePoint4.position.x - 1)
            {
                mlerp = false;
            }
        }

        CheckMenuPosition();
    }

    public void RotateCamera(Vector3 pos)
    {
        camera.transform.LookAt(pos);
    }

    public void MainToPlay()
    {
        HideToTheLeft(mainMenu);
        EnableMenu(playMenu);
        MoveToTheCenter(playMenu);
    }

    public void PlayToMain()
    {
        HideToTheLeft(playMenu);
        EnableMenu(mainMenu);
        MoveToTheCenter(mainMenu);
    }

    public void MapToPlay()
    {
        HideToTheLeft(mapMenu);
        EnableMenu(playMenu);
        MoveToTheCenter(playMenu);
        ShowTitle(menuTitle);
        mlerp = true;
    }

    public void PlayToMap()
    {
        HideToTheLeft(playMenu);
        EnableMenu(mapMenu);
        MoveToTheCenter(mapMenu);
        HideTitle(menuTitle);
        mlerp = true;
    }

    public void MainToOptions()
    {
        HideToTheLeft(mainMenu);
        EnableMenu(optionsMenu);
        MoveToTheCenter(optionsMenu);
    }

    public void OptionsToMain()
    {
        HideToTheLeft(optionsMenu);
        EnableMenu(mainMenu);
        MoveToTheCenter(mainMenu);
        OptionsMenu.optionsMenu.EnableQuality();
    }

    public void PlayToChar()
    {
        HideToTheLeft(playMenu);
        EnableMenu(charMenu);
        MoveToTheCenter(charMenu);
        lerpedTransform3 = camera.transform;
        thePoint3 = cameraCharPos;
        lerp = true;
        qlerp = true;
    }

    public void CharToPlay()
    {
        HideToTheLeft(charMenu);
        EnableMenu(playMenu);
        MoveToTheCenter(playMenu);
        lerpedTransform3 = camera.transform;
        thePoint3 = cameraMainPos;
        lerp = true;
        qlerp = true;
    }

    public void CharToGear()
    {
        HideToTheLeft(charMenu);
        EnableMenu(gearMenu);
        MoveToTheCenter(gearMenu);
        lerpedTransform3 = camera.transform;
        thePoint3 = cameraGearPos;
        HideTitle(menuTitle);
        lerp = true;
        qlerp = true;
        mlerp = true;
    }

    public void GearToChar()
    {
        HideToTheLeft(gearMenu);
        EnableMenu(charMenu);
        MoveToTheCenter(charMenu);
        ShowTitle(menuTitle);
        lerpedTransform3 = camera.transform;
        thePoint3 = cameraCharPos;
        lerp = true;
        qlerp = true;
        mlerp = true;
    }

    public void CharToLevel()
    {
        HideToTheLeft(charMenu);
        EnableMenu(levelMenu);
        MoveToTheCenter(levelMenu);
        lerpedTransform3 = camera.transform;
        thePoint3 = cameraLevelPos;
        HideTitle(menuTitle);
        lerp = true;
        qlerp = true;
        mlerp = true;
    }

    public void LevelToChar()
    {
        HideToTheLeft(levelMenu);
        EnableMenu(charMenu);
        MoveToTheCenter(charMenu);
        ShowTitle(menuTitle);
        lerpedTransform3 = camera.transform;
        thePoint3 = cameraCharPos;
        lerp = true;
        qlerp = true;
        mlerp = true;
    }

    public void StartTutorial()
    {
        Application.LoadLevel(1);
    }

    void EnableMenu(GameObject target)
    {
        target.SetActive(true);
    }

    void DisableMenu(GameObject target)
    {
        target.SetActive(false);
    }

    public void MoveToTheCenter(GameObject target)
    {
        lerp = true;
        lerpedTransform1 = target.transform;
        thePoint1 = centerPoint;
    }

    public void ShowTitle(GameObject target)
    {
        mlerp = true;
        lerpedTransform4 = target.transform;
        thePoint4 = titlePoint;
    }

    public void HideToTheLeft(GameObject target)
    {
        lerp = true;
        lerpedTransform2 = target.transform;
        thePoint2 = leftPoint;
    }

    public void HideTitle(GameObject target)
    {
        mlerp = true;
        lerpedTransform4 = target.transform;
        thePoint4 = leftPoint;
    }

    void CheckMenuPosition()
    {
        if (mainMenu.transform.position.x <= leftPoint.position.x + 1 && mainMenu.transform.position.x >= leftPoint.position.x - 1)
        {
            DisableMenu(mainMenu);
        }

        if (optionsMenu.transform.position.x <= leftPoint.position.x + 1 && optionsMenu.transform.position.x >= leftPoint.position.x - 1)
        {
            DisableMenu(optionsMenu);
        }

        if (playMenu.transform.position.x <= leftPoint.position.x + 1 && playMenu.transform.position.x >= leftPoint.position.x - 1)
        {
            DisableMenu(playMenu);
        }

        if (mapMenu.transform.position.x <= leftPoint.position.x + 1 && mapMenu.transform.position.x >= leftPoint.position.x - 1)
        {
            DisableMenu(mapMenu);
        }

        if (charMenu.transform.position.x <= leftPoint.position.x + 1 && charMenu.transform.position.x >= leftPoint.position.x - 1)
        {
            DisableMenu(charMenu);
        }
    }

    public void SaveData()
    {
        GameController.controller.SaveGame();
        StartCoroutine("SavedGameEnable");
    }

    public void ShowError01()
    {
        StartCoroutine("Error01Enable");
    }

    IEnumerator SavedGameEnable()
    {
        gameSaved.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameSaved.SetActive(false);
    }

    IEnumerator Error01Enable()
    {
        error01Msg.SetActive(true);
        yield return new WaitForSeconds(2f);
        error01Msg.SetActive(false);
    }
}
