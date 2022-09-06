using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] int selectedRow;
    [SerializeField] GameObject selectionMenu;

    [SerializeField] Transform listObject;
    [SerializeField] GameObject selectionPrefab;

    public static TeamManager instance;
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

    void UpdateInventoryList()
    {
        foreach(Character character in InventoryManager.instance.playerInventory.characterInInventory)
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
        foreach(Transform child in listObject.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        selectionMenu.SetActive(false);

        SaveManager.instance.SaveGameFile();
    }

    public void SelectRow(int row)
    {
        selectedRow = row;
        selectionMenu.SetActive(true);

        UpdateInventoryList();
    }

    public void SelectThisCharacter(CharacterSelection character)
    {

        print("changed from body " + MenuSceneManager.instance.charactersTeam[selectedRow].characterBody + "to body " + character.characterBody);

        MenuSceneManager.instance.charactersTeam[selectedRow].SetIdleCharData(character.characterBody, character.characterFace, character.characterWeapon, character.characterTop, character.characterBack);

        MenuSceneManager.instance.SetNewCharacter();

        CloseSelectionMenu();

    }
}
