using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] Transform[] idlePosition;
    //[SerializeField] List<GameObject> selectedChars = new List<GameObject>();

    //Character
    [SerializeField] List<GameplayCharacterIdle> characters = new List<GameplayCharacterIdle>();

    //UI
    [SerializeField] GameObject HomeMenu;
    [SerializeField] GameObject BattleMenu;

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

        BattleManager.instance.SelectedMission(mission);
    }

}
