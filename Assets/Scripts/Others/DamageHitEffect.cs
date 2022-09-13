using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageHitEffect : MonoBehaviour
{
    public TextMeshPro damageText;

    public void SetDamageText(Color color, float damage)
    {
        damageText.color = color;
        if(damage > 0)
        {
            damageText.text = "" + damage;
        }
        else
        {
            damageText.text = "Missed";
        }
    }
}
