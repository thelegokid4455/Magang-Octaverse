using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCharacterIdle : MonoBehaviour
{
    public PlayerCharacterPure thisCharacter;
    public Elements thisElement;

    //stat
    public int characterRow;
    public int characterSpeed;

    //[SerializeField] SpriteRenderer characterSprite;
    [SerializeField] SpriteRenderer spriteBody;
    [SerializeField] SpriteRenderer spriteFace;
    [SerializeField] SpriteRenderer spriteWeapon;
    [SerializeField] SpriteRenderer spriteTop;
    [SerializeField] SpriteRenderer spriteBack;

    public BodyPart characterBody;
    public BodyPart characterFace;
    public BodyPart characterWeapon;
    public BodyPart characterTop;
    public BodyPart characterBack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIdleCharData(BodyPart body, BodyPart face, BodyPart weapon, BodyPart top, BodyPart back)
    {
        characterBody = body;
        characterFace = face;
        characterWeapon = weapon;
        characterTop = top;
        characterBack = back;

        SetIdleImage();
    }

    public void SetIdleImage()
    {
        //set image
        spriteBody.sprite = characterBody.partImage;
        spriteFace.sprite = characterFace.partImage;
        spriteWeapon.sprite = characterWeapon.partImage;
        spriteTop.sprite = characterTop.partImage;
        spriteBack.sprite = characterBack.partImage;
    }
}
