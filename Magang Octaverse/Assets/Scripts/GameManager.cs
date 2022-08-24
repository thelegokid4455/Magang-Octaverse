using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //temporary characters
    public PlayerCharacterPure character1;
    public PlayerCharacterPure character2;
    public PlayerCharacterPure character3;
    public PlayerCharacterPure character4;

    public GameObject MenuScene;
    public GameObject BattleScene;
    public bool inBattle;

    //UI
    public Sprite neutralCardBase;
    public Sprite fireCardBase;
    public Sprite waterCardBase;
    public Sprite earthCardBase;
    public Sprite lightCardBase;
    public Sprite darkCardBase;

    public GameObject battleCard;
    public GameObject battleCharacter;

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MenuScene.SetActive(!inBattle);
        BattleScene.SetActive(inBattle);
    }

    public void EnterBattle()
    {
        inBattle = true;
    }

    public void ExitBattle()
    {
        inBattle = false;
    }

}

public enum Elements
{
    Neutral,
    Fire,
    Water,
    Earth,
    Light,
    Dark
}