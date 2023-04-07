using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ItemContainer : MonoBehaviour, SCRIPT_IInteractable
{
    public bool alreadyInteracting { get; set; }
    public bool canInteract { get; set; }
    public bool inInteractionArea { get; set; }

    public GameObject[] loot;
    public int containerGridWidth = 5;
    public int containerGridHeight = 5;
    public SCRIPT_InventoryController inventoryController;
  //  public SCRIPT_InteractableObjectTrigger interactionTrigger;

    public SCRIPT_ItemGrid containerGrid;
    bool isInitialized = false;

    public List<StoredItem> storedItemList = new List<StoredItem>();

    // TODO: подкласс item вынести отдельно, потому что пришлось дублировать его здесь и в InventoryController
    public class StoredItem
    {
        public GameObject item;
        public Vector2Int positionOnGrid;
        public bool isRotated;
        public float weight;
        public bool isRotatable;
        public int Width;
        public int Height;

        public StoredItem(GameObject _item, int _positionOnGridX, int _positionOnGridY, bool _isRotated, float _weight, int width, int height)
        {
            item = _item;
            positionOnGrid.x = _positionOnGridX;
            positionOnGrid.y = _positionOnGridY;
            isRotated = _isRotated;
            weight = _weight;
            Width = width;
            Height = height;
        }
    }

    private void Awake()
    {
        HandleContainerGrid(false);
        inventoryController = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();

        alreadyInteracting = false;
        canInteract = false;
        inInteractionArea = false;
    }

    public void Interact()
    {
        alreadyInteracting = true;
        if (canInteract == false)// ||
        //    interactionTrigger.inInteractionArea == false)
        {
            alreadyInteracting = false;
            Debug.Log($"Can't interact: {alreadyInteracting} : {canInteract} : {inInteractionArea}");
            return;
        }

        HandleContainerGrid(true);
        inventoryController.HandleInventoryGrid(true);
        GridInit();
        canInteract = true;
    }

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

    public void HandleContainerGrid(bool isActive)
    {
        Vector2 position = new Vector2();
        RectTransform inventoryRect = containerGrid.GetComponent<RectTransform>();

        if (isActive)
        {
            position.y = 630;
        }
        else
        {
            position.y = 3000;
            alreadyInteracting = false;
        }
        position.x = inventoryRect.position.x;

        inventoryRect.position = position;
    }

    private void PlaceItems(bool initialized)
    {
        inventoryController.selectedItemGrid = containerGrid;
        if (inventoryController.itemContainer != null)
        {
            inventoryController.itemContainer.alreadyInteracting = false;
        }
        inventoryController.itemContainer = this;
        if (initialized == false)
        {
            for (int i = 0; i < loot.Length; i++)
            {
                inventoryController.InsertItemIntoContainer(loot[i]);
            }
        }
        else
        {
            for (int i = 0; i < storedItemList.Count; i++)
            {
                inventoryController.InsertItemIntoInitializedContainer(storedItemList[i]);
            } 
        }
    }
}



