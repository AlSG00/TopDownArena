using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ItemContainer : MonoBehaviour, SCRIPT_IInteractable
{
    public bool alreadyInteracting { get; set; }
    public bool canInteract { get; set; }
    public bool inInteractionArea { get; set; }

    //public GameObject[] loot; // Список лута, который будет расставлен по контейнеру при первом открытии
    [SerializeField] private int containerGridWidth = 5; // размеры сетки контейрена
    [SerializeField] private int containerGridHeight = 5;
   // public InventoryController inventoryController;
  //  public SCRIPT_InteractableObjectTrigger interactionTrigger;

    public SCRIPT_ItemGrid containerGrid;
    bool isInitialized = false;

    //public List<StoredItem> storedItemList = new List<StoredItem>();
    public List<SCRIPT_InventoryItem> itemsToGenerate = new List<SCRIPT_InventoryItem>();
    public List<SCRIPT_InventoryItem> storedItemList = new List<SCRIPT_InventoryItem>();

    public delegate void ContainerOpenAction(bool isInitialized, List<SCRIPT_InventoryItem> storedItemCollection, SCRIPT_ItemGrid containerGrid);
    public static event ContainerOpenAction OnContainerOpen;
    // TODO: подкласс item вынести отдельно, потому что пришлось дублировать его здесь и в InventoryController
    //public class StoredItem
    //{
    //    public GameObject item;
    //    public Vector2Int positionOnGrid;
    //    public bool isRotated;
    //    public float weight;
    //    public bool isRotatable;
    //    public int Width;
    //    public int Height;

    //    public StoredItem(GameObject _item, int _positionOnGridX, int _positionOnGridY, bool _isRotated, float _weight, int width, int height)
    //    {
    //        item = _item;
    //        positionOnGrid.x = _positionOnGridX;
    //        positionOnGrid.y = _positionOnGridY;
    //        isRotated = _isRotated;
    //        weight = _weight;
    //        Width = width;
    //        Height = height;
    //    }
    //}


    /////////////////////////////////////////////////////////////////////////////////////////////////


    private void Awake()
    {
        SetContainerGridVisibility(false);
        //inventoryController = GameObject.Find("_PlayerCamera").GetComponent<InventoryController>();

        alreadyInteracting = false;
        canInteract = false;
        inInteractionArea = false;
    }

    public void Interact()
    {
        if (containerGrid == null)
        {
            Debug.Log("Container grid is null");
            return;
        }

        alreadyInteracting = true;
        if (canInteract == false)// ||
        //    interactionTrigger.inInteractionArea == false)
        {
            alreadyInteracting = false;
            Debug.Log($"Can't interact: {alreadyInteracting} : {canInteract} : {inInteractionArea}");
            return;
        }

        SetContainerGridVisibility(true);
        //inventoryController.isCheckingInventory = !inventoryController.isCheckingInventory;
        //inventoryController.SetInventoryVisibility(true);
        //inventoryController.ShowContainerGrid(true);
        InitializeGrid();

        canInteract = true;
        alreadyInteracting = false;
    }

    private void InitializeGrid()
    {
        //if (inventoryController == null)
        //{
        //    inventoryController = GameObject.Find("_PlayerCamera").GetComponent<InventoryController>();
        //}

        if (containerGrid.transform.childCount != 0)
        {
            containerGrid.ClearGrid();
        }

        containerGrid._gridSizeWidth = containerGridWidth;
        containerGrid._gridSizeHeight = containerGridHeight;
        containerGrid.Initialize(containerGridWidth, containerGridHeight);

        //НОВОЕ
        

        containerGrid.testList = storedItemList;
        
        //containerGrid.testList = new List<SCRIPT_InventoryItem>(storedItemList);
        //containerGrid.testList = 

        PlaceItems(isInitialized);

        isInitialized = true;
        alreadyInteracting = false;
    }

    public void SetContainerGridVisibility(bool isActive)
    {

        //inventoryController.ShowContainerGrid(isActive);

        containerGrid.SetContainerGridVisibility(isActive);
        //Vector2 position = new Vector2();
        //RectTransform inventoryRect = containerGrid.GetComponent<RectTransform>();

        //if (isActive)
        //{
        //    position.y = 630;
        //}
        //else
        //{
        //    position.y = 3000;
        //    alreadyInteracting = false;
        //}
        //position.x = inventoryRect.position.x;

        //inventoryRect.position = position;

    }

    private void PlaceItems(bool isInitialized)
    {
        // Нужно ли оно тут. Посмотреть, как зануляется флаг у других интерактивных объектов, я забыл


        //СОВСЕМ ЗАБЫЛ
        //Нужно хранить данные не на ItemGrid, а в самом контейнере, потом что ItemGrid Постоянно меняется, между контейнерами
        //    И получается нету никакой постоянной привязки к данным

        OnContainerOpen?.Invoke(isInitialized, itemsToGenerate, containerGrid);


        //inventoryController.selectedItemGrid = containerGrid;
        //if (inventoryController.itemContainer != null)
        //{
        //    inventoryController.itemContainer.alreadyInteracting = false;
        //}
        //inventoryController.itemContainer = this;

        //if (initialized == false)
        //{
        //    for (int i = 0; i < loot.Length; i++)
        //    {
        //        inventoryController.InsertItemIntoContainer(loot[i]);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < storedItemList.Count; i++)
        //    {
        //        inventoryController.InsertItemIntoInitializedContainer(storedItemList[i]);
        //    } 
        //}

        //if (initialized == false)
        //{
        //    for (int i = 0; i < storedItemList.Count; i++)
        //    {
        //        inventoryController.InsertItemIntoContainer(storedItemList[i]);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < storedItemList.Count; i++)
        //    {
        //        inventoryController.InsertItemIntoInitializedContainer(storedItemList[i]);
        //    }
        //}
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////


    //private void Awake()
    //{
    //    SetContainerGridVisibility(false);
    //    inventoryController = GameObject.Find("_PlayerCamera").GetComponent<InventoryController>();

    //    alreadyInteracting = false;
    //    canInteract = false;
    //    inInteractionArea = false;
    //}

    //public void Interact()
    //{
    //    alreadyInteracting = true;
    //    if (canInteract == false)// ||
    //    //    interactionTrigger.inInteractionArea == false)
    //    {
    //        alreadyInteracting = false;
    //        Debug.Log($"Can't interact: {alreadyInteracting} : {canInteract} : {inInteractionArea}");
    //        return;
    //    }

    //    //SetContainerGridVisibility(true);
    //    //inventoryController.isCheckingInventory = !inventoryController.isCheckingInventory;
    //    //inventoryController.SetInventoryVisibility(true);
    //    //inventoryController.ShowContainerGrid(true);
    //    InitializeGrid();
    //    canInteract = true;
    //}

    //private void InitializeGrid()
    //{
    //    if (inventoryController == null)
    //    {
    //        inventoryController = GameObject.Find("_PlayerCamera").GetComponent<InventoryController>();
    //    }

    //    if (containerGrid.transform.childCount != 0)
    //    {
    //        containerGrid.ClearGrid();
    //    }

    //    containerGrid._gridSizeWidth = containerGridWidth;
    //    containerGrid._gridSizeHeight = containerGridHeight;
    //    containerGrid.Initialize(containerGridWidth, containerGridHeight);

    //    //НОВОЕ
    //    containerGrid.testList = storedItemList;


    //    PlaceItems(isInitialized);

    //    isInitialized = true;
    //    alreadyInteracting = false;
    //}

    //public void SetContainerGridVisibility(bool isActive)
    //{

    //    //inventoryController.ShowContainerGrid(isActive);

    //    containerGrid.SetVisibility(isActive);
    //    //Vector2 position = new Vector2();
    //    //RectTransform inventoryRect = containerGrid.GetComponent<RectTransform>();

    //    //if (isActive)
    //    //{
    //    //    position.y = 630;
    //    //}
    //    //else
    //    //{
    //    //    position.y = 3000;
    //    //    alreadyInteracting = false;
    //    //}
    //    //position.x = inventoryRect.position.x;

    //    //inventoryRect.position = position;

    //    //InventoryController.OnInventoryOpened?.Invoke(true, true);
    //}

    //private void PlaceItems(bool initialized)
    //{
    //   // Нужно ли оно тут. Посмотреть, как зануляется флаг у других интерактивных объектов, я забыл


    //    //СОВСЕМ ЗАБЫЛ
    //    //Нужно хранить данные не на ItemGrid, а в самом контейнере, потом что ItemGrid Постоянно меняется, между контейнерами
    //    //    И получается нету никакой постоянной привязки к данным


    //    inventoryController.selectedItemGrid = containerGrid;
    //    if (inventoryController.itemContainer != null)
    //    {
    //        inventoryController.itemContainer.alreadyInteracting = false;
    //    }
    //    inventoryController.itemContainer = this;

    //    //if (initialized == false)
    //    //{
    //    //    for (int i = 0; i < loot.Length; i++)
    //    //    {
    //    //        inventoryController.InsertItemIntoContainer(loot[i]);
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    for (int i = 0; i < storedItemList.Count; i++)
    //    //    {
    //    //        inventoryController.InsertItemIntoInitializedContainer(storedItemList[i]);
    //    //    } 
    //    //}

    //    if (initialized == false)
    //    {
    //        for (int i = 0; i < storedItemList.Count; i++)
    //        {
    //            inventoryController.InsertItemIntoContainer(storedItemList[i]);
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < storedItemList.Count; i++)
    //        {
    //            inventoryController.InsertItemIntoInitializedContainer(storedItemList[i]);
    //        }
    //    }
    //}
}



