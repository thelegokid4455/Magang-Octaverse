using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] Transform[] idlePosition;
    //[SerializeField] List<GameObject> selectedChars = new List<GameObject>();

    //Character
    [SerializeField] List<GameplayCharacterIdle> characters = new List<GameplayCharacterIdle>();
    [SerializeField] GameObject characterObjects;

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

            characterObjects.SetActive(false);

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

            characterObjects.SetActive(true);
        }

        currentCoinText.text = GameManager.instance.currentCoin.ToString();
    }

    public void SetIdleCharData()
    {
        foreach(GameplayCharacterIdle character in characters)
        {
            character.characterBody = GameManager.instance.selectedCharacters[characters.IndexOf(character)].characterBody;
            character.characterFace = GameManager.instance.selectedCharacters[characters.IndexOf(character)].characterFace;
            character.characterWeapon = GameManager.instance.selectedCharacters[characters.IndexOf(character)].characterWeapon;
            character.characterTop = GameManager.instance.selectedCharacters[characters.IndexOf(character)].characterTop;
            character.characterBack = GameManager.instance.selectedCharacters[characters.IndexOf(character)].characterBack;

            character.SetIdleImage();
        }
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
}
