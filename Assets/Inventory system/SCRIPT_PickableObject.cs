using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PickableObject : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    //public SCRIPT_ItemData itemData;
    public GameObject inventoryPrefab;
    private SCRIPT_InventoryController inventory;

    private void Start()
    {
        alreadyInteracting = false;
        canInteract = false;
        inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
    }

    public void Interact()
    {
        Debug.Log("Interacting");
        canInteract = false;
        inventory.selectedItemGrid = inventory.inventoryGrid;
        if (inventory.InsertItemIntoInventory(gameObject))
        {
            Destroy(gameObject);
        }
        else
        {
            canInteract = true;
        }
    }
}
