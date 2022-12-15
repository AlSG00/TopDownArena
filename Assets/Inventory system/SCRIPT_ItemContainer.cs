using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ItemContainer : MonoBehaviour, SCRIPT_IInteractable
{
    public bool alreadyInteracting { get; set; }
    public bool canInteract { get; set; }

    public GameObject[] loot;
    public int containerGridWidth = 5;
    public int containerGridHeight = 5;
    public SCRIPT_InventoryController inventoryController;

    public SCRIPT_ItemGrid containerGrid;

    private void Awake()
    {
        inventoryController = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
    }

    public void Interact()
    {
        if (canInteract == false)
        {
            return;
        }

        GridInit();
        PlaceItems();
        canInteract = true;
    }

    //private bool isInitialized;
    private void GridInit()
    {
        if (inventoryController == null)
        {
            inventoryController = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        }
        //if (!isInitialized)
        //{
            containerGrid._gridSizeWidth = containerGridWidth;
            containerGrid._gridSizeHeight = containerGridHeight;
            containerGrid.Init(containerGridWidth, containerGridHeight);
        //    isInitialized = true;
        //}
    }

    private void PlaceItems()
    {
        //while (!isInitialized)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        inventoryController.selectedItemGrid = containerGrid;
        for (int i = 0; i < loot.Length; i++)
        {
            inventoryController.InsertItemIntoInventory(loot[i]);
        }

    }
}
