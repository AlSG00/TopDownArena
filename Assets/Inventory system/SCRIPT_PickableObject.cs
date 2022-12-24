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
        canInteract = false;
        //Debug.Log("Searching space 1");
        Vector2Int? positionOnGrid = inventory.selectedItemGrid.FindSpaceForObject(inventoryPrefab.GetComponent<SCRIPT_InventoryItem>());
        if (positionOnGrid == null)
        {
            //Debug.Log("Searching space 2");
            //SCRIPT_InventoryItem itemToPick = inventoryPrefab.GetComponent<SCRIPT_InventoryItem>();/*inventoryPrefab.GetComponent<SCRIPT_InventoryItem>().Rotated();*/
            //itemToPick.Rotated();
            //positionOnGrid = inventory.selectedItemGrid.FindSpaceForObject(inventoryPrefab.GetComponent<SCRIPT_InventoryItem>());
            //inventoryPrefab.GetComponent<SCRIPT_InventoryItem>().Rotated();
            //if (positionOnGrid == null)
            //{
            //    Debug.Log("No free space");
            //    alreadyInteracting = false;
            //    return;
            //}
            //Debug.Log("Fiasko");
            alreadyInteracting = false;
            return;
        }

        inventory.selectedItemGrid = inventory.inventoryGrid;
        inventory.InsertItemIntoInventory(gameObject);
        Destroy(gameObject);
        // canInteract = true;
    }
}
