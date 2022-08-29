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
    public List<Card> availableCards = new List<Card>();

    //[SerializeField] SpriteRenderer characterSprite;
    [SerializeField] SpriteRenderer spriteBody;
    [SerializeField] SpriteRenderer spriteFace;
    [SerializeField] SpriteRenderer spriteWeapon;
    [SerializeField] SpriteRenderer spriteTop;
    [SerializeField] SpriteRenderer spriteBack;

    [SerializeField] BodyPart characterBody;
    [SerializeField] BodyPart characterFace;
    [SerializeField] BodyPart characterWeapon;
    [SerializeField] BodyPart characterTop;
    [SerializeField] BodyPart characterBack;

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

        if (thisCharacter)
        {
            //characterSprite.sprite = thisCharacter.characterSprite;

            //set image
            spriteBody.sprite = thisCharacter.characterBody.partImage;
            spriteFace.sprite = thisCharacter.characterFace.partImage;
            spriteWeapon.sprite = thisCharacter.characterWeapon.partImage;
            spriteTop.sprite = thisCharacter.characterTop.partImage;
            spriteBack.sprite = thisCharacter.characterBack.partImage;

            //characterSpeed = thisCharacter.characterSpeed;

            //set speed
            characterSpeed += thisCharacter.characterBody.partSpeedAdd
                + thisCharacter.characterFace.partSpeedAdd
                + thisCharacter.characterWeapon.partSpeedAdd
                + thisCharacter.characterTop.partSpeedAdd
                + thisCharacter.characterBack.partSpeedAdd;

            //set health
            maxHealth += thisCharacter.characterBody.partHealthAdd
                + thisCharacter.characterFace.partHealthAdd
                + thisCharacter.characterWeapon.partHealthAdd
                + thisCharacter.characterTop.partHealthAdd
                + thisCharacter.characterBack.partHealthAdd;

            //set cards
            if (thisCharacter.isEnemy)
            {
                foreach(Card card in thisCharacter.enemyCards)
                {

                    availableCards.Add(card);
                }
            }
            else
            {
                //availableCards.Add(thisCharacter.characterBody.partCard);
                availableCards.Add(thisCharacter.characterFace.partCard);
                availableCards.Add(thisCharacter.characterWeapon.partCard);
                availableCards.Add(thisCharacter.characterTop.partCard);
                availableCards.Add(thisCharacter.characterBack.partCard);
            }

        }
        else
        {
            //set image
            spriteBody.sprite = characterBody.partImage;
            spriteFace.sprite = characterFace.partImage;
            spriteWeapon.sprite = characterWeapon.partImage;
            spriteTop.sprite = characterTop.partImage;
            spriteBack.sprite = characterBack.partImage;

            //set speed
            characterSpeed += characterBody.partSpeedAdd
                + characterFace.partSpeedAdd
                + characterWeapon.partSpeedAdd
                + characterTop.partSpeedAdd
                + characterBack.partSpeedAdd;

            //set health
            maxHealth += characterBody.partHealthAdd
                + characterFace.partHealthAdd
                + characterWeapon.partHealthAdd
                + characterTop.partHealthAdd
                + characterBack.partHealthAdd;

            //set cards
            //availableCards.Add(characterBody.partCard);
            availableCards.Add(characterFace.partCard);
            availableCards.Add(characterWeapon.partCard);
            availableCards.Add(characterTop.partCard);
            availableCards.Add(characterBack.partCard);
        }


        curHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        shieldBar.maxValue = maxShield;
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
