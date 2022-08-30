using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body Part", menuName = "Gameplay/Body Part")]
public class BodyPart : ScriptableObject
{
    public int partId;
    public string partName;
    public Sprite partImage;
    [TextArea(2,10)]
    public string partDescription;
    public BodyParts partType;
    public Elements partElement;

    public int partHealthAdd;
    public int partSpeedAdd;
    public int partLuckAdd;

    public Card partCard;
}
