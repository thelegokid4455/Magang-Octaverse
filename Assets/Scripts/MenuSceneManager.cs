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

    //UI

    [SerializeField] Text currentCoinText;

    [SerializeField] GameObject HomeMenu;
    [SerializeField] GameObject BattleMenu;
    [SerializeField] GameObject FinishMenu;

    [SerializeField] GameObject WinSign;
    [SerializeField] GameObject LoseSign;

    [SerializeField] Text coinEarnText;

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
    }

    // Update is called once per frame
    void Update()
    {
        HomeMenu.SetActive(!GameManager.instance.inBattle);
        BattleMenu.SetActive(GameManager.instance.inBattle);

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

    public void FinishBattle(bool isWin)
    {
        FinishMenu.SetActive(true);

        coinEarnText.text = "+" + BattleManager.instance.currentMission.missionReward + " Coins";

        WinSign.SetActive(isWin);
        LoseSign.SetActive(!isWin);

        //

    }

    public void FinishBattleConfirmation()
    {

        FinishMenu.SetActive(false);

        WinSign.SetActive(false);
        LoseSign.SetActive(false);

        GameManager.instance.ExitBattle();

    }
}
