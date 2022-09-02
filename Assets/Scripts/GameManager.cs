using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //used characters
    public List<Character> selectedCharacters = new List<Character>();

    //gold
    public int currentMoney;

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
                

                var count = 0;

                while(count < 4)
                {
                    var newChar = new Character();
                    selectedCharacters.Add(newChar);

                    newChar.characterBody = InventoryManager.instance.playerInventory.characterInInventory[count].characterBody;
                    newChar.characterFace = InventoryManager.instance.playerInventory.characterInInventory[count].characterFace;
                    newChar.characterWeapon = InventoryManager.instance.playerInventory.characterInInventory[count].characterWeapon;
                    newChar.characterTop = InventoryManager.instance.playerInventory.characterInInventory[count].characterTop;
                    newChar.characterBack = InventoryManager.instance.playerInventory.characterInInventory[count].characterBack;

                    count++;
                }

                //selectedCharacters.Add(InventoryManager.instance.playerInventory.characterInInventory[0]);
                //selectedCharacters.Add(InventoryManager.instance.playerInventory.characterInInventory[1]);
                //selectedCharacters.Add(InventoryManager.instance.playerInventory.characterInInventory[2]);
                //selectedCharacters.Add(InventoryManager.instance.playerInventory.characterInInventory[3]);
            }
            else
            {

            }

            MenuSceneManager.instance.SetCharData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MenuScene.SetActive(!inBattle);
        //BattleScene.SetActive(inBattle);
        
        //cheat
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.M))
        {
            AddGold(10000);
        }
#endif
    }

    public bool HasEnoughMoney(int compare)
    {
        if (currentMoney >= compare) return true;
        return false;
    }
    public void AddGold(int amount)
    {
        currentMoney += amount;
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

        MenuSceneManager.instance.SetCharData();


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