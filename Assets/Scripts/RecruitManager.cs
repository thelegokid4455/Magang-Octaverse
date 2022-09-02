using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitManager : MonoBehaviour
{
    public int buyingPrice;
    public int breedingPrice;

    [SerializeField] Character selectedChar1;
    [SerializeField] Character selectedChar2;

    //parent slect
    [SerializeField] int selectedRow;
    [SerializeField] GameObject selectionMenu;

    [SerializeField] Transform listObject;
    [SerializeField] GameObject selectionPrefab;

    //sprite
    [SerializeField] Image spriteBody1;
    [SerializeField] Image spriteFace1;
    [SerializeField] Image spriteWeapon1;
    [SerializeField] Image spriteTop1;
    [SerializeField] Image spriteBack1;

    [SerializeField] Image spriteBody2;
    [SerializeField] Image spriteFace2;
    [SerializeField] Image spriteWeapon2;
    [SerializeField] Image spriteTop2;
    [SerializeField] Image spriteBack2;

    public static RecruitManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //breeding
    public void BreedNewCharacter()
    {
        if (!GameManager.instance.HasEnoughMoney(buyingPrice))
        {
            print("not enough money!");
            return;
        }

        var invm = InventoryManager.instance;

        if (invm.playerInventory.characterInInventory.Count >= invm.playerInventory.maxCharactersInInventory)
        {
            print("not enough space!");
            return;
        }

        invm.AddToInventory(
            ChooseParentPart(Random.Range(0, 2), BodyParts.Body),
            ChooseParentPart(Random.Range(0, 2), BodyParts.Face),
            ChooseParentPart(Random.Range(0, 2), BodyParts.Weapon),
            ChooseParentPart(Random.Range(0, 2), BodyParts.Top),
            ChooseParentPart(Random.Range(0, 2), BodyParts.Back)
            );

        GameManager.instance.AddGold(-buyingPrice);
    }

    BodyPart ChooseParentPart(int whichParent, BodyParts part)
    {
        if (whichParent == 0)
        {
            switch(part)
            {
                case BodyParts.Body: return selectedChar1.characterBody;
                case BodyParts.Face: return selectedChar1.characterFace;
                case BodyParts.Weapon: return selectedChar1.characterWeapon;
                case BodyParts.Top: return selectedChar1.characterTop;
                case BodyParts.Back: return selectedChar1.characterBack;
            }
        }
        else
        {
            switch (part)
            {
                case BodyParts.Body: return selectedChar2.characterBody;
                case BodyParts.Face: return selectedChar2.characterFace;
                case BodyParts.Weapon: return selectedChar2.characterWeapon;
                case BodyParts.Top: return selectedChar2.characterTop;
                case BodyParts.Back: return selectedChar2.characterBack;
            }
        }

        return null;
    }

    public void SelectParent(int which)
    {

    }

    //-parent
    void UpdateInventoryList()
    {
        foreach (Character character in InventoryManager.instance.playerInventory.characterInInventory)
        {
            var newSelection = Instantiate(selectionPrefab);
            newSelection.transform.parent = listObject;
            newSelection.transform.localScale = Vector3.one;
            newSelection.GetComponent<CharacterSelection>().SetSelectionCharData(character.characterBody, character.characterFace, character.characterWeapon, character.characterTop, character.characterBack);
        }
    }

    //buttons
    public void CloseSelectionMenu()
    {
        foreach (Transform child in listObject.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        selectionMenu.SetActive(false);
    }

    public void SelectRow(int row)
    {
        selectedRow = row;
        selectionMenu.SetActive(true);

        UpdateInventoryList();
    }

    public void SelectThisCharacter(CharacterSelection character)
    {
        if (selectedRow == 1)
        {
            selectedChar1.characterBody = character.characterBody;
            selectedChar1.characterFace = character.characterFace;
            selectedChar1.characterWeapon = character.characterWeapon;
            selectedChar1.characterTop = character.characterTop;
            selectedChar1.characterBack = character.characterBack;
        }
        else if (selectedRow == 2)
        {
            selectedChar2.characterBody = character.characterBody;
            selectedChar2.characterFace = character.characterFace;
            selectedChar2.characterWeapon = character.characterWeapon;
            selectedChar2.characterTop = character.characterTop;
            selectedChar2.characterBack = character.characterBack;
        }

        CloseSelectionMenu();

        SetData();

    }

    public void SetData()
    {

        if (!selectedChar1.characterBody)
        {
            selectedChar1.characterBody = GameManager.instance.selectedCharacters[0].characterBody;
            selectedChar1.characterFace = GameManager.instance.selectedCharacters[0].characterFace;
            selectedChar1.characterWeapon = GameManager.instance.selectedCharacters[0].characterWeapon;
            selectedChar1.characterTop = GameManager.instance.selectedCharacters[0].characterTop;
            selectedChar1.characterBack = GameManager.instance.selectedCharacters[0].characterBack;
        }
        if (!selectedChar2.characterBody)
        {
            selectedChar2.characterBody = GameManager.instance.selectedCharacters[1].characterBody;
            selectedChar2.characterFace = GameManager.instance.selectedCharacters[1].characterFace;
            selectedChar2.characterWeapon = GameManager.instance.selectedCharacters[1].characterWeapon;
            selectedChar2.characterTop = GameManager.instance.selectedCharacters[1].characterTop;
            selectedChar2.characterBack = GameManager.instance.selectedCharacters[1].characterBack;
        }

        SetImage();
    }

    void SetImage()
    {
        spriteBody1.sprite = selectedChar1.characterBody.partImage;
        spriteFace1.sprite = selectedChar1.characterFace.partImage;
        spriteWeapon1.sprite = selectedChar1.characterWeapon.partImage;
        spriteTop1.sprite = selectedChar1.characterTop.partImage;
        spriteBack1.sprite = selectedChar1.characterBack.partImage;

        spriteBody2.sprite = selectedChar2.characterBody.partImage;
        spriteFace2.sprite = selectedChar2.characterFace.partImage;
        spriteWeapon2.sprite = selectedChar2.characterWeapon.partImage;
        spriteTop2.sprite = selectedChar2.characterTop.partImage;
        spriteBack2.sprite = selectedChar2.characterBack.partImage;
    }

    //gacha
    public void BuyNewCharacter()
    {
        if (!GameManager.instance.HasEnoughMoney(buyingPrice))
        {
            print("not enough money!"); 
            return;
        }

        var invm = InventoryManager.instance;

        if (invm.playerInventory.characterInInventory.Count >= invm.playerInventory.maxCharactersInInventory)
        {
            print("not enough space!");
            return;
        }

        invm.AddToInventory(
                invm.allCharacterBody[RandomCharacter(invm.allCharacterBody)],
                invm.allCharacterFace[RandomCharacter(invm.allCharacterFace)],
                invm.allCharacterWeapon[RandomCharacter(invm.allCharacterWeapon)],
                invm.allCharacterTop[RandomCharacter(invm.allCharacterTop)],
                invm.allCharacterBack[RandomCharacter(invm.allCharacterBack)]
                );

        GameManager.instance.AddGold(-buyingPrice);
        

    }

    int RandomCharacter(List<BodyPart> partList)
    {
        return Random.Range(0, partList.Count);
    }
}
