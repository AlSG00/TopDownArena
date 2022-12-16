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
        canInteract = true;
    }

    //private bool isInitialized;
    private void GridInit()
    {
        if (inventoryController == null)
        {
            inventoryController = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        }

        containerGrid._gridSizeWidth = containerGridWidth;
        containerGrid._gridSizeHeight = containerGridHeight;
        containerGrid.Init(containerGridWidth, containerGridHeight);
        PlaceItems(isInitialized);

        isInitialized = true;
    }

    private void PlaceItems(bool initialized)
    {
        если контейнер открывается впервые, то просто раскидываем предметы (придумать, как генерить рандомные предметы)
        if (initialized == false)
        {
            inventoryController.selectedItemGrid = containerGrid;
            for (int i = 0; i < loot.Length; i++)
            {
                inventoryController.InsertItemIntoInventory(loot[i]);
            }
        }
        else
        {
            если контейнер уже был инициализирован, то надо где-то запомнить позиции предметов и расставлять предметы по своим позициям
        }
    }

    private void PlaceItemsInitialized()
    {

    }
}
