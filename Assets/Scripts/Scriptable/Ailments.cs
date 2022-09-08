using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ailment", menuName = "Gameplay/Ailment")]
public class Ailments : ScriptableObject
{
    public AilmentType ailmentNames;
    public Elements ailmentElement;
    public int ailmentMaxTurns;

    public int ailmentTrueDamage;
    public int ailmentNormalDamage;

    public bool isPositif;

    //status add
    public int ailmentHealthPercentAdd;

    public int ailmentShieldPercentAdd;


    public int ailmentManaAdd;
    public int ailmentManaSteal;

    //public int ailmentSpeedAdd;

    public bool cleanseAll;
    public bool drawCard;
    public bool shieldBreak;
}
