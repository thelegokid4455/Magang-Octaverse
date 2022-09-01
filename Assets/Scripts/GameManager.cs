using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //temporary characters
    public List<Character> selectedCharacters = new List<Character>();

    //gold
    public int currentCoin;

    //public GameObject MenuScene;
    //public GameObject BattleScene;
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
        if(selectedCharacters.Count <= 0)
        {
            if(InventoryManager.instance.playerInventory.characterInInventory.Count >= 4)
            {
                selectedCharacters.Add(InventoryManager.instance.playerInventory.characterInInventory[0]);
                selectedCharacters.Add(InventoryManager.instance.playerInventory.characterInInventory[1]);
                selectedCharacters.Add(InventoryManager.instance.playerInventory.characterInInventory[2]);
                selectedCharacters.Add(InventoryManager.instance.playerInventory.characterInInventory[3]);
            }
            else
            {
                selectedCharacters.Add(InventoryManager.instance.playerInventory.startingCharacter[0]);
                selectedCharacters.Add(InventoryManager.instance.playerInventory.startingCharacter[1]);
                selectedCharacters.Add(InventoryManager.instance.playerInventory.startingCharacter[2]);
                selectedCharacters.Add(InventoryManager.instance.playerInventory.startingCharacter[3]);
            }

            MenuSceneManager.instance.SetIdleCharData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MenuScene.SetActive(!inBattle);
        //BattleScene.SetActive(inBattle);
    }

    public void AddGold(int amount)
    {
        currentCoin += amount;
    }

    public void EnterBattle()
    {
        //StartCoroutine(TransitionAnimationStart());
        inBattle = true;
        //StartCoroutine(TransitionAnimationExit());
    }

    public void ExitBattle()
    {
        //StartCoroutine(TransitionAnimationStart());
        inBattle = false;
        //StartCoroutine(TransitionAnimationExit());

        MenuSceneManager.instance.SetIdleCharData();


        BattleManager.instance.currentMission = null;
        BattleManager.instance.hasFinished = false;
    }

}

[Serializable]
public class Character
{
    public BodyPart characterBody;
    public BodyPart characterFace;
    public BodyPart characterWeapon;
    public BodyPart characterTop;
    public BodyPart characterBack;
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

public enum BodyParts
{
    Body,
    Face,
    Weapon,
    Top,
    Back
}