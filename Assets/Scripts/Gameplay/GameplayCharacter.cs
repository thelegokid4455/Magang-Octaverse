using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayCharacter : MonoBehaviour
{
    public PlayerCharacterPure thisCharacter;
    public Elements thisElement;
    public bool isEnemy;

    //stat
    public int characterRow;
    public int characterSpeed;

    public float maxHealth;
    public float curHealth;

    public float maxShield;
    public float curShield;

    float targetHealth;
    float targetShield;

    //gameplay
    public int cardHeldThisDeckRotation;
    public List<GameplayCardAttack> myCards = new List<GameplayCardAttack>();
    public List<Card> availableCards = new List<Card>();

    //cards
    public int cardUsed;
    public int cardInHand;
    public int cardLeft;

    //buffs
    public List<ActiveAilments> activeAilments = new List<ActiveAilments>();

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

    [SerializeField] Image effect_Bleed;
    [SerializeField] Image effect_Blind;
    [SerializeField] Image effect_Break;
    [SerializeField] Image effect_Burn;
    [SerializeField] Image effect_Chill;
    [SerializeField] Image effect_Cursed;
    [SerializeField] Image effect_Exhausted;
    [SerializeField] Image effect_Leech;
    [SerializeField] Image effect_Poison;
    [SerializeField] Image effect_Provoke;
    [SerializeField] Image effect_Sleep;
    [SerializeField] Image effect_Stun;
    [SerializeField] Image effect_Absorb;
    [SerializeField] Image effect_Berserk;
    [SerializeField] Image effect_Fortune;
    [SerializeField] Image effect_Regeneration;
    [SerializeField] Image effect_Toughness;
    [SerializeField] Image effect_Cleansing;
    [SerializeField] Image effect_DestroyMana;
    [SerializeField] Image effect_Draw;
    [SerializeField] Image effect_Heal;
    [SerializeField] Image effect_ManaBonus;
    [SerializeField] Image effect_Reposition;
    [SerializeField] Image effect_StealMana;

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

        cardLeft = BattleManager.instance.cardsMaxHeldPerCharacterSingleRotation;

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
        if(attackCard.cardAttackType == AttackType.Magic)
        {
            StartCoroutine(AttackAnim(animObject.GetComponent<Animation>().GetClip("Attack Use")));

        }
        else if (attackCard.cardAttackType == AttackType.Slash)
        {
            StartCoroutine(AttackAnim(animObject.GetComponent<Animation>().GetClip("Attack Melee")));
        }

        GetComponent<AudioSource>().PlayOneShot(GameManager.instance.GetAttackSound(attackCard.cardAttackType));
        var effect = Instantiate(GameManager.instance.GetAttackEffect(attackCard.cardAttackType), transform.position, transform.rotation);
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

    public void ApplyTrueDamage(Elements element, float dmg)
    {
        curHealth -= CountWeakness(element, dmg);

        if(dmg > 0)
        {
            var dtext = Instantiate(GameManager.instance.damageTextEffect, transform.position, Quaternion.identity);
            dtext.GetComponent<DamageHitEffect>().SetDamageText(GameManager.instance.healthDamageColor, CountWeakness(element, dmg));

            StartCoroutine(AttackAnim(animObject.GetComponent<Animation>().GetClip("Get Hit")));
        }

    }

    public void ApplyNormalDamage(Elements element, float dmg)
    {
        if (curShield <= 0)
        {
            curHealth -= CountWeakness(element, dmg);

            //print("applied normal damage " + dmg);
        }
        else
        {
            curShield -= CountWeakness(element, dmg);

            //print("applied shield damage " + dmg);
        }

        if(dmg > 0)
        {
            var dtext = Instantiate(GameManager.instance.damageTextEffect, transform.position, Quaternion.identity);
            dtext.GetComponent<DamageHitEffect>().SetDamageText(GameManager.instance.healthDamageColor, CountWeakness(element, dmg));

            StartCoroutine(AttackAnim(animObject.GetComponent<Animation>().GetClip("Get Hit")));
        }
    }

    float CountWeakness(Elements element, float dmg)
    {
        var gm = GameManager.instance;

        if (element == Elements.Fire)
        {
            switch(thisElement)
            {
                case Elements.Neutral: return dmg;
                case Elements.Fire: return dmg;
                case Elements.Earth: return (dmg * Mathf.RoundToInt(gm.strongerDamagePercent / 100));
                case Elements.Water: return (dmg * Mathf.RoundToInt(gm.weakenedDamagePercent / 100));
                case Elements.Light: return dmg;
                case Elements.Dark: return dmg;
            }
        }
        else if (element == Elements.Water)
        {
            switch (thisElement)
            {
                case Elements.Neutral: return dmg;
                case Elements.Fire: return (dmg * Mathf.RoundToInt(gm.strongerDamagePercent / 100));
                case Elements.Earth: return (dmg * Mathf.RoundToInt(gm.weakenedDamagePercent / 100));
                case Elements.Water: return dmg;
                case Elements.Light: return dmg;
                case Elements.Dark: return dmg;
            }
        }
        else if (element == Elements.Earth)
        {
            switch (thisElement)
            {
                case Elements.Neutral: return dmg;
                case Elements.Fire: return (dmg * Mathf.RoundToInt(gm.weakenedDamagePercent / 100));
                case Elements.Earth: return dmg;
                case Elements.Water: return (dmg * Mathf.RoundToInt(gm.strongerDamagePercent / 100));
                case Elements.Light: return dmg;
                case Elements.Dark: return dmg;
            }
        }
        else if (element == Elements.Light)
        {
            switch (thisElement)
            {
                case Elements.Neutral: return dmg;
                case Elements.Fire: return dmg;
                case Elements.Earth: return dmg;
                case Elements.Water: return dmg;
                case Elements.Light: return dmg;
                case Elements.Dark: return (dmg * Mathf.RoundToInt(gm.strongerDamagePercent / 100));
            }
        }
        else if (element == Elements.Dark)
        {
            switch (thisElement)
            {
                case Elements.Neutral: return dmg;
                case Elements.Fire: return dmg;
                case Elements.Earth: return dmg;
                case Elements.Water: return dmg;
                case Elements.Light: return (dmg * Mathf.RoundToInt(gm.strongerDamagePercent / 100));
                case Elements.Dark: return dmg;
            }
        }
        else
        {
            return dmg;
        }

        return dmg;
    }

    public void AddHealth(float value)
    {
        curHealth += value;

        if (value > 0)
        {
            var dtext = Instantiate(GameManager.instance.damageTextEffect, transform.position, Quaternion.identity);
            dtext.GetComponent<DamageHitEffect>().SetDamageText(GameManager.instance.healthAddColor, value);
        }
    }

    public void AddShield(int value)
    {
        curShield += value;

        if (value > 0)
        {
            var dtext = Instantiate(GameManager.instance.damageTextEffect, transform.position, Quaternion.identity);
            dtext.GetComponent<DamageHitEffect>().SetDamageText(GameManager.instance.shieldAddColor, value);
        }
    }

    public void AddAilment(Ailments newBuff)
    {
        foreach(ActiveAilments ail in activeAilments)
        {
            if (ail.ailmentType.ailmentNames == newBuff.ailmentNames)
            {
                if(newBuff.ailmentNames == AilmentType.Absorb)
                {
                    if (isEnemy)
                    {
                        BattleManager.instance.enemyCurMana += 1;
                        BattleManager.instance.playerCurMana -= 1;
                    }
                    else
                    {
                        BattleManager.instance.enemyCurMana -= 1;
                        BattleManager.instance.playerCurMana += 1;
                    }
                }
                else
                {
                    ail.ailmentCurrentTurns++;
                    return;
                }
            }
        }
        var a = new ActiveAilments();
        a.ailmentType = newBuff;
        a.ailmentCurrentTurns = +2;
        if (a.ailmentCurrentTurns > newBuff.ailmentMaxTurns + 1) a.ailmentCurrentTurns = newBuff.ailmentMaxTurns + 1;
        activeAilments.Add(a);
    }

    public void ApplyAilment()
    {
        foreach (ActiveAilments ailment in activeAilments.ToList())
        {
            AilmentShow();
            if (ailment.ailmentCurrentTurns <= 0)
            {
                activeAilments.Remove(ailment);
                return;
            }

            ApplyTrueDamage(ailment.ailmentType.ailmentElement, ailment.ailmentType.ailmentTrueDamage);
            ApplyNormalDamage(ailment.ailmentType.ailmentElement, ailment.ailmentType.ailmentNormalDamage);

            AddHealth((ailment.ailmentType.ailmentHealthPercentAdd / 100) * maxHealth);
            AddShield((120 / 100) * ailment.ailmentType.ailmentShieldPercentAdd);

            if (ailment.ailmentType.ailmentNames == AilmentType.Absorb)
            {
                if (isEnemy)
                {

                    BattleManager.instance.enemyCurMana += 1;
                    BattleManager.instance.playerCurMana -= 1;
                }
                else
                {

                    BattleManager.instance.enemyCurMana -= 1;
                    BattleManager.instance.playerCurMana += 1;
                }
            }

            ailment.ailmentCurrentTurns--;

            print("ailment " + ailment.ailmentType.name + " reduce 1");
        }
    }

    public void ApplyAilmentInstant(GameplayCharacter character, Ailments toAdd, bool fromEnemy)
    {
        AilmentShow();
        AddHealth((toAdd.ailmentHealthPercentAdd / 100) * maxHealth);
        AddShield((120 / 100) * toAdd.ailmentShieldPercentAdd);

        if(fromEnemy) //if from enemy
        {
            if (toAdd.ailmentManaAdd)
                BattleManager.instance.enemyCurMana += 2;

            if(isEnemy) //to this character is player
            {
                if (toAdd.ailmentManaSteal)
                {
                    BattleManager.instance.enemyCurMana += 1;
                    BattleManager.instance.playerCurMana -= 1;
                }
                if (toAdd.ailmentManaDestroy)
                    BattleManager.instance.enemyCurMana -= 1;
            }
            else //to this character is enemy
            {
            }
        }
        else //if from player
        {
            if (toAdd.ailmentManaAdd)
                BattleManager.instance.playerCurMana += 2;


            if (isEnemy) //to this character is enemy
            {
                


            }
            else //to this character is player
            {
                if (toAdd.ailmentManaSteal)
                {
                    BattleManager.instance.enemyCurMana -= 1;
                    BattleManager.instance.playerCurMana += 1;
                }

                if (toAdd.ailmentManaDestroy)
                    BattleManager.instance.playerCurMana -= 1;
            }

            
        }

        if (toAdd.drawCard)
        {
            //BattleManager.instance.curPlayerCard--;
            character.cardLeft--;

            BattleManager.instance.DrawCard(1, false);
        }

        print("ailment " + toAdd.name + " reduce 1");
    }

    public void AilmentShow()
    {
        foreach (ActiveAilments ailment in activeAilments)
        {
            if(ailment.ailmentCurrentTurns > 0)
            {
                effect_Bleed.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Bleeding);
                effect_Blind.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Blind);
                effect_Break.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Break);
                effect_Burn.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Burn);
                effect_Chill.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Chill);
                effect_Cursed.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Cursed);
                effect_Exhausted.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Exhausted);
                effect_Leech.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Leech);
                effect_Poison.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Poison);
                effect_Provoke.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Provoke);
                effect_Sleep.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Sleep);
                effect_Stun.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Stun);
                effect_Absorb.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Absorb);
                effect_Berserk.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Berserk);
                effect_Fortune.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Fortune);
                effect_Regeneration.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Regeneration);
                effect_Toughness.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Toughness);
                effect_Cleansing.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.CleansingBuff);
                effect_DestroyMana.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.DestroyMana);
                effect_Draw.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.DrawCard);
                effect_Heal.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Heal);
                effect_ManaBonus.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.ManaBonus);
                effect_StealMana.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.StealMana);
                effect_Reposition.gameObject.SetActive(ailment.ailmentType.ailmentNames == AilmentType.Reposition);

            }
            else
            {
                effect_Bleed.gameObject.SetActive(false);
                effect_Blind.gameObject.SetActive(false);
                effect_Break.gameObject.SetActive(false);
                effect_Burn.gameObject.SetActive(false);
                effect_Chill.gameObject.SetActive(false);
                effect_Cursed.gameObject.SetActive(false);
                effect_Exhausted.gameObject.SetActive(false);
                effect_Leech.gameObject.SetActive(false);
                effect_Poison.gameObject.SetActive(false);
                effect_Provoke.gameObject.SetActive(false);
                effect_Sleep.gameObject.SetActive(false);
                effect_Stun.gameObject.SetActive(false);
                effect_Absorb.gameObject.SetActive(false);
                effect_Berserk.gameObject.SetActive(false);
                effect_Fortune.gameObject.SetActive(false);
                effect_Regeneration.gameObject.SetActive(false);
                effect_Toughness.gameObject.SetActive(false);
                effect_Cleansing.gameObject.SetActive(false);
                effect_DestroyMana.gameObject.SetActive(false);
                effect_Draw.gameObject.SetActive(false);
                effect_Heal.gameObject.SetActive(false);
                effect_ManaBonus.gameObject.SetActive(false);
                effect_StealMana.gameObject.SetActive(false);
                effect_Reposition.gameObject.SetActive(false);
            }
        }
    }

}

[Serializable]
public class ActiveAilments
{
    public Ailments ailmentType;

    public int ailmentCurrentTurns;
}