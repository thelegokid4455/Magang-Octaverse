using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //temporary characters
    public List<Character> selectedCharacters = new List<Character>();

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