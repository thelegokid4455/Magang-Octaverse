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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Reset Inv to Start")]
    void ResetInventory()
    {
        playerInventory.ResetInventory();
    }

    public void AddToInventory(BodyPart body, BodyPart face, BodyPart weapon, BodyPart top, BodyPart back)
    {
        playerInventory.AddToInventory(body, face, weapon, top, back);
    }

    public void RemoveFromInventory()
    {

    }
}
