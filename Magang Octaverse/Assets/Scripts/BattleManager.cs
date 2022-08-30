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

    //stats
    [SerializeField] int playerMaxMana;
    int playerCurMana;
    [SerializeField] int playerMaxCard;
    int curPlayerCard;
    [SerializeField] int maxPlayerDiscard;
    int curPlayerDiscard;

    [SerializeField] int enemyMaxMana;
    int enemyCurMana;
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

    [SerializeField] int cardsMaxHeldPerCharacterSingleRotation;


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
        if(currentPhase == 0) //draw
        {

        }
        else if (currentPhase == 1) //preparation
        {
            
        }
        else //battle
        {
            curBattleAnimationTurnTime -= Time.deltaTime;
            if(curBattleAnimationTurnTime <= 0)
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

        //limit
        if (playerCurMana > playerMaxMana) playerCurMana = playerMaxMana;
        if (enemyCurMana > enemyMaxMana) enemyCurMana = enemyMaxMana;

        //UI
        gameRoundText.text = "Round " + currentRound;
        gameReadyButton.SetActive(currentPhase == 1);

        playerManaText.text = playerCurMana + " / " + playerMaxMana;
        playerCardText.text = curPlayerCard.ToString();
        playerDiscardText.text = curPlayerDiscard.ToString();

        enemyManaText.text = enemyCurMana + " / " + enemyMaxMana;
        enemyCardText.text = curEnemyCard.ToString();
        enemyDiscardText.text = curEnemyDiscard.ToString();
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
        /*
        foreach (GameplayCharacter character in currentCharacters.ToList())
        {
            if (character.curHealth <= 0)
            {
                Destroy(character.gameObject);
                currentEnemies.Remove(character);
            }
        }
        */

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
        }

        //SetCharacterPosition();
    }

    void SpawnEnemies()
    {
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
                }
            }
            
        }

        //SetCharacterPosition();
    }

    void SetBattle()
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

        foreach(GameplayCharacter character in currentCharacters)
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

        SetCharacterPosition();

        DrawCard(10);
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

                Destroy(character.gameObject);
                currentCharacters.Remove(character);
            }
        }

        foreach (GameplayCharacter character in currentEnemies.ToList())
        {
            if (character.curHealth <= 0)
            {
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


    void DrawCard(int cardsToDraw)
    {
        print("starting draw phase, round " + currentRound);

        var count = 0;
        while(count < cardsToDraw)
        {
            var indexChar = UnityEngine.Random.Range(0, currentCharacters.Count);
            var indexEnemy = UnityEngine.Random.Range(0, currentCharacters.Count);

            if (curPlayerCard > 0)
            {
                foreach (GameplayCharacter character in currentCharacters)
                {
                    if (currentCharacters.IndexOf(character) == indexChar)
                    {
                        if (character.cardHeldThisDeckRotation < cardsMaxHeldPerCharacterSingleRotation)
                        {
                            SpawnNewCard(character, CheckEmptySlotReturn(character.characterRow - 1));
                            character.cardHeldThisDeckRotation++;
                        }
                        else
                        {
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
        }
        else
        {

            newCard.SendMessage("SetRow", (currentCharacters.IndexOf(character) + 1), SendMessageOptions.DontRequireReceiver);
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
        curPlayerCard = playerMaxCard;

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
        card.transform.parent = CheckEmptySlotSelect(card.cardCharRow - 1);

        //card.transform.DOLocalMove(Vector3.zero, 1);
        //card.transform.DOScale(Vector3.one, 1);

        playerCurMana -= card.thisCard.cardMana;
        //print("Set card " + card.name);
    }

    public void DeSelectCard(GameplayCardAttack card)
    {
        if (currentPhase != 1) return;

        //card.transform.parent = characterPositions[card.cardCharRow].selectedCardPos;
        card.transform.parent = CheckEmptySlotReturn(card.cardCharRow - 1);

        //card.transform.DOLocalMove(Vector3.zero, 1);
        //card.transform.DOScale(Vector3.one, 1);

        playerCurMana += card.thisCard.cardMana;

        selectedCards.Remove(card);

        //print("Removed card " + card.name);
    }

    void MoveCardToPos(GameplayCardAttack card)
    {
        //card.transform.parent = characterPositions[card.cardCharRow].selectedCardPos;
        card.transform.parent = CheckEmptySlotReturn(card.cardCharRow - 1);

        //card.transform.DOLocalMove(Vector3.zero, 1);
        //card.transform.DOScale(Vector3.one, 1);

        //playerCurMana += card.thisCard.cardMana;

        selectedCards.Remove(card);

        //print("Removed card " + card.name);
    }

    public void ButtonReady()
    {
        if (currentPhase != 1) return;

        PhaseAttackEnemy();
        print("starting battle phase, round " + currentRound);
    }

    void PhaseAttackEnemy()
    {
        currentPhase = 2;
        curBattleAnimationTurnTime = battleAnimationTurnTime;

        EnemyUseCard();
    }

    void CardTurnUse()
    {
        if(curBattleTurnCard >= selectedCards.Count)
        {
            curPlayerCard -= playerCardUsed;
            curEnemyCard -= enemyCardUsed;

            curPlayerDiscard += playerCardUsed;
            curEnemyDiscard += enemyCardUsed;

            playerCardUsed = 0;
            enemyCardUsed = 0;

            CardTurnFinish();
            return;
        }

        selectedCards = selectedCards.OrderByDescending(ch => ch.cardCharSpeed).ToList();

        CardTurnUseStats();

        ///////////////////////////////////////////////



        /////////////////////////////////////////////////////////

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

                            currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1].ApplyTrueDamage(selectedCards[curBattleTurnCard].thisCard.cardTrueDamage);
                            currentCharacters[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1].ApplyNormalDamage(selectedCards[curBattleTurnCard].thisCard.cardNormalDamage);
                        }
                        else
                        {
                            if (currentEnemies.Count <= 1) return;

                            currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1].ApplyTrueDamage(selectedCards[curBattleTurnCard].thisCard.cardTrueDamage);
                            currentEnemies[selectedCards[curBattleTurnCard].selectedTarget.IndexOf(targets) + 1].ApplyNormalDamage(selectedCards[curBattleTurnCard].thisCard.cardNormalDamage);
                        }
                    }
                    else
                    {
                        //win game
                    }

                }
                else
                {
                    targets.ApplyTrueDamage(selectedCards[curBattleTurnCard].thisCard.cardTrueDamage);
                    targets.ApplyNormalDamage(selectedCards[curBattleTurnCard].thisCard.cardNormalDamage);
                }
            }
        }
        

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
            

            enemyCardUsed++;

        }
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
            

            playerCardUsed++;
        }
        

        //selectedCards.Remove(selectedCards[curBattleTurnCard]);

        Destroy(selectedCards[curBattleTurnCard].gameObject);
    }
    void CardTurnFinish()
    {
        selectedCards.Clear();
        //player finish attacking
        print("player finish attacking");

        NextRound();
    }

    void EnemyUseCard()
    {
        foreach(GameplayCharacter enemy in currentEnemies)
        {
            var index = UnityEngine.Random.Range(0, enemy.availableCards.Count);

            if (enemyCurMana >= enemy.availableCards[index].cardMana)
            {
                SpawnNewCard(enemy, enemyPositions[currentEnemies.IndexOf(enemy)].selectedCardPos);
                
            }
            else
            {

            }

            
        }
    }

    //END

    void NextRound()
    {
        currentRound++;

        foreach (WavesPerMission waves in currentMission.totalWaves)
        {
            if (currentWave >= currentMission.totalWaves.Count)
            {
                //win
                FinishedBattle();
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

    void FinishedBattle()
    {
        GameManager.instance.ExitBattle();
    }

    //CHECKS
    public bool CheckPlayerHasEnoughMana(int number)
    {
        if (number <= playerCurMana)
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
                        //print("pos has child, restart");
                    }
                }
            }
        }
        
        //print("no pos found");
        return null;

    }

    Transform CheckEmptySlotSelect(int slot)
    {
        //var count = 0;

        if (characterPositions[slot].selectedCardPos.childCount == 0)
        {
            return characterPositions[slot].selectedCardPos;
        }
        else
        {
            print("no pos found");
            return null;
        }
    }

}

[Serializable]
public class CharacterPos
{
    public Transform selectedCardPos;
    public Transform selectedCharacterPos;
    public List<Transform> cardPositions = new List<Transform>();
    //public List<GameplayCardAttack> cardPositionCards = new List<GameplayCardAttack>();
}