using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayCharacter : MonoBehaviour
{
    public PlayerCharacterPure thisCharacter;

    //stat
    public int characterRow;
    public int characterSpeed;

    public int maxHealth;
    public int curHealth;

    public int maxShield;
    public int curShield;

    int targetHealth;
    int targetShield;

    //gameplay
    public int cardHeldThisDeckRotation;
    public List<GameplayCardAttack> myCards = new List<GameplayCardAttack>();

    [SerializeField] SpriteRenderer characterSprite;

    //UI
    [SerializeField] Slider healthBar;
    [SerializeField] Slider shieldBar;

    // Start is called before the first frame update
    void Awake()
    {
        //SetData();
    }

    private void Start()
    {
        //SetData();
    }

    public void SetCharacter(PlayerCharacterPure character)
    {
        thisCharacter = character;
    }

    public void SetData()
    {
        curHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        shieldBar.maxValue = maxShield;

        if (thisCharacter)
        {
            characterSprite.sprite = thisCharacter.characterSprite;
            characterSpeed = thisCharacter.characterSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetHealth = curHealth;
        targetShield = curShield;

        healthBar.DOValue(targetHealth, 1);
        shieldBar.DOValue(targetShield, 1);

        shieldBar.gameObject.SetActive(curShield > 0);

        if (curHealth > maxHealth) curHealth = maxHealth;
    }

    public void ApplyTrueDamage(int dmg)
    {
        curHealth -= dmg;
    }

    public void ApplyNormalDamage(int dmg)
    {
        if (curShield <= 0)
        {
            curHealth -= dmg;

            //print("applied normal damage " + dmg);
        }
        else
        {
            curShield -= dmg;

            //print("applied shield damage " + dmg);
        }
    }
    public void AddHealth(int value)
    {
        curHealth += value;
    }

    public void AddShield(int value)
    {
        curShield += value;
    }

}
