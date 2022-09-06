using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;

    public List<BodyPart> allCharacterBody = new List<BodyPart>();
    public List<BodyPart> allCharacterFace = new List<BodyPart>();
    public List<BodyPart> allCharacterWeapon = new List<BodyPart>();
    public List<BodyPart> allCharacterTop = new List<BodyPart>();
    public List<BodyPart> allCharacterBack = new List<BodyPart>();

    public static InventoryManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetID();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Reset Inv to Start")]
    public void ResetInventory()
    {
        playerInventory.ResetInventory();
    }

    public void AddToInventory(Character character)
    {
        playerInventory.AddToInventory(character.characterBody, character.characterFace, character.characterWeapon, character.characterTop, character.characterBack);
    }

    public void RemoveFromInventory()
    {

    }

    [ContextMenu("Set Item ID")]
    public void SetID()
    {
        foreach (BodyPart part in allCharacterBody)
        {
            part.partId = allCharacterBody.IndexOf(part);
        }
        foreach (BodyPart part in allCharacterFace)
        {
            part.partId = allCharacterFace.IndexOf(part);
        }
        foreach (BodyPart part in allCharacterWeapon)
        {
            part.partId = allCharacterWeapon.IndexOf(part);
        }
        foreach (BodyPart part in allCharacterTop)
        {
            part.partId = allCharacterTop.IndexOf(part);
        }
        foreach (BodyPart part in allCharacterBack)
        {
            part.partId = allCharacterBack.IndexOf(part);
        }
    }
}
