using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pure Character", menuName = "Gameplay/Character Pure")]
public class PlayerCharacterPure : ScriptableObject
{
    public int characterId;
    public string characterJob;
    public Sprite characterSprite;

    public int characterSpeed;

    public bool isEnemy;

    public List<Card> enemyCards = new List<Card>();

    //parts
    public BodyPart characterBody;
    public BodyPart characterFace;
    public BodyPart characterWeapon;
    public BodyPart characterTop;
    public BodyPart characterBack;
}
