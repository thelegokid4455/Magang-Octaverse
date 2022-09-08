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




    //public int ailmentSpeedAdd;
    public bool ailmentManaAdd;
    public bool ailmentManaSteal;
    public bool ailmentManaDestroy;

    public bool cleanseAll;
    public bool drawCard;
    public bool shieldBreak;
}
