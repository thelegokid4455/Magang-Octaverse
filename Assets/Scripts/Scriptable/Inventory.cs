using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Gameplay/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Character> characterInInventory = new List<Character>();

    public List<Character> startingCharacter = new List<Character>();

    public void ResetInventory()
    {
        characterInInventory.Clear();

        foreach(Character character in startingCharacter)
        {
            characterInInventory.Add(character);
        }
    }
}
