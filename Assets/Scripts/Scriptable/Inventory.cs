using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Gameplay/Inventory")]
public class Inventory : ScriptableObject
{
    public int maxCharactersInInventory = 10;

    public List<Character> characterInInventory = new List<Character>();

    public List<Character> startingCharacter = new List<Character>();


    [ContextMenu("Reset Inv to Start")]
    public void ResetInventory()
    {
        characterInInventory.Clear();

        foreach(Character character in startingCharacter)
        {
            characterInInventory.Add(character);
        }
    }

    public void AddToInventory(BodyPart body, BodyPart face, BodyPart weapon, BodyPart top, BodyPart back)
    {
        if (characterInInventory.Count >= maxCharactersInInventory) return;

        var newChar = new Character();
        newChar.characterBody = body;
        newChar.characterFace = face;
        newChar.characterWeapon = weapon;
        newChar.characterTop = top;
        newChar.characterBack = back;

        characterInInventory.Add(newChar);
    }
}
