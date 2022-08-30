using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;

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

    void AddToInventory()
    {

    }

    void RemoveFromInventory()
    {

    }
}
