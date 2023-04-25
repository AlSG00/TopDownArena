using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ItemContainer : MonoBehaviour, SCRIPT_IInteractable
{
    public class ContainerItem
    {
        public SCRIPT_InventoryItem Item;
        public int StackCount;
        public Vector2Int position;

        public ContainerItem(SCRIPT_InventoryItem item/*, int stackCount*/)
        {
            Item = item;
            StackCount = item.stackCount;
            position = item.positionOnGrid;
        }
    }

    public bool alreadyInteracting { get; set; }
    public bool canInteract { get; set; }
    public bool inInteractionArea { get; set; }

    [SerializeField] private int containerGridWidth = 5; // размеры сетки контейрена
    [SerializeField] private int containerGridHeight = 5;

    public SCRIPT_ItemGrid containerGrid;
    [SerializeField] private bool isInitialized = false;

    public List<SCRIPT_InventoryItem> itemsToGenerate = new List<SCRIPT_InventoryItem>();
    public List<SCRIPT_InventoryItem> storedItemList = new List<SCRIPT_InventoryItem>();
    public List<ContainerItem> storedContainerItemList = new List<ContainerItem>();

    public delegate void ContainerOpenAction(bool isInitialized, List<SCRIPT_InventoryItem> storedItemCollection, SCRIPT_ItemGrid containerGrid);
    public static event ContainerOpenAction OnContainerOpen;

    private bool isOpened = false;

    private void Awake()
    {
        containerGrid.SetContainerGridVisibility(false);
        alreadyInteracting = false;
        canInteract = false;
        inInteractionArea = false;
    }

    private void OnEnable()
    {
        InventoryController.OnInventoryOpened += CloseContainer;
    }

    private void OnDisable()
    {
        InventoryController.OnInventoryOpened += CloseContainer;
    }

    public void Interact()
    {
        if (containerGrid == null)
        {
            Debug.Log("Container grid is null");
            return;
        }
        
        if (canInteract == false)
        {
            Debug.Log($"Can't interact: {alreadyInteracting} : {canInteract} : {inInteractionArea}");
            return;
        }

        isOpened = true;
        alreadyInteracting = true;
        canInteract = true;

        InitializeGrid();
        containerGrid.SetContainerGridVisibility(true);
        
        alreadyInteracting = false;
    }

    private void InitializeGrid()
    {
        //if (containerGrid.transform.childCount != 0)
        //{
        //    if (isInitialized)
        //    {
        //        containerGrid.ClearGrid();
        //    }
        //}

        containerGrid._gridSizeWidth = containerGridWidth;
        containerGrid._gridSizeHeight = containerGridHeight;
        containerGrid.Initialize(containerGridWidth, containerGridHeight);
        PlaceItems(isInitialized);

        isInitialized = true;
        alreadyInteracting = false;
    }

    private void PlaceItems(bool isInitialized)
    {
        if (isInitialized)
        {
            OnContainerOpen?.Invoke(isInitialized, storedItemList, containerGrid);
        }
        else
        {
            OnContainerOpen?.Invoke(isInitialized, itemsToGenerate, containerGrid);
        }
    }

    private void CloseContainer(bool isInventoryOpened)
    {
        if (isInventoryOpened ||
            isOpened == false)
        {
            return;
        }

        storedItemList = new List<SCRIPT_InventoryItem>(containerGrid.testList);
        containerGrid.ClearGrid();
        isOpened = false;
    }
}



