using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Gameplay/Card")]
public class Card : ScriptableObject
{
    public int cardId;
    public string cardName;
    public Sprite cardIllust;
    public int cardMana;
    public Elements cardElement;
    [TextArea(2,10)]
    public string cardDesc;

    public bool isEnemy;

    public string cardAttackType = "Attack";
    public AudioClip cardAttackSound;
    public GameObject cardAttackEffect;

    public int cardTrueDamage;
    public int cardNormalDamage;

    //status add
    public int cardHealthAdd;
    public int cardShieldAdd;

    //attack stat
    public bool cardAttackRow1;
    public bool cardAttackRow2;
    public bool cardAttackRow3;
    public bool cardAttackRow4;

    public bool cardHealRow1;
    public bool cardHealRow2;
    public bool cardHealRow3;
    public bool cardHealRow4;
}
