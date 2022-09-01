using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitManager : MonoBehaviour
{
    public int buyingPrice;

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

    public void BuyNewCharacter()
    {
        if (!GameManager.instance.HasEnoughMoney(buyingPrice))
        {
            print("not enough money!"); 
            return;
        }

        var invm = InventoryManager.instance;
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
