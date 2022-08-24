using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] Transform[] idlePosition;
    [SerializeField] List<GameObject> selectedChars = new List<GameObject>();

    //UI
    [SerializeField] GameObject HomeMenu;
    [SerializeField] GameObject BattleMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HomeMenu.SetActive(!GameManager.instance.inBattle);
        BattleMenu.SetActive(GameManager.instance.inBattle);
    }

    public void SelectCampaign(Mission mission)
    {
        print("Selected mission " + mission.missionName);

        BattleManager.instance.SelectedMission(mission);
    }
}
