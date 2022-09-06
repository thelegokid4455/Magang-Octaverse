using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] bool keepResetting;
    bool dontSaveLast;

    public static SaveManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("SavedFile") > 0)
        {
            LoadGameFile();
        }
        else
        {
            ResetSaveFile(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGameFile();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGameFile();
        }
#endif
    }

    [ContextMenu("Save Game File")]
    public void SaveGameFile()
    {
        InventoryManager.instance.SetID();

        //gold
        PlayerPrefs.SetInt("PlayerGold", GameManager.instance.currentMoney);

        //selected chars
        foreach (Character character in GameManager.instance.selectedCharacters)
        {
            var name = "SelectedCharSlot" + GameManager.instance.selectedCharacters.IndexOf(character);

            PlayerPrefs.SetInt(name + "Body", character.characterBody.partId);
            PlayerPrefs.SetInt(name + "Face", character.characterFace.partId);
            PlayerPrefs.SetInt(name + "Weapon", character.characterWeapon.partId);
            PlayerPrefs.SetInt(name + "Top", character.characterTop.partId);
            PlayerPrefs.SetInt(name + "Back", character.characterBack.partId);
        }

        //characters in inventory
        foreach (Character character in InventoryManager.instance.playerInventory.characterInInventory)
        {
            var name = "CharSlot" + InventoryManager.instance.playerInventory.characterInInventory.IndexOf(character);

            PlayerPrefs.SetInt(name + "Body", character.characterBody.partId);
            PlayerPrefs.SetInt(name + "Face", character.characterFace.partId);
            PlayerPrefs.SetInt(name + "Weapon", character.characterWeapon.partId);
            PlayerPrefs.SetInt(name + "Top", character.characterTop.partId);
            PlayerPrefs.SetInt(name + "Back", character.characterBack.partId);
        }
        PlayerPrefs.SetInt("CharactersOwned", InventoryManager.instance.playerInventory.characterInInventory.Count);

        PlayerPrefs.SetInt("SavedFile", 1);

        print("Saved at " + System.DateTime.Now);
    }

    [ContextMenu("Load Save File")]
    public void LoadGameFile()
    {
        InventoryManager.instance.SetID();
        InventoryManager.instance.ResetInventory();

        //gold
        GameManager.instance.currentMoney = PlayerPrefs.GetInt("PlayerGold", 0);

        //characters in inventory
        var count = InventoryManager.instance.playerInventory.startingCharacter.Count;

        while (count < PlayerPrefs.GetInt("CharactersOwned", InventoryManager.instance.playerInventory.startingCharacter.Count))
        {
            var name = "CharSlot" + count;

            var newCharacter = new Character();
            newCharacter.characterBody = InventoryManager.instance.allCharacterBody[PlayerPrefs.GetInt(name + "Body", 0)];
            newCharacter.characterFace = InventoryManager.instance.allCharacterFace[PlayerPrefs.GetInt(name + "Face", 0)];
            newCharacter.characterWeapon = InventoryManager.instance.allCharacterWeapon[PlayerPrefs.GetInt(name + "Weapon", 0)];
            newCharacter.characterTop = InventoryManager.instance.allCharacterTop[PlayerPrefs.GetInt(name + "Top", 0)];
            newCharacter.characterBack = InventoryManager.instance.allCharacterBack[PlayerPrefs.GetInt(name + "Back", 0)];

            if (newCharacter.characterBody.partId != -1)
                InventoryManager.instance.AddToInventory(newCharacter);

            count++;
        }

        //characters selected
        var selectedIndex = 0;

        while (selectedIndex < 4)
        {
            var name = "SelectedCharSlot" + selectedIndex;

            GameManager.instance.selectedCharacters[selectedIndex].characterBody = InventoryManager.instance.allCharacterBody[PlayerPrefs.GetInt(name + "Body", 0)];
            GameManager.instance.selectedCharacters[selectedIndex].characterFace = InventoryManager.instance.allCharacterFace[PlayerPrefs.GetInt(name + "Face", 0)];
            GameManager.instance.selectedCharacters[selectedIndex].characterWeapon = InventoryManager.instance.allCharacterWeapon[PlayerPrefs.GetInt(name + "Weapon", 0)];
            GameManager.instance.selectedCharacters[selectedIndex].characterTop = InventoryManager.instance.allCharacterTop[PlayerPrefs.GetInt(name + "Top", 0)];
            GameManager.instance.selectedCharacters[selectedIndex].characterBack = InventoryManager.instance.allCharacterBack[PlayerPrefs.GetInt(name + "Back", 0)];

            MenuSceneManager.instance.charactersTeam[selectedIndex].SetIdleCharData(
                GameManager.instance.selectedCharacters[selectedIndex].characterBody,
                GameManager.instance.selectedCharacters[selectedIndex].characterFace,
                GameManager.instance.selectedCharacters[selectedIndex].characterWeapon,
                GameManager.instance.selectedCharacters[selectedIndex].characterTop,
                GameManager.instance.selectedCharacters[selectedIndex].characterBack);

            MenuSceneManager.instance.SetNewCharacter();

            selectedIndex++;
        }

        print("Loaded at " + System.DateTime.Now);
    }

    [ContextMenu("Reset Save File")]

    public void ResetSaveFile(bool isQuit)
    {
        if(InventoryManager.instance)
        {
            InventoryManager.instance.SetID();
            InventoryManager.instance.ResetInventory();
        }
        PlayerPrefs.DeleteAll();

        print("Save File Reset");

        if(isQuit)
        {
            Application.Quit();
            dontSaveLast = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

        }
    }

    //application
    private void OnApplicationQuit()
    {
        if(!dontSaveLast)
        {
            if (!keepResetting)
                SaveGameFile();
            else
                ResetSaveFile(false);
        }
    }
}
