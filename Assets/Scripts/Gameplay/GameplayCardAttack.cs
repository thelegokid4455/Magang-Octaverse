using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameplayCardAttack : MonoBehaviour
{
    //stats
    public Card thisCard;
    bool isSelected;
    public Elements thisElement;

    //sounds
    [SerializeField] AudioClip selectSound;
    [SerializeField] AudioClip deselectSound;

    public int cardCharRow;
    public int cardCharSpeed;

    //Battle
    public List<GameplayCharacter> selectedTarget = new List<GameplayCharacter>();
    public GameplayCharacter cardCharacter;

    //UI Pos
    [SerializeField] Vector3 growScale;
    [SerializeField] Vector3 normalScale;
    [SerializeField] Vector3 posSelect;
    Vector3 targetScale;
    Vector3 targetPos;

    [SerializeField] float growSpeed;

    //UI
    [SerializeField] TextMeshPro nameText;
    [SerializeField] SpriteRenderer imageSprite;
    [SerializeField] TextMeshPro manaText;
    [SerializeField] TextMeshPro damageText;
    [SerializeField] TextMeshPro shieldText;
    [SerializeField] TextMeshPro descText;
    [SerializeField] SpriteRenderer cardBaseSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetCard(Card card)
    {
        thisCard = card;
        //
    }

    void SetRow(int row)
    {
        cardCharRow = row;
    }
    void SetData()
    {
        targetScale = normalScale;
        targetPos = Vector3.zero;

        foreach (GameplayCharacter character in BattleManager.instance.currentCharacters)
        {
            if (BattleManager.instance.currentCharacters.IndexOf(character) == cardCharRow - 1)
            {
                if (!thisCard.isEnemy)
                {
                    BattleManager.instance.currentCharacters[BattleManager.instance.currentCharacters.IndexOf(character)].myCards.Add(this);
                    thisElement = BattleManager.instance.currentCharacters[cardCharRow - 1].thisElement;
                }
                else
                {
                    thisElement = BattleManager.instance.currentEnemies[cardCharRow - 1].thisElement;
                }

            }
        }
        
        SetImage();

    }



    public void SetImage()
    {
        if (thisCard)
        {
            nameText.text = thisCard.cardName;
            imageSprite.sprite = thisCard.cardIllust;

            bool exhausted = false;

            foreach (ActiveAilments ail in cardCharacter.activeAilments)
            {
                if (ail.ailmentType.ailmentNames == AilmentType.Exhausted)
                {
                    exhausted = true;
                }
            }

            manaText.text = (exhausted ? (thisCard.cardMana * 2) : (thisCard.cardMana)).ToString();
            manaText.color = exhausted ? Color.red : Color.white;


            if (thisCard.cardTrueDamage > 0)
            {
                damageText.text = thisCard.cardTrueDamage.ToString();
            }
            else
            {
                damageText.text = thisCard.cardNormalDamage.ToString();
            }

            shieldText.text = thisCard.cardShieldAdd.ToString();
            descText.text = thisCard.cardDesc;

            switch (thisElement)
            {
                case Elements.Neutral:
                    cardBaseSprite.sprite = GameManager.instance.neutralCardBase;
                    break;
                case Elements.Fire:
                    cardBaseSprite.sprite = GameManager.instance.fireCardBase;
                    break;
                case Elements.Water:
                    cardBaseSprite.sprite = GameManager.instance.waterCardBase;
                    break;
                case Elements.Earth:
                    cardBaseSprite.sprite = GameManager.instance.earthCardBase;
                    break;
                case Elements.Light:
                    cardBaseSprite.sprite = GameManager.instance.lightCardBase;
                    break;
                case Elements.Dark:
                    cardBaseSprite.sprite = GameManager.instance.darkCardBase;
                    break;
            }
            //print("Set card " + thisElement);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(thisCard)
        {
            if(isSelected)
            {
                
            }
        }

        transform.DOScale(targetScale, growSpeed);
        transform.DOLocalMove(targetPos, growSpeed);
    }
    void SetTargets()
    {
        selectedTarget.Clear();
        //print("Add target to selected target in " + name);

        if (thisCard.isEnemy)
        {
            cardCharSpeed = BattleManager.instance.currentEnemies[cardCharRow - 1].characterSpeed;

            if (thisCard.cardAttackRow1 && BattleManager.instance.currentCharacters.Count >= 1)
                selectedTarget.Add(BattleManager.instance.currentCharacters[0]);
            if (thisCard.cardAttackRow2 && BattleManager.instance.currentCharacters.Count >= 2)
                selectedTarget.Add(BattleManager.instance.currentCharacters[1]);
            if (thisCard.cardAttackRow3 && BattleManager.instance.currentCharacters.Count >= 3)
                selectedTarget.Add(BattleManager.instance.currentCharacters[2]);
            if (thisCard.cardAttackRow4 && BattleManager.instance.currentCharacters.Count >= 4)
                selectedTarget.Add(BattleManager.instance.currentCharacters[3]);
        }
        else
        {
            cardCharSpeed = BattleManager.instance.currentCharacters[cardCharRow - 1].characterSpeed;

            if (thisCard.cardAttackRow1 && BattleManager.instance.currentEnemies.Count >= 1)
                selectedTarget.Add(BattleManager.instance.currentEnemies[0]);
            if (thisCard.cardAttackRow2 && BattleManager.instance.currentEnemies.Count >= 2)
                selectedTarget.Add(BattleManager.instance.currentEnemies[1]);
            if (thisCard.cardAttackRow3 && BattleManager.instance.currentEnemies.Count >= 3)
                selectedTarget.Add(BattleManager.instance.currentEnemies[2]);
            if (thisCard.cardAttackRow4 && BattleManager.instance.currentEnemies.Count >= 4)
                selectedTarget.Add(BattleManager.instance.currentEnemies[3]);
        }

    }

    public void SelectCard()
    {
        if (isSelected)
        {
            DeSelectCards();
        }
        else
        {
            if (BattleManager.instance.CheckPlayerHasEnoughMana(thisCard.cardMana, cardCharacter))
            {
                BattleManager.instance.SelectCard(this);
                isSelected = true;
                targetScale = normalScale;

                SetTargets();

                GetComponent<AudioSource>().PlayOneShot(selectSound);
            }
            else
            {
                print("Not enough mana!");
                MenuSceneManager.instance.SetNotification("Not enough mana!");
            }
        }
        
    }


    public void DeSelectCards()
    {
        selectedTarget.Clear();
        isSelected = false;

        BattleManager.instance.DeSelectCard(this);

        GetComponent<AudioSource>().PlayOneShot(deselectSound);
    }

    private void OnMouseEnter()
    {
        if (thisCard.isEnemy) return;
        if (BattleManager.instance.currentPhase != 1) return;

        if (!isSelected)
        {
            targetScale = growScale;
            targetPos = posSelect;
        }
    }

    private void OnMouseExit()
    {
        if (thisCard.isEnemy) return;
        if (BattleManager.instance.currentPhase != 1) return;

        if (!isSelected)
        {
            targetScale = normalScale;
            targetPos = Vector3.zero;
        }
    }

    private void OnMouseDown()
    {
        if (thisCard.isEnemy) return;
        if (BattleManager.instance.currentPhase != 1) return;

        SelectCard();
    }
}
