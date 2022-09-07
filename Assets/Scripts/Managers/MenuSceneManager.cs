using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    //[SerializeField] List<GameObject> selectedChars = new List<GameObject>();

    //Character
    public List<GameplayCharacterIdle> charactersHome = new List<GameplayCharacterIdle>();

    public List<GameplayCharacterIdle> charactersTeam = new List<GameplayCharacterIdle>();

    //UI

    //-screens
    [SerializeField] int selectedScreen;

    [SerializeField] Text currentCoinText;

    //scenes
    [SerializeField] GameObject HomeScene;
    [SerializeField] GameObject TeamScene;
    [SerializeField] GameObject RecruitScene;
    [SerializeField] GameObject StoreScene;

    //menus
    [SerializeField] GameObject HomeMenu;
    [SerializeField] GameObject TeamMenu;
    [SerializeField] GameObject RecruitMenu;
    [SerializeField] GameObject StoreMenu;

    //main menus
    [SerializeField] GameObject BattleScene;
    [SerializeField] GameObject BattleMenu;
    [SerializeField] GameObject MainMenu;

    //finish
    [SerializeField] GameObject FinishMenu;

    [SerializeField] GameObject WinSign;
    [SerializeField] GameObject LoseSign;

    [SerializeField] Text coinEarnText;

    //transition
    public GameObject transitionObject;

    //NOTIF
    [SerializeField] GameObject notifGameobject;
    [SerializeField] Text notifText;

    public static MenuSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetIdleCharData();
        selectedScreen = 1;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (GameManager.instance.inBattle)
        {
            BattleMenu.SetActive(true);
            BattleScene.SetActive(true);

            MainMenu.SetActive(false);

            HomeScene.SetActive(false);
            TeamScene.SetActive(false);
            RecruitScene.SetActive(false);
            StoreScene.SetActive(false);

            HomeMenu.SetActive(false);
            TeamMenu.SetActive(false);
            RecruitMenu.SetActive(false);
            StoreMenu.SetActive(false);

        }
        else
        {
            if (selectedScreen == 1) HomeScene.SetActive(true); else HomeScene.SetActive(false);
            if (selectedScreen == 2) TeamScene.SetActive(true); else TeamScene.SetActive(false);
            if (selectedScreen == 3) RecruitScene.SetActive(true); else RecruitScene.SetActive(false);
            if (selectedScreen == 4) StoreScene.SetActive(true); else StoreScene.SetActive(false);

            if (selectedScreen == 1) HomeMenu.SetActive(true); else HomeMenu.SetActive(false);
            if (selectedScreen == 2) TeamMenu.SetActive(true); else TeamMenu.SetActive(false);
            if (selectedScreen == 3) RecruitMenu.SetActive(true); else RecruitMenu.SetActive(false);
            if (selectedScreen == 4) StoreMenu.SetActive(true); else StoreMenu.SetActive(false);

            BattleMenu.SetActive(false);
            BattleScene.SetActive(false);

            MainMenu.SetActive(true);
        }

        currentCoinText.text = GameManager.instance.currentMoney.ToString();
    }

    public void SetCharData()
    {

        //team
        foreach (GameplayCharacterIdle character in charactersTeam)
        {
            character.characterBody = GameManager.instance.selectedCharacters[charactersTeam.IndexOf(character)].characterBody;
            character.characterFace = GameManager.instance.selectedCharacters[charactersTeam.IndexOf(character)].characterFace;
            character.characterWeapon = GameManager.instance.selectedCharacters[charactersTeam.IndexOf(character)].characterWeapon;
            character.characterTop = GameManager.instance.selectedCharacters[charactersTeam.IndexOf(character)].characterTop;
            character.characterBack = GameManager.instance.selectedCharacters[charactersTeam.IndexOf(character)].characterBack;

            character.SetIdleImage();
        }

        //home
        foreach (GameplayCharacterIdle character in charactersHome)
        {
            character.characterBody = charactersTeam[charactersHome.IndexOf(character)].characterBody;
            character.characterFace = charactersTeam[charactersHome.IndexOf(character)].characterFace;
            character.characterWeapon = charactersTeam[charactersHome.IndexOf(character)].characterWeapon;
            character.characterTop = charactersTeam[charactersHome.IndexOf(character)].characterTop;
            character.characterBack = charactersTeam[charactersHome.IndexOf(character)].characterBack;

            character.SetIdleImage();
        }
    }

    public void SetNewCharacter()
    {

        //selected
        foreach (Character character in GameManager.instance.selectedCharacters)
        {
            character.characterBody = charactersTeam[GameManager.instance.selectedCharacters.IndexOf(character)].characterBody;
            character.characterFace = charactersTeam[GameManager.instance.selectedCharacters.IndexOf(character)].characterFace;
            character.characterWeapon = charactersTeam[GameManager.instance.selectedCharacters.IndexOf(character)].characterWeapon;
            character.characterTop = charactersTeam[GameManager.instance.selectedCharacters.IndexOf(character)].characterTop;
            character.characterBack = charactersTeam[GameManager.instance.selectedCharacters.IndexOf(character)].characterBack;

            //character.SetIdleImage();
        }

        SetCharData();
    }

    //buttons
    public void SelectCampaign(Mission mission)
    {
        print("Selected mission " + mission.missionName);

        StartCoroutine(TransitionAnimationStartMission(mission));

    }

    public void ChangeMenu(int target)
    {
        selectedScreen = target;

        RecruitManager.instance.SetData();
    }

    //transition
    public IEnumerator TransitionAnimationStartMission(Mission mission)
    {
        transitionObject.SetActive(true);
        transitionObject.GetComponent<Animation>().Stop();
        transitionObject.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(transitionObject.GetComponent<Animation>().clip.length / 2);

        BattleManager.instance.SelectedMission(mission);

        yield return new WaitForSeconds(transitionObject.GetComponent<Animation>().clip.length / 2);

        transitionObject.SetActive(false);
        transitionObject.GetComponent<Animation>().Stop();
    }

    //UI
    public void SetNotification(string newText)
    {
        notifGameobject.SetActive(true);
        notifText.text = newText;
    }

    //battle
    public void FinishBattle(bool isWin)
    {
        FinishMenu.SetActive(true);

        coinEarnText.text = "+" + BattleManager.instance.currentMission.missionReward + " Coins";

        WinSign.SetActive(isWin);
        LoseSign.SetActive(!isWin);

        selectedScreen = 1;

        //

    }

    public void FinishBattleConfirmation()
    {

        FinishMenu.SetActive(false);

        WinSign.SetActive(false);
        LoseSign.SetActive(false);

        GameManager.instance.ExitBattle();

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetData()
    {
        SaveManager.instance.ResetSaveFile(true);
        
    }
}
