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

    public List<Card> characterCards = new List<Card>();
}
