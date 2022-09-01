using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayCharacter : MonoBehaviour
{
    public PlayerCharacterPure thisCharacter;
    public Elements thisElement;

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

    //sprites
    [SerializeField] GameObject animObject;

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

    }

    private void Start()
    {

    }

    public void SetCharacter(PlayerCharacterPure character)
    {
        thisCharacter = character;
    }

    public void SetData(BodyPart newBody, BodyPart newFace, BodyPart newWeapon, BodyPart newTop, BodyPart newBack)
    {

        if (thisCharacter)
        {
            characterBody = thisCharacter.characterBody;
            characterFace = thisCharacter.characterFace;
            characterWeapon = thisCharacter.characterWeapon;
            characterTop = thisCharacter.characterTop;
            characterBack = thisCharacter.characterBack;

            //set image
            spriteBody.sprite = thisCharacter.characterBody.partImage;
            spriteFace.sprite = thisCharacter.characterFace.partImage;
            spriteWeapon.sprite = thisCharacter.characterWeapon.partImage;
            spriteTop.sprite = thisCharacter.characterTop.partImage;
            spriteBack.sprite = thisCharacter.characterBack.partImage;

            //set element
            thisElement = thisCharacter.characterBody.partElement;

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
            characterBody = newBody;
            characterFace = newFace;
            characterWeapon = newWeapon;
            characterTop = newTop;
            characterBack = newBack;

            //set image
            spriteBody.sprite = characterBody.partImage;
            spriteFace.sprite = characterFace.partImage;
            spriteWeapon.sprite = characterWeapon.partImage;
            spriteTop.sprite = characterTop.partImage;
            spriteBack.sprite = characterBack.partImage;

            //set element
            thisElement = characterBody.partElement;

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

        foreach(GameplayCardAttack card in myCards)
        {
            card.thisElement = thisElement;
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

    //animation
    public void Attack(Card attackCard)
    {
        animObject.GetComponent<Animation>().Stop();
        if(attackCard.cardAttackType == "Use")
        {
            StartCoroutine(AttackAnim(animObject.GetComponent<Animation>().GetClip("Attack Use")));

        }
        else if (attackCard.cardAttackType == "Attack")
        {
            StartCoroutine(AttackAnim(animObject.GetComponent<Animation>().GetClip("Attack Melee")));
        }

        GetComponent<AudioSource>().PlayOneShot(attackCard.cardAttackSound);
        var effect = Instantiate(attackCard.cardAttackEffect, transform.position, transform.rotation);
        effect.transform.localScale = new Vector3(transform.lossyScale.x, 1, 1);

    }

    IEnumerator AttackAnim(AnimationClip animName)
    {

        animObject.GetComponent<Animation>().clip = animName;
        animObject.GetComponent<Animation>().Play();

        yield return new WaitForSeconds(animName.length);

        animObject.GetComponent<Animation>().Stop();
        animObject.GetComponent<Animation>().Play("Idle");
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
