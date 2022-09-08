using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    public Mission currentMission;

    public int currentRound;
    public int currentPhase; //0 Draw //1 Preparation //2 Battle
    public int currentWave;

    //sounds
    public AudioClip startBattleSound;
    public AudioClip newRoundSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    //stats
    [SerializeField] int playerMaxMana;
    public int playerCurMana;
    [SerializeField] int playerMaxCard;
    int curPlayerCard;
    [SerializeField] int maxPlayerDiscard;
    int curPlayerDiscard;

    public int startingMana;

    [SerializeField] int enemyMaxMana;
    public int enemyCurMana;
    [SerializeField] int enemyMaxCard;
    int curEnemyCard;
    [SerializeField] int maxEnemyDiscard;
    int curEnemyDiscard;

    //battle
    [SerializeField] List<GameplayCardAttack> selectedCards = new List<GameplayCardAttack>();
    [SerializeField] float battleAnimationTurnTime; //(when attacking)
    float curBattleAnimationTurnTime; //(when attacking)
    int curBattleTurnCard; //cards turn to attack (when attacking)

    int playerCardUsed; //card used last turn;
    int enemyCardUsed;

    [SerializeField] int manaAddPerRound;

    public int cardsMaxHeldPerCharacterSingleRotation;

    public bool hasStarted;
    public bool hasFinished;


    //players
    public List<GameplayCharacter> currentCharacters = new List<GameplayCharacter>();

    //Enemies
    public List<GameplayCharacter> currentEnemies = new List<GameplayCharacter>();

    //positions
    [SerializeField] CharacterPos[] characterPositions;
    [SerializeField] CharacterPos[] enemyPositions;

    //UI
    public Transform centerScreen;

    [SerializeField] Text playerManaText;
    [SerializeField] Text playerCardText;
    [SerializeField] Text playerDiscardText;

    [SerializeField] Text enemyManaText;
    [SerializeField] Text enemyCardText;
    [SerializeField] Text enemyDiscardText;

    [SerializeField] Text gameRoundText;
    [SerializeField] GameObject gameReadyButton;

    public static BattleManager instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //SetBattle();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted)
        {
            if (!hasFinished)
            {
                if (currentPhase == 0) //draw
                {

                }
                else if (currentPhase == 1) //preparation
                {

                }
                else //battle
                {
                    curBattleAnimationTurnTime -= Time.deltaTime;
                    if (curBattleAnimationTurnTime <= 0)
                    {
                        CardTurnUse();
                    }
                }

                //pos
                foreach (GameplayCharacter character in currentCharacters)
                {
                    if (character != null)
                    {
                        //character.transform.DOLocalMove(Vector3.zero, 1);
                        //character.transform.DOScale(Vector3.one, 1);
                    }
                }
                foreach (GameplayCharacter character in currentEnemies)
                {
                    if (character != null)
                    {
                        //character.transform.DOLocalMove(Vector3.zero, 1);
                        //character.transform.DOScale(Vector3.one, 1);
                    }
                }
                foreach (GameplayCardAttack card in selectedCards)
                {
                    if (card != null)
                    {
                        card.transform.DOLocalMove(Vector3.zero, 1);
                        card.transform.DOScale(Vector3.one, 1);
                    }
                }
            }
            else
            {

            }

            //limit
            if (playerCurMana > playerMaxMana) playerCurMana = playerMaxMana;
            if (enemyCurMana > enemyMaxMana) enemyCurMana = enemyMaxMana;

            //UI
            gameRoundText.text = "ROUND \n" + currentRound;
            gameReadyButton.SetActive(currentPhase == 1);

            playerManaText.text = playerCurMana + " / " + playerMaxMana;
            playerCardText.text = curPlayerCard.ToString();
            playerDiscardText.text = curPlayerDiscard.ToString();

            enemyManaText.text = enemyCurMana + " / " + enemyMaxMana;
            enemyCardText.text = curEnemyCard.ToString();
            enemyDiscardText.text = curEnemyDiscard.ToString();
        }
        
    }

    //PRE BATTLE
    public void SelectedMission(Mission mission)
    {
        GameManager.instance.EnterBattle();
        currentMission = mission;

        SpawnPlayers();
        SpawnEnemies();

        SetBattle();
    }

    void SpawnPlayers()
    {
        var playerCount = 0;

        foreach (Character character in GameManager.instance.selectedCharacters)
        {
            var newPlayer = Instantiate(GameManager.instance.battleCharacter, Vector3.zero, Quaternion.identity);
            //newEnemy.GetComponent<GameplayCharacter>().SetCharacter(character);
            newPlayer.GetComponent<GameplayCharacter>().SetData(character.characterBody, character.characterFace, character.characterWeapon, character.characterTop, character.characterBack);

            newPlayer.transform.parent = characterPositions[GameManager.instance.selectedCharacters.IndexOf(character)].selectedCharacterPos;

            newPlayer.transform.DOLocalMove(Vector3.zero, 1);
            newPlayer.transform.localRotation = Quaternion.identity;
            newPlayer.transform.DOScale(Vector3.one, 1);

            currentCharacters.Add(newPlayer.GetComponent<GameplayCharacter>());

            playerCount++;
        }

        //SetCharacterPosition();
    }

    void SpawnEnemies()
    {

        var enemyCount = 0;
        foreach (GameplayCharacter character in currentEnemies.ToList())
        {
            if (character.curHealth <= 0)
            {
                Destroy(character.gameObject);
                currentEnemies.Remove(character);
            }
        }

        foreach (WavesPerMission waves in currentMission.totalWaves)
        {
            if(currentMission.totalWaves.IndexOf(waves) == currentWave)
            {
                foreach (PlayerCharacterPure character in waves.enemyToSpawn)
                {
                    var newEnemy = Instantiate(GameManager.instance.battleCharacter, Vector3.zero, Quaternion.identity);
                    newEnemy.GetComponent<GameplayCharacter>().SetCharacter(character);
                    newEnemy.GetComponent<GameplayCharacter>().SetData(character.characterBody, character.characterFace, character.characterWeapon, character.characterTop, character.characterBack);

                    newEnemy.transform.parent = enemyPositions[waves.enemyToSpawn.IndexOf(character)].selectedCharacterPos;

                    newEnemy.transform.DOLocalMove(Vector3.zero, 1);
                    newEnemy.transform.localRotation = Quaternion.identity;
                    newEnemy.transform.DOScale(Vector3.one, 1);

                    currentEnemies.Add(newEnemy.GetComponent<GameplayCharacter>());

                    enemyCount++;
                }
            }
            
        }

        //SetCharacterPosition();
    }

    void SetBattle()
    {
        playerCurMana = startingMana;
        curPlayerCard = playerMaxCard;

        enemyCurMana = startingMana;
        curEnemyCard = enemyMaxCard;

        currentRound = 1;
        currentPhase = 0;
        currentWave = 0;

        curPlayerDiscard = 0;
        curEnemyDiscard = 0;

        curBattleTurnCard = 0;

        foreach (GameplayCharacter character in currentCharacters)
        {
            //character.SetData();
            character.cardHeldThisDeckRotation = 0;
        }
        foreach (GameplayCharacter character in currentEnemies)
        {
            //character.SetData();
            character.cardHeldThisDeckRotation = 0;
        }

        print("starting battle, round " + currentRound);

        hasStarted = true;


        GetComponent<AudioSource>().PlayOneShot(startBattleSound);

        SetCharacterPosition();

        DrawCard(10);
    }
    void ResetStats()
    {
        playerCurMana = playerMaxMana;
        curPlayerCard = playerMaxCard;

        enemyCurMana = enemyMaxMana;
        curEnemyCard = enemyMaxCard;

        currentRound = 1;
        currentPhase = 0;
        currentWave = 0;

        curPlayerDiscard = 0;
        curEnemyDiscard = 0;

        curBattleTurnCard = 0;


        print("All data reset :)");
    }

    void SetCharacterPosition()
    {
        //Remove dead ones
        foreach (GameplayCharacter character in currentCharacters.ToList())
        {
            if (character.curHealth <= 0)
            {
                //remove dead cards
                /*
                foreach (Transform cardPos in characterPositions[currentCharacters.IndexOf(character)].cardPositions)
                {
                    if(cardPos.childCount > 0)
                    {
                        Destroy(cardPos.GetChild(0).gameObject);
                    }
                }
                */

                foreach(GameplayCardAttack card in character.myCards.ToList())
                {
                    if (card != null)
                    {
                        Destroy(card.gameObject);
                    }
                    else
                    {
                        character.myCards.Remove(card);
                    }
                    
                }

                curPlayerDiscard -= character.cardUsed;
                curPlayerCard -= character.cardLeft;

                Destroy(character.gameObject);
                currentCharacters.Remove(character);
            }
        }

        foreach (GameplayCharacter character in currentEnemies.ToList())
        {
            if (character.curHealth <= 0)
            {

                curEnemyDiscard -= character.cardUsed;
                curEnemyCard -= character.cardLeft;

                Destroy(character.gameObject);
                currentEnemies.Remove(character);
            }
        }

        //sort to front
        foreach (GameplayCharacter character in currentCharacters.ToList())
        {
            character.characterRow = currentCharacters.IndexOf(character) + 1;

            character.transform.parent = characterPositions[character.characterRow-1].selectedCharacterPos;
            character.transform.DOLocalMove(Vector3.zero, 1);
            character.transform.DOScale(Vector3.one, 1);

            foreach(GameplayCardAttack card in character.myCards.ToList())
            {
                if(card != null)
                {
                    card.cardCharRow = character.characterRow;
                    MoveCardToPos(card);
                }
                else
                {
                    character.myCards.Remove(card);
                }
            }
            
        }
        foreach (GameplayCharacter character in currentEnemies.ToList())
        {
            character.characterRow = currentEnemies.IndexOf(character) + 1;

            character.transform.parent = enemyPositions[character.characterRow - 1].selectedCharacterPos;
            character.transform.DOLocalMove(Vector3.zero, 1);
            character.transform.DOScale(Vector3.one, 1);
        }
    }

    static int SortBySpeed(GameplayCharacter p1, GameplayCharacter p2)
    {
        return p1.characterSpeed.CompareTo(p2.characterSpeed);
    }


    public void DrawCard(int cardsToDraw)
    {
        print("starting draw phase, round " + currentRound);


        var count = 0;
        while(count < cardsToDraw)
        {
            var indexChar = UnityEngine.Random.Range(0, currentCharacters.Count);
            var lastIndex = 0;
            //var indexEnemy = UnityEngine.Random.Range(0, currentCharacters.Count);

            if (curPlayerCard > 0)
            {
                foreach (GameplayCharacter character in currentCharacters)
                {
                    if (currentCharacters.IndexOf(character) == indexChar)
                    {
                        lastIndex = indexChar;
                        if (character.cardHeldThisDeckRotation < cardsMaxHeldPerCharacterSingleRotation)
                        {
                            SpawnNewCard(character, CheckEmptySlotReturn(character.characterRow - 1));
                            character.cardHeldThisDeckRotation++;

                            character.cardLeft--;
                            character.cardInHand++;

                            curPlayerCard --;
                        }
                        else
                        {
                            while (indexChar != lastIndex)
                            {
                                indexChar = UnityEngine.Random.Range(0, currentCharacters.Count);

                                SpawnNewCard(character, CheckEmptySlotReturn(character.characterRow - 1));
                                character.cardHeldThisDeckRotation++;

                                character.cardLeft--;
                                character.cardInHand++;

                                curPlayerCard--;

                                lastIndex = indexChar;
                            }
                            //return;
                        }
                    }
                    
                }
            }
            else
            {
                ReShuffleDeckPlayer();
            }

            count++;
        }

        foreach(GameplayCharacter character in currentCharacters)
        {
            foreach (GameplayCardAttack cards in character.myCards)
            {
                cards.SetImage();
            }
        }

        
        

        PreparationPhase();
    }

    void SpawnNewCard(GameplayCharacter character,  Transform cardPos)
    {
        var index = UnityEngine.Random.Range(0, character.availableCards.Count);

        var newCard = Instantiate(GameManager.instance.battleCard, Vector3.zero, Quaternion.identity);
        newCard.SendMessage("SetCard", character.availableCards[index], SendMessageOptions.DontRequireReceiver);

        if (character.availableCards[index].isEnemy)
        {
            // enemyPositions[currentEnemies.IndexOf(character)].selectedCardPos;

            newCard.SendMessage("SetRow", (currentEnemies.IndexOf(character) + 1), SendMessageOptions.DontRequireReceiver);
            selectedCards.Add(newCard.GetComponent<GameplayCardAttack>());

            enemyCurMana -= newCard.GetComponent<GameplayCardAttack>().thisCard.cardMana;

            newCard.GetComponent<GameplayCardAttack>().cardCharacter = currentEnemies[newCard.GetComponent<GameplayCardAttack>().cardCharRow - 1];
        }
        else
        {

            newCard.SendMessage("SetRow", (currentCharacters.IndexOf(character) + 1), SendMessageOptions.DontRequireReceiver);

            newCard.GetComponent<GameplayCardAttack>().cardCharacter = currentCharacters[newCard.GetComponent<GameplayCardAttack>().cardCharRow - 1];
        }

        newCard.SendMessage("SetData", SendMessageOptions.DontRequireReceiver);
        newCard.SendMessage("SetTargets", SendMessageOptions.DontRequireReceiver);

        newCard.transform.parent = cardPos;

        newCard.transform.DOLocalMove(Vector3.zero, 1);
        newCard.transform.localRotation = Quaternion.identity;
        newCard.transform.DOScale(Vector3.one, 1);

        
    }

    void PreparationPhase()
    {
        print("starting preparation phase, round " + currentRound);

        currentPhase = 1;

        
    }

    void ReShuffleDeckPlayer()
    {
        curPlayerDiscard = 0;

        var totalCards = 0;
        foreach(GameplayCharacter character in currentCharacters)
        {
            foreach(GameplayCardAttack card in character.myCards)
            {
                totalCards += 1;
            }
        }

        curPlayerCard = (cardsMaxHeldPerCharacterSingleRotation * currentCharacters.Count) - totalCards;

        foreach (GameplayCharacter character in currentCharacters)
        {
            character.cardHeldThisDeckRotation = 0;

        }

        DrawCard(1);
    }

    //BATTLING

    public void SelectCard(GameplayCardAttack card)
    {
        if (currentPhase != 1) return;

        selectedCards.Add(card);

        //card.transform.parent = characterPositions[card.cardCharRow - 1].selectedCardPos;
        card.transform.parent = CheckEmptySlotSelect(true, card.cardCharRow - 1);

        //card.transform.DOLocalMove(Vector3.zero, 1);
        //card.transform.DOScale(Vector3.one, 1);

        bool exhausted = false;
        foreach (ActiveAilments ail in card.cardCharacter.activeAilments)
        {
            if (ail.ailmentType.ailmentNames == AilmentType.Exhausted)
            {
                exhausted = true;
            }
        }

        playerCurMana -= card.thisCard.cardMana * (exhausted? 2 : 1);
    }

    public void DeSelectCard(GameplayCardAttack card)
    {
        if (currentPhase != 1) return;

        //card.transform.parent = characterPositions[card.cardCharRow].selectedCardPos;
        card.transform.parent = CheckEmptySlotReturn(card.cardCharRow - 1);

        //card.transform.DOLocalMove(Vector3.zero, 1);
        //card.transform.DOScale(Vector3.one, 1);

        bool exhausted = false;
        foreach (ActiveAilments ail in card.cardCharacter.activeAilments)
        {
            if (ail.ailmentType.ailmentNames == AilmentType.Exhausted)
            {
                exhausted = true;
            }
        }

        playerCurMana += card.thisCard.cardMana * (exhausted ? 2 : 1);

        selectedCards.Remove(card);
    }

    void MoveCardToPos(GameplayCardAttack card)
    {
        //card.transform.parent = characterPositions[card.cardCharRow].selectedCardPos;
        card.transform.parent = CheckEmptySlotReturn(card.cardCharRow - 1);

        //card.transform.DOLocalMove(Vector3.zero, 1);
        //card.transform.DOScale(Vector3.one, 1);

        //playerCurMana += card.thisCard.cardMana;

        selectedCards.Remove(card);
    }

    public void ButtonReady()
    {
        if (currentPhase != 1) return;

        PhaseAttackEnemy();
        //print("starting battle phase, round " + currentRound);
    }

    void PhaseAttackEnemy()
    {
        currentPhase = 2;
        curBattleAnimationTurnTime = battleAnimationTurnTime;

        EnemyUseCard();
    }

    void CardTurnUse()
    {
        if (hasFinished) return;

        if(curBattleTurnCard >= selectedCards.Count)
        {
            //curPlayerCard -= playerCardUsed;
            //curEnemyCard -= enemyCardUsed;

            curPlayerDiscard += playerCardUsed;
            curEnemyDiscard += enemyCardUsed;

            playerCardUsed = 0;
            enemyCardUsed = 0;

            CardTurnFinish();
            return;
        }

        selectedCards = selectedCards.OrderByDescending(ch => ch.cardCharSpeed).ToList();

        CardTurnUseStats();

        curBattleTurnCard++;
        curBattleAnimationTurnTime = battleAnimationTurnTime;
    }
    void CardTurnUseStats()
    {
        //remove dead chars card
        foreach(GameplayCardAttack card in selectedCards.ToList())
        {
            if (card.thisCard.isEnemy)
            {
                if (currentEnemies[card.cardCharRow - 1].curHealth <= 0)
                {
                    if (card != null)
                    {
                        Destroy(card.gameObject);
                        selectedCards.Remove(card);
                    }
                }
            }
            else
            {
                if (currentCharacters[card.cardCharRow - 1].curHealth <= 0)
                {
                    if(card != null)
                    {
                        Destroy(card.gameObject);
                        selectedCards.Remove(card);
                    }
                }
            }
        }

        //test set target each card turn
        foreach (GameplayCharacter character in currentEnemies.ToList())
        {
            foreach(GameplayCardAttack card in character.myCards)
            {
                card.SendMessage("SetTarget", SendMessageOptions.DontRequireReceiver);
            }
        }

        if(selectedCards[curBattleTurnCard].selectedTarget.Count > 0)
        {
            foreach (GameplayCharacter targets in selectedCards[curBattleTurnCard].selectedTarget.ToList())
            {
                if (targets.curHealth <= 0)
                {

                    if (currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1])
                    {
                        if (selectedCards[curBattleTurnCard].thisCard.isEnemy)
                        {
                            if (currentCharacters.Count <= 1) return;

                            currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1].ApplyTrueDamage(selectedCards[curBattleTurnCard].thisCard.cardElement, selectedCards[curBattleTurnCard].thisCard.cardTrueDamage);
                            currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1].ApplyNormalDamage(selectedCards[curBattleTurnCard].thisCard.cardElement, selectedCards[curBattleTurnCard].thisCard.cardNormalDamage);

                            
                        }
                        else
                        {
                            if (currentEnemies.Count <= 1) return;

                            currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1].ApplyTrueDamage(selectedCards[curBattleTurnCard].thisCard.cardElement, selectedCards[curBattleTurnCard].thisCard.cardTrueDamage);
                            currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1].ApplyNormalDamage(selectedCards[curBattleTurnCard].thisCard.cardElement, selectedCards[curBattleTurnCard].thisCard.cardNormalDamage);

                            
                            
                        }
                    }
                    else
                    {
                        //win game
                        FinishedBattle(true);
                        return;
                    }

                }
                else
                {
                    targets.ApplyTrueDamage(selectedCards[curBattleTurnCard].thisCard.cardElement, selectedCards[curBattleTurnCard].thisCard.cardTrueDamage);
                    targets.ApplyNormalDamage(selectedCards[curBattleTurnCard].thisCard.cardElement, selectedCards[curBattleTurnCard].thisCard.cardNormalDamage);
                }
            }
        }

        var curCardAil = selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.activatedAilment;
        var curCardReq = selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.activeRequirement;


        if (selectedCards[curBattleTurnCard].thisCard.isEnemy)
        {
            if (currentEnemies[selectedCards[curBattleTurnCard].cardCharRow - 1].curHealth <= 0)
            {
                Destroy(selectedCards[curBattleTurnCard].gameObject);
                return;
            }

            currentEnemies[selectedCards[curBattleTurnCard].cardCharRow - 1].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
            currentEnemies[selectedCards[curBattleTurnCard].cardCharRow - 1].AddShield(selectedCards[curBattleTurnCard].thisCard.cardShieldAdd);

            if (selectedCards[curBattleTurnCard].thisCard.cardHealRow1) 
            { 
                if (currentEnemies.Count >= 1) 
                    currentEnemies[0].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
                else return;
            }
            
            if (selectedCards[curBattleTurnCard].thisCard.cardHealRow2) 
            { 
                if (currentEnemies.Count >= 2) 
                    currentEnemies[1].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
                else return;
            }
            
            if (selectedCards[curBattleTurnCard].thisCard.cardHealRow3) 
            { 
                if (currentEnemies.Count >= 3) 
                    currentEnemies[2].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
                else return;
            }
            
            if (selectedCards[curBattleTurnCard].thisCard.cardHealRow4) 
            { 
                if (currentEnemies.Count >= 4) 
                    currentEnemies[3].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
                else return;
            }

            //ailments
            foreach (GameplayCharacter targets in selectedCards[curBattleTurnCard].selectedTarget.ToList())
            {
                foreach (AilmentType charAilment in curCardReq)
                {
                    //add ailment
                    if (curCardAil != null)
                    {
                        //no requirements
                        if (charAilment == AilmentType.None)
                        {
                            print("no ailment requirement");

                            //instant
                            foreach (Ailments ail in curCardAil)
                            {
                                currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].ApplyAilmentInstant(ail, true);
                            }

                            //check same ailments
                            if (HasSameAilment(currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments, curCardAil[curCardReq.IndexOf(charAilment)]))
                            {
                                foreach (ActiveAilments ail in currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments.ToList())
                                {
                                    if (ail.ailmentType == curCardAil[curCardReq.IndexOf(charAilment)])
                                    {
                                        if (selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.needShield)
                                        {
                                            if (targets.curShield > 0)
                                            {
                                                //has ailments same
                                                ail.ailmentCurrentTurns = curCardAil[curCardReq.IndexOf(charAilment)].ailmentMaxTurns;
                                            }
                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.needShield)
                                {
                                    if (targets.curShield > 0)
                                    {
                                        currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].AddAilment(curCardAil[curCardReq.IndexOf(charAilment)]);
                                    }
                                }
                                else
                                {
                                    currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].AddAilment(curCardAil[curCardReq.IndexOf(charAilment)]);
                                }
                            }


                        }
                        //has requirements
                        else
                        {
                            print("ailments has requirement");
                            //character buff
                            foreach (ActiveAilments buff in currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments.ToList())
                            {
                                if (buff.ailmentType.ailmentNames == curCardReq[curCardReq.IndexOf(charAilment)])
                                {

                                    //instant
                                    foreach (Ailments ail in curCardAil)
                                    {
                                        currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].ApplyAilmentInstant(ail, true);
                                    }

                                    //check same ailments
                                    if (HasSameAilment(currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments, curCardAil[curCardReq.IndexOf(charAilment)]))
                                    {
                                        foreach (ActiveAilments ail in currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments.ToList())
                                        {
                                            if (ail.ailmentType == curCardAil[curCardReq.IndexOf(charAilment)])
                                            {

                                                if (selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.needShield)
                                                {
                                                    if (targets.curShield > 0)
                                                    {
                                                        //has ailments same
                                                        ail.ailmentCurrentTurns = curCardAil[curCardReq.IndexOf(charAilment)].ailmentMaxTurns;

                                                        print("ailments add turn");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.needShield)
                                        {
                                            if (targets.curShield > 0)
                                            {
                                                currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].AddAilment(curCardAil[curCardReq.IndexOf(charAilment)]);
                                            }
                                        }
                                        else
                                        {
                                            currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].AddAilment(curCardAil[curCardReq.IndexOf(charAilment)]);
                                        }
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        print("not ailments");
                        //no ailments
                    }
                }


            }


            selectedCards[curBattleTurnCard].cardCharacter.cardUsed++;
            selectedCards[curBattleTurnCard].cardCharacter.cardInHand--;
            enemyCardUsed++;

        }
        ///////////////
        else
        {
            if (currentCharacters[selectedCards[curBattleTurnCard].cardCharRow - 1].curHealth <= 0)
            {
                Destroy(selectedCards[curBattleTurnCard].gameObject);
                return;
            }

            currentCharacters[selectedCards[curBattleTurnCard].cardCharRow - 1].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
            currentCharacters[selectedCards[curBattleTurnCard].cardCharRow - 1].AddShield(selectedCards[curBattleTurnCard].thisCard.cardShieldAdd);

            if (selectedCards[curBattleTurnCard].thisCard.cardHealRow1) 
            {
                if (currentCharacters.Count >= 1)
                    currentCharacters[0].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
                else return;
            }
            
            if (selectedCards[curBattleTurnCard].thisCard.cardHealRow2) 
            {
                if (currentCharacters.Count >= 2) 
                    currentCharacters[1].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
                else return;
            }
            
            if (selectedCards[curBattleTurnCard].thisCard.cardHealRow3) 
            {
                if (currentCharacters.Count >= 3) 
                    currentCharacters[2].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
                else return;
            }
            
            if (selectedCards[curBattleTurnCard].thisCard.cardHealRow4) 
            {
                if (currentCharacters.Count >= 4) 
                    currentCharacters[3].AddHealth(selectedCards[curBattleTurnCard].thisCard.cardHealthAdd);
                else return;
            }

            //ailments
            foreach (GameplayCharacter targets in selectedCards[curBattleTurnCard].selectedTarget.ToList())
            {
                foreach(AilmentType charAilment in curCardReq)
                {
                    //add ailment
                    if (curCardAil != null)
                    {
                        //no requirements
                        if (charAilment == AilmentType.None)
                        {
                            print("no ailment requirement");

                            //instant
                            foreach (Ailments ail in curCardAil)
                            {
                                currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].ApplyAilmentInstant(ail, false);
                            }

                            //check same ailments
                            if (HasSameAilment(currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments, curCardAil[curCardReq.IndexOf(charAilment)]))
                            {
                                foreach (ActiveAilments ail in currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments.ToList())
                                {
                                    if (ail.ailmentType == curCardAil[curCardReq.IndexOf(charAilment)])
                                    {
                                        if (selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.needShield)
                                        {
                                            if (targets.curShield > 0)
                                            {
                                                //has ailments same
                                                ail.ailmentCurrentTurns = curCardAil[curCardReq.IndexOf(charAilment)].ailmentMaxTurns;
                                            }
                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.needShield)
                                {
                                    if (targets.curShield > 0)
                                    {
                                        currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].AddAilment(curCardAil[curCardReq.IndexOf(charAilment)]);
                                    }
                                }
                                else
                                {
                                    currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].AddAilment(curCardAil[curCardReq.IndexOf(charAilment)]);
                                }
                            }


                        }
                        //has requirements
                        else
                        {
                            print("ailments has requirement");
                            //character buff
                            foreach (ActiveAilments buff in currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments.ToList())
                            {
                                if (buff.ailmentType.ailmentNames == curCardReq[curCardReq.IndexOf(charAilment)])
                                {

                                    //instant
                                    foreach(Ailments ail in curCardAil)
                                    {
                                        currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].ApplyAilmentInstant(ail, false);
                                    }

                                    //check same ailments
                                    if (HasSameAilment(currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments, curCardAil[curCardReq.IndexOf(charAilment)]))
                                    {
                                        foreach (ActiveAilments ail in currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].activeAilments.ToList())
                                        {
                                            if (ail.ailmentType == curCardAil[curCardReq.IndexOf(charAilment)])
                                            {

                                                if (selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.needShield)
                                                {
                                                    if (targets.curShield > 0)
                                                    {
                                                        //has ailments same
                                                        ail.ailmentCurrentTurns = curCardAil[curCardReq.IndexOf(charAilment)].ailmentMaxTurns;

                                                        print("ailments add turn");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (selectedCards[curBattleTurnCard].thisCard.cardAilmentRequirements.needShield)
                                        {
                                            if (targets.curShield > 0)
                                            {
                                                currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].AddAilment(curCardAil[curCardReq.IndexOf(charAilment)]);
                                            }
                                        }
                                        else
                                        {
                                            currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets)].AddAilment(curCardAil[curCardReq.IndexOf(charAilment)]);
                                        }
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        print("not ailments");
                        //no ailments
                    }
                }
            }

            selectedCards[curBattleTurnCard].cardCharacter.cardUsed++;
            selectedCards[curBattleTurnCard].cardCharacter.cardInHand--;
            playerCardUsed++;


        }


        selectedCards[curBattleTurnCard].cardCharacter.Attack(selectedCards[curBattleTurnCard].thisCard);
        //selectedCards.Remove(selectedCards[curBattleTurnCard]);

        foreach (GameplayCharacter character in currentCharacters)
        {
            character.AilmentShow();
        }
        foreach (GameplayCharacter character in currentEnemies)
        {
            character.AilmentShow();
        }

        Destroy(selectedCards[curBattleTurnCard].gameObject);
    }

    bool HasSameAilment(List<ActiveAilments> checkList, Ailments checkType)
    {
        //check same ailments
        foreach (ActiveAilments ail in checkList)
        {
            if (ail.ailmentType.ailmentNames == checkType.ailmentNames)
            {
                return true;
            }
        }
        return false;
    }

    void CardTurnFinish()
    {
        if (hasFinished) return;

        selectedCards.Clear();

        //player finish attacking

        ApplyAilments();

        NextRound();
    }

    void EnemyUseCard()
    {
        foreach(GameplayCharacter enemy in currentEnemies)
        {
            var index = UnityEngine.Random.Range(0, enemy.availableCards.Count);

            bool exhausted = false;
            foreach(ActiveAilments ail in enemy.activeAilments)
            {
                if(ail.ailmentType.ailmentNames == AilmentType.Exhausted)
                {
                    exhausted = true;
                }
            }

            if (enemyCurMana >= (enemy.availableCards[index].cardMana * (exhausted? 2 : 1)))
            {
                //SpawnNewCard(enemy, enemyPositions[currentEnemies.IndexOf(enemy)].selectedCardPos);
                SpawnNewCard(enemy, CheckEmptySlotSelect(false, currentEnemies[currentEnemies.IndexOf(enemy)].characterRow - 1));

                enemy.cardUsed++;

                enemy.cardInHand--;
                enemy.cardLeft--;

                curEnemyCard--;
                
            }
            else
            {

            }

            
        }
    }

    //END

    void ApplyAilments()
    {
        foreach (GameplayCharacter character in currentCharacters)
        {
            character.ApplyAilment(false);
        }
        foreach (GameplayCharacter character in currentEnemies)
        {
            character.ApplyAilment(true);
        }
    }

    void NextRound()
    {
        if (hasFinished) return;


        currentRound++;


        foreach (GameplayCharacter character in currentCharacters)
        {
            character.AilmentShow();
        }
        foreach (GameplayCharacter character in currentEnemies)
        {
            character.AilmentShow();
        }

        GetComponent<AudioSource>().PlayOneShot(newRoundSound);

        foreach (WavesPerMission waves in currentMission.totalWaves)
        {
            if (currentWave >= currentMission.totalWaves.Count)
            {
                //win
                FinishedBattle(true);
                return;
            }
            else
            {
                if (currentEnemies.Count <= 0)
                {
                    currentWave++;
                    SpawnEnemies();
                }
                else
                {
                    var count = 0;
                    foreach (GameplayCharacter character in currentEnemies)
                    {
                        if (character.curHealth <= 0) count++;

                    }
                    if (count >= currentEnemies.Count)
                    {
                        currentWave++;
                        SpawnEnemies();
                    }
                }
            }
        }

        if (currentCharacters.Count <= 0)
        {
            FinishedBattle(false);
            return;
        }
        if (currentEnemies.Count <= 0)
        {
            FinishedBattle(true);
            return;
        }

        playerCurMana += manaAddPerRound;
        if (playerCurMana > playerMaxMana) playerCurMana = playerMaxMana;

        enemyCurMana += manaAddPerRound;
        if (enemyCurMana > enemyMaxMana) enemyCurMana = enemyMaxMana;

        foreach(GameplayCharacter character in currentCharacters)
        {
            character.curShield = 0;
        }
        foreach (GameplayCharacter character in currentEnemies)
        {
            character.curShield = 0;
        }

        currentPhase = 0;

        curBattleTurnCard = 0;

        SetCharacterPosition();

        DrawCard(2);
    }

    public void FinishedBattle(bool isWin)
    {
        //GameManager.instance.ExitBattle();
        hasStarted = false;
        hasFinished = true;

        //clear players, cards and enemies
        foreach (GameplayCharacter character in currentCharacters)
        {
            foreach (GameplayCardAttack cards in character.myCards.ToList())
            {
                if (cards != null)
                    Destroy(cards.gameObject);

            }

            character.myCards.Clear();
            if (character != null)
                Destroy(character.gameObject);

        }

        foreach (GameplayCharacter character in currentEnemies)
        {
            foreach (GameplayCardAttack cards in character.myCards.ToList())
            {
                if (cards != null)
                    Destroy(cards.gameObject);

            }

            character.myCards.Clear();
            if (character != null)
                Destroy(character.gameObject);

        }
        foreach (GameplayCardAttack cards in selectedCards)
        {
            if (cards != null)
                Destroy(cards.gameObject);

        }
        currentCharacters.Clear();
        currentEnemies.Clear();
        selectedCards.Clear();

        //reset
        ResetStats();

        //sound
        if (isWin)
        {
            GetComponent<AudioSource>().PlayOneShot(winSound);

            //add gold if win
            GameManager.instance.AddGold(currentMission.missionReward);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(loseSound);
        }

        //set UI
        MenuSceneManager.instance.FinishBattle(isWin);

        //reset mission
        currentMission = null;

        SaveManager.instance.SaveGameFile();
    }

    //CHECKS
    public bool CheckPlayerHasEnoughMana(int number, GameplayCharacter character)
    {
        bool exhausted = false;
        foreach (ActiveAilments ail in character.activeAilments)
        {
            if (ail.ailmentType.ailmentNames == AilmentType.Exhausted)
            {
                exhausted = true;
            }
        }

        if ((number * (exhausted ? 2 : 1)) <= playerCurMana)
        {
            return true;
        }
        return false;
    }

    Transform CheckEmptySlotReturn(int slot)
    {
        var count = 0;

        while(count < characterPositions[slot].cardPositions.Count)
        {
            foreach (Transform pos in characterPositions[slot].cardPositions)
            {
                if (characterPositions[slot].cardPositions.IndexOf(pos) == count)
                {
                    if (pos.childCount == 0)
                    {
                        return pos;
                    }
                    else
                    {
                        count++;
                    }
                }
            }
        }

        return null;

    }

    Transform CheckEmptySlotSelect(bool isPlayer, int slot)
    {
        var count = 0;

        if(isPlayer)
        {
            while (count < characterPositions[slot].selectedCardPos.Count)
            {
                foreach (Transform pos in characterPositions[slot].selectedCardPos)
                {
                    if (characterPositions[slot].selectedCardPos.IndexOf(pos) == count)
                    {
                        if (pos.childCount == 0)
                        {
                            return pos;
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
            }
        }
        else
        {
            while (count < enemyPositions[slot].selectedCardPos.Count)
            {
                foreach (Transform pos in enemyPositions[slot].selectedCardPos)
                {
                    if (enemyPositions[slot].selectedCardPos.IndexOf(pos) == count)
                    {
                        if (pos.childCount == 0)
                        {
                            return pos;
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
            }
        }

        return null;
    }

}

[Serializable]
public class CharacterPos
{
    public List<Transform> selectedCardPos = new List<Transform>();
    public Transform selectedCharacterPos;
    public List<Transform> cardPositions = new List<Transform>();
    //public List<GameplayCardAttack> cardPositionCards = new List<GameplayCardAttack>();
}