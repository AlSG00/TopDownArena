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
    bool isInitialized = false;

    public List<StoredItem> storedItemList = new List<StoredItem>();

    public class StoredItem
    {
        public SCRIPT_InventoryItem item;
        public Vector2Int positionOnGrid;
    }

    private void Awake()
    {
        inventoryController = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
       // containerGrid = GameObject.Find("ContainerGrid").GetComponent<SCRIPT_ItemGrid>();
    }

    public void Interact()
    {
        alreadyInteracting = true;
        if (canInteract == false)
        {
            Debug.Log("Can't interact");
            return;
        }

        GridInit();
        canInteract = true;
    }

    //private bool isInitialized;
    private void GridInit()
    {
        if (inventoryController == null)
        {
            inventoryController = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        }

        if (containerGrid.transform.childCount != 0)
        {
            containerGrid.ClearGrid();
        }

        containerGrid._gridSizeWidth = containerGridWidth;
        containerGrid._gridSizeHeight = containerGridHeight;
        containerGrid.Init(containerGridWidth, containerGridHeight);
        PlaceItems(isInitialized);

        isInitialized = true;
        alreadyInteracting = false;
    }

    private void PlaceItems(bool initialized)
    {
        if (initialized == false)
        {
            inventoryController.selectedItemGrid = containerGrid;
            inventoryController.itemContainer = this;
            for (int i = 0; i < loot.Length; i++)
            {
                inventoryController.InsertItemIntoContainer(loot[i]);
            }
        }
        else
        {
            inventoryController.selectedItemGrid = containerGrid;
            for (int i = 0; i < loot.Length; i++)
            {
                inventoryController.InsertItemIntoInitializedContainer(loot[i]);
            }
        }
    }
}



