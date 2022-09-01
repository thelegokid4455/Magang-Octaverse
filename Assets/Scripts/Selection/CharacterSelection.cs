using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public BodyPart characterBody;
    public BodyPart characterFace;
    public BodyPart characterWeapon;
    public BodyPart characterTop;
    public BodyPart characterBack;

    [SerializeField] Image spriteBody;
    [SerializeField] Image spriteFace;
    [SerializeField] Image spriteWeapon;
    [SerializeField] Image spriteTop;
    [SerializeField] Image spriteBack;

    public void SetSelectionCharData(BodyPart body, BodyPart face, BodyPart weapon, BodyPart top, BodyPart back)
    {
        characterBody = body;
        characterFace = face;
        characterWeapon = weapon;
        characterTop = top;
        characterBack = back;

        SetImage();
    }

    void SetImage()
    {
        spriteBody.sprite = characterBody.partImage;
        spriteFace.sprite = characterFace.partImage;
        spriteWeapon.sprite = characterWeapon.partImage;
        spriteTop.sprite = characterTop.partImage;
        spriteBack.sprite = characterBack.partImage;
    }

    public void ClickThisCharacter()
    {
        TeamManager.instance.SelectThisCharacter(this);
    }
}
