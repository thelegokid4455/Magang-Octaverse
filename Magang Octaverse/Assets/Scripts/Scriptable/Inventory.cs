using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Gameplay/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Character> characterInInventory = new List<Character>();

    public Character startingCharacter;

    public void ResetInventory()
    {

    }
}
