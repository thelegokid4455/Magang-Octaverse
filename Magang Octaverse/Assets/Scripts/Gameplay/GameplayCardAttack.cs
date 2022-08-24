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

    public int cardCharRow;
    public int cardCharSpeed;

    //Battle
    public List<GameplayCharacter> selectedTarget = new List<GameplayCharacter>();


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
        SetImage();
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
                    BattleManager.instance.currentCharacters[BattleManager.instance.currentCharacters.IndexOf(character)].myCards.Add(this);
            }
        }
    }



    void SetImage()
    {
        if (thisCard)
        {
            nameText.text = thisCard.cardName;
            imageSprite.sprite = thisCard.cardIllust;
            manaText.text = thisCard.cardMana.ToString();
            damageText.text = thisCard.cardNormalDamage.ToString();
            shieldText.text = thisCard.cardShieldAdd.ToString();
            descText.text = thisCard.cardDesc;

            switch (thisCard.cardElement)
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
            if (BattleManager.instance.CheckPlayerHasEnoughMana(thisCard.cardMana))
            {
                BattleManager.instance.SelectCard(this);
                isSelected = true;
                targetScale = normalScale;

                SetTargets();
            }
            else
            {
                print("Not enough mana!");
            }
        }
        
    }


    public void DeSelectCards()
    {
        selectedTarget.Clear();
        isSelected = false;

        BattleManager.instance.DeSelectCard(this);
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
