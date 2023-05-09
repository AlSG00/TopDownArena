using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("References")]
    public SCRIPT_ItemGrid inventoryGrid;
    public SCRIPT_ItemGrid containerItemGrid;
    public SCRIPT_ItemGrid selectedItemGrid;
    public SCRIPT_ItemContainer itemContainer;
    [HideInInspector] public RectTransform selectedItemGridRect;
    [SerializeField] private Player_Movement _playerMovement;
    [SerializeField] private SCRIPT_PlayerCarryingWeight _playerCarryingWeight;
    [SerializeField] private SCRIPT_InventoryHighlight inventoryHighlight;
    [SerializeField] private ItemInfoWindowHandler itemInfoWindow;

    [Header("Item")]
    public SCRIPT_InventoryItem selectedItem;
    public List<SCRIPT_InventoryItem> inventoryItemList = new List<SCRIPT_InventoryItem>();
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Transform itemDropPoint;
    private SCRIPT_InventoryItem overlapItem;
    private RectTransform itemRectTransform;

    [Header("Button holding properties")]
    [SerializeField] private float timeToHold = 0.3f;
    private float buttonHoldTime = 0f;
    private bool isHoldingCheckStateButton = false;
    private bool isHoldingShiftButton = false;
    private bool isHoldingDropItemButton = false;

    [Header("States")]
    public bool isCheckingInventory = false;
    public bool isHighlightingStateIcons = false;
    public bool isDroppingStack = false;
    public delegate void OpenAction(bool isOpened);
    public static event OpenAction OnInventoryOpened;
    public static event OpenAction OnStateIconShow;  

    private void OnEnable()
    {
        SCRIPT_ItemContainer.OnContainerOpen += FillContainerGrid;
    }

    private void OnDisable()
    {
        SCRIPT_ItemContainer.OnContainerOpen -= FillContainerGrid;
    }

    private void Start()
    {
        inventoryGrid.testList = inventoryItemList;
        isCheckingInventory = false;
        SetInventoryVisibility(isCheckingInventory);
    }

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isHoldingShiftButton = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isHoldingShiftButton = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleItemHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            if (isHoldingShiftButton)
            {
                MoveItemFast();
            }
            else
            {
                LeftMouseButtonPress();
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            RightMouseButtonPress();
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (itemInfoWindow.isShowingDetails)
            {
                itemInfoWindow.ShowDetails(false);
            }
            else
            {
                itemInfoWindow.ShowDetails(true);
            }

            
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isHoldingDropItemButton = true;
            buttonHoldTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            isHoldingDropItemButton = false;
            if (isDroppingStack)
            {
                isDroppingStack = false;
            }
            else
            {
                DropItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isHoldingCheckStateButton = true;
            buttonHoldTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            HandleStateIconsVisibility();
        }

        // TODO: Как нибудь переделать эту страшную штуку
        if (isHoldingCheckStateButton &&
            isCheckingInventory == false)
        {
            buttonHoldTime += Time.deltaTime;
            if (buttonHoldTime >= timeToHold
                && isHighlightingStateIcons == false)
            {
                buttonHoldTime = 0;
                isHighlightingStateIcons = true;
                OnStateIconShow?.Invoke(true);
            }
        }

        if (isHoldingDropItemButton &&
            isCheckingInventory)
        {
            buttonHoldTime += Time.deltaTime;
            if (buttonHoldTime >= timeToHold)
            {
                buttonHoldTime = 0;
                isDroppingStack = true;
                DropStack();
            }
        }
    }

    public int InsertIntoAvailableStacks(SCRIPT_InventoryItem itemToStack, int stackCount, bool addToInventory)
    {
        if (addToInventory)
        {
            selectedItemGrid = inventoryGrid;
        }

        int leftToStack = stackCount;

        for (int i = 0; i < selectedItemGrid._gridSizeHeight; i++)
        {
            for (int j = 0; j < selectedItemGrid._gridSizeWidth; j++)
            {
                if (selectedItemGrid.inventoryItemSlot[j, i] != null &&
                    selectedItemGrid.inventoryItemSlot[j, i].isStackable &&
                    selectedItemGrid.inventoryItemSlot[j, i].name == itemToStack.name &&
                    selectedItemGrid.inventoryItemSlot[j, i].stackCount < selectedItemGrid.inventoryItemSlot[j, i].maxStackCount)
                {
                    int temp = leftToStack + selectedItemGrid.inventoryItemSlot[j, i].stackCount;

                    if (temp < selectedItemGrid.inventoryItemSlot[j, i].maxStackCount)
                    {
                        if (addToInventory && selectedItemGrid != itemToStack.lastGrid)
                        {
                            _playerCarryingWeight.AddWeight(leftToStack * selectedItemGrid.inventoryItemSlot[j, i].weight);
                        }
                        selectedItemGrid.inventoryItemSlot[j, i].stackCount += leftToStack;
                        selectedItemGrid.inventoryItemSlot[j, i].UpdateCounter();
                        return 0;
                    }
                    else
                    {
                        int valueToFillStack = selectedItemGrid.inventoryItemSlot[j, i].maxStackCount - selectedItemGrid.inventoryItemSlot[j, i].stackCount;
                        leftToStack -= valueToFillStack;
                        selectedItemGrid.inventoryItemSlot[j, i].stackCount += valueToFillStack;
                        if (addToInventory && selectedItemGrid != itemToStack.lastGrid)
                        {
                            _playerCarryingWeight.AddWeight(valueToFillStack * selectedItemGrid.inventoryItemSlot[j, i].weight);
                        }
                    }

                    selectedItemGrid.inventoryItemSlot[j, i].UpdateCounter();
                }
            }
        }

        return leftToStack;
    }

    private void HandleStateIconsVisibility()
    {
        isHoldingCheckStateButton = false;
        isHighlightingStateIcons = false;

        if (buttonHoldTime < timeToHold)
        {
            isCheckingInventory = !isCheckingInventory;
            GetItemBack();
            SetInventoryVisibility(isCheckingInventory);
        }
        else
        {
            if (isCheckingInventory == false)
            {
                OnStateIconShow?.Invoke(false); // NEW;
            }
        }

        buttonHoldTime = 0f;
    }

    private void DropItem()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            selectedItem = selectedItemGrid.inventoryItemSlot[tileGridPosition.x, tileGridPosition.y];
        }

        if (selectedItem == null)
        {
            return;
        }

        if (selectedItem.isDropping)
        {
            return;
        }
        selectedItem.isDropping = true;

        if (selectedItem.isOnCursor)
        {
            if (selectedItem.isSingleDropping)
            {
                for (int i = 0; i < selectedItem.stackCount; i++)
                {
                    Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
                }
                selectedItemGrid.testList.Remove(selectedItem);
                UpdateCarryingWeight(selectedItem, true);
                Destroy(selectedItem.gameObject);
                inventoryHighlight.Show(false);
            }
            else
            {
                GameObject droppedItem = Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
                PickableObject droppedItemData = droppedItem.GetComponent<PickableObject>();
                droppedItemData.stackCount = selectedItem.stackCount;
                selectedItemGrid.testList.Remove(selectedItem);
                UpdateCarryingWeight(selectedItem, true);
                Destroy(selectedItem.gameObject);
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            if (selectedItem.isSingleDropping)
            {
                if (selectedItem.stackCount == 1)
                {
                    selectedItemGrid.testList.Remove(selectedItem);
                    UpdateCarryingWeight(selectedItem, false);
                    itemInfoWindow.SetVisibility(false, selectedItem);
                    Destroy(selectedItem.gameObject);
                    inventoryHighlight.Show(false);
                }
                else
                {
                    selectedItem.isDropping = false;
                    selectedItem.stackCount--;
                    selectedItem.UpdateCounter();
                    UpdateCarryingWeight(selectedItem, false);
                }
                Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
            }
            else
            {
                GameObject droppedItem = Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
                PickableObject droppedItemData = droppedItem.GetComponent<PickableObject>();
                droppedItemData.stackCount = selectedItem.stackCount;
                selectedItemGrid.testList.Remove(selectedItem);
                UpdateCarryingWeight(selectedItem, true);
                itemInfoWindow.SetVisibility(false, selectedItem);
                Destroy(selectedItem.gameObject);
                inventoryHighlight.Show(false);
            }
        }

        selectedItem = null;
    }

    private void DropStack()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            selectedItem = selectedItemGrid.inventoryItemSlot[tileGridPosition.x, tileGridPosition.y];
        }

        if (selectedItem == null)
        {
            return;
        }

        if (selectedItem.isDropping)
        {
            return;
        }
        itemInfoWindow.SetVisibility(false, selectedItem);
        selectedItem.isDropping = true;
        selectedItemGrid.PickUpItem(selectedItem.positionOnGrid.x, selectedItem.positionOnGrid.y);
        if (selectedItem.isSingleDropping)
        {
            for (int i = 0; i < selectedItem.stackCount; i++)
            {
                Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
            }
            selectedItemGrid.testList.Remove(selectedItem);
            UpdateCarryingWeight(selectedItem, true);
            Destroy(selectedItem.gameObject);
            inventoryHighlight.Show(false);
        }
        else
        {
            GameObject droppedItem = Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
            PickableObject droppedItemData = droppedItem.GetComponent<PickableObject>();
            droppedItemData.stackCount = selectedItem.stackCount;
            selectedItemGrid.testList.Remove(selectedItem);
            UpdateCarryingWeight(selectedItem, true);
            Destroy(selectedItem.gameObject);
            inventoryHighlight.Show(false);
        }
    }

    private void UpdateCarryingWeight(SCRIPT_InventoryItem item, bool dropFullStack)
    {
        if (item.lastGrid.isPlayerInventory)
        {
            if (item.isSingleDropping)
            {
                if (dropFullStack)
                {
                    _playerCarryingWeight.TakeWeight(item.weight * item.stackCount);
                }
                else
                {
                    _playerCarryingWeight.TakeWeight(item.weight);
                }
            }
            else
            {
                _playerCarryingWeight.TakeWeight(item.weight * item.stackCount);
            }
        }
    }

    private void RightMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            selectedItem = selectedItemGrid.inventoryItemSlot[tileGridPosition.x, tileGridPosition.y];
            if (selectedItem == null)
            {
                return;
            }
        }

        if (selectedItem.isOnCursor)
        {
            SCRIPT_InventoryItem secondItem = selectedItemGrid.inventoryItemSlot[tileGridPosition.x, tileGridPosition.y];
            if (secondItem != null)
            {
                if (secondItem.name == selectedItem.name &&
                    selectedItem.isStackable &&
                    selectedItem.isSingleDropping)
                {
                    if (secondItem.stackCount < secondItem.maxStackCount)
                    {
                        secondItem.stackCount++;
                        selectedItem.stackCount--;
                        secondItem.UpdateCounter();
                        selectedItem.UpdateCounter();
                        
                        if (selectedItemGrid != selectedItem.lastGrid)
                        {
                            if (selectedItemGrid.isPlayerInventory)
                            {
                                _playerCarryingWeight.AddWeight(selectedItem.weight);
                            }
                            else
                            {
                                _playerCarryingWeight.TakeWeight(selectedItem.weight);
                            }
                        }

                        if (selectedItem.stackCount == 0)
                        {
                            Destroy(selectedItem.gameObject);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (selectedItem.stackCount > 1 &&
                    selectedItem.isSingleDropping)
                {
                    SCRIPT_InventoryItem itemOnCursor = selectedItem;
                    CreateItemForUi(itemOnCursor);
                    selectedItem.stackCount = 1;
                    selectedItem.UpdateCounter();
                    PlaceItemOnGrid(tileGridPosition);

                    selectedItem = itemOnCursor;
                    itemRectTransform = itemOnCursor.GetComponent<RectTransform>();
                    itemRectTransform.SetParent(canvasTransform);
                    itemRectTransform.SetAsLastSibling();

                    selectedItem.stackCount--;
                    selectedItem.UpdateCounter();

                    if (selectedItem.stackCount == 0)
                    {
                        Destroy(selectedItem.gameObject);
                    }
                }
                else
                {
                    PlaceItemOnGrid(tileGridPosition);
                }
            }

            return;
        }

        if (selectedItem.isUsable == false) //TODO: Возможно, тут вообще надо убрать метод PlaceItemOnGrid
        {
            PlaceItemOnGrid(tileGridPosition);
            return;
        }

        if (selectedItem.useItemAudioSource != null &&
            selectedItem.useItemAudio != null)
        {
            selectedItem.useItemAudioSource.PlayOneShot(selectedItem.useItemAudio);
        }

        selectedItem.GetComponent<SCRIPT_IItem>().Use();

        if (selectedItemGrid.isPlayerInventory)
        {
            _playerCarryingWeight.TakeWeight(selectedItem.weight);
        }

        if (selectedItem.stackCount > 1)
        {
            selectedItem.stackCount--;
            selectedItem.UpdateCounter();
        }
        else
        {
            selectedItemGrid.testList.Remove(selectedItem);
            itemInfoWindow.SetVisibility(false, selectedItem);
            Destroy(selectedItem.gameObject);
            inventoryHighlight.Show(false);
        }

        selectedItem = null;
    }

    // TODO: Учитывать, что в будущем этот метод используется для взятия всего стака
    Vector2Int previousPosition;
    private void PickItemFromGrid(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            selectedItem.SetOnCursorFlag(true);
            selectedItem.lastGrid = selectedItemGrid;
            selectedItemGrid.testList.Remove(selectedItem);
            itemRectTransform = selectedItem.GetComponent<RectTransform>();
            itemRectTransform.SetAsLastSibling();
        }
    }

    public void InsertItemIntoInventory(SCRIPT_InventoryItem item, int stackCount)
    {
        if (selectedItemGrid == null)
        {
            Debug.Log("Grid is not selected");
            return;
        } 

        CreateItemForUi(item);
        SCRIPT_InventoryItem itemToInsert = selectedItem; // TODO: Может лучше избавиться от глобально переменной Selected Item???
        selectedItem = null;
        itemToInsert.stackCount = stackCount;
        itemToInsert.lastGrid = selectedItemGrid;
        InsertItem(itemToInsert);

        // TODO: Продумать здесь логику на случай, если в инвентаре будет несколько разных сеток
        _playerCarryingWeight.AddWeight(itemToInsert.weight * itemToInsert.stackCount);
    }

    public void CreateItemForUi(SCRIPT_InventoryItem item)
    {
        SCRIPT_InventoryItem inventoryItem = Instantiate(item);
        selectedItem = inventoryItem;
        itemRectTransform = inventoryItem.GetComponent<RectTransform>();
        itemRectTransform.SetParent(canvasTransform);
        itemRectTransform.SetAsLastSibling();
        inventoryItem.Set(inventoryItem.itemData);
    }

    private void FillContainerGrid(bool isInitialized, List<SCRIPT_InventoryItem> storedItemList, SCRIPT_ItemGrid containerGrid)
    {
        containerItemGrid = containerGrid;
        selectedItemGrid = containerGrid;

        if (isInitialized)
        {
            InsertItemIntoInitializedContainer(storedItemList);
        }
        else
        {
            InsertItemIntoContainer(storedItemList);
        }

        isCheckingInventory = true;
        SetInventoryVisibility(isCheckingInventory);
    }

    public void InsertItemIntoContainer(List<SCRIPT_InventoryItem> storedItemList)
    {
        selectedItemGrid.testList = new List<SCRIPT_InventoryItem>();
        foreach (var item in storedItemList)
        {
            CreateItemForUi(item);
            SCRIPT_InventoryItem itemToInsert = selectedItem;
            selectedItem = null;
            Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
            if (positionOnGrid == null)
            {
                return;
            }
            selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);
            itemToInsert.lastGrid = selectedItemGrid;
            itemToInsert.positionOnGrid.x = positionOnGrid.Value.x;
            itemToInsert.positionOnGrid.y = positionOnGrid.Value.y;
            item.positionOnGrid = itemToInsert.positionOnGrid;
            selectedItemGrid.testList.Add(itemToInsert);
        }
    }

    public void InsertItemIntoInitializedContainer(List<SCRIPT_InventoryItem> storedItemList)
    {
        selectedItemGrid.testList = new List<SCRIPT_InventoryItem>();
        foreach (SCRIPT_InventoryItem item in storedItemList)
        {
            //CreateItemForUi(item);
            item.gameObject.SetActive(true);
            if (item.isRotated)
            {
                RotateItem();
            }
            SCRIPT_InventoryItem itemToInsert = item;
            selectedItem = null;
            selectedItemGrid.PlaceItem(itemToInsert, item.positionOnGrid.x, item.positionOnGrid.y);
            selectedItemGrid.testList.Add(itemToInsert);
        }
    }

    private void InsertItem(SCRIPT_InventoryItem itemToInsert)
    {
        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert); 

        if (positionOnGrid == null)
        {
            return;
        }

        if (selectedItemGrid.returnRotated == false)
        {
            selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);
        }
        else
        {
            itemToInsert.Rotated();
            selectedItemGrid.returnRotated = false;
            selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);
        }
        selectedItemGrid.testList.Add(itemToInsert);
    }

    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        if (selectedItem.isRotatable)
        {
            selectedItem.Rotated();
        }
    }

    Vector2Int oldPosition;
    SCRIPT_InventoryItem itemToHighlight;
    private void HandleItemHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid)
        {
            return;
        }

        oldPosition = positionOnGrid;

        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetParent(selectedItemGrid);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.Height,
                selectedItem.Width)
                );
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void LeftMouseButtonPress()
    {
        if (isCheckingInventory == false)
        {
            return;
        }

        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickItemFromGrid(tileGridPosition);
        }
        else
        {
            PlaceItemOnGrid(tileGridPosition);
        }
    }

    private void MoveItemFast()
    {
        SCRIPT_ItemGrid previousGrid = null;
        if (isCheckingInventory == false ||
            containerItemGrid == null)
        {
            return;
        }

        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickItemFromGrid(tileGridPosition);
            if (selectedItem == null)
            {
                return;
            }
        }

        if (selectedItem.isStackable)
        {
            if (selectedItemGrid.isPlayerInventory)
            {
                selectedItemGrid = containerItemGrid;
                int stackCountRemaining = InsertIntoAvailableStacks(selectedItem, selectedItem.stackCount, false);
                if (stackCountRemaining > 0)
                {
                    if (selectedItemGrid != selectedItem.lastGrid)
                    {
                        _playerCarryingWeight.TakeWeight(selectedItem.weight * (selectedItem.stackCount - stackCountRemaining));
                    }
                    selectedItem.stackCount = stackCountRemaining;
                    Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(selectedItem);
                    if (positionOnGrid == null)
                    {
                        selectedItemGrid = selectedItem.lastGrid;
                        PlaceItemOnGrid(selectedItem.positionOnGrid);
                        selectedItem = null;
                        return;
                    }

                    SCRIPT_InventoryItem itemToInsert = selectedItem;
                    if (itemToInsert.lastGrid != selectedItemGrid)
                    {
                        _playerCarryingWeight.TakeWeight(itemToInsert.weight * itemToInsert.stackCount);
                    }
                    selectedItem.SetOnCursorFlag(false);
                    selectedItem = null;

                    InsertItem(itemToInsert);

                    //selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);
                    //itemToInsert.lastGrid = selectedItemGrid;
                    //itemToInsert.positionOnGrid.x = positionOnGrid.Value.x;
                    //itemToInsert.positionOnGrid.y = positionOnGrid.Value.y;
                    //selectedItemGrid.testList.Add(itemToInsert); // TODO: Вспомнить, почему я не поставил этот метод в PlaceItem();
                    selectedItemGrid = inventoryGrid;
                }
                else
                {
                    //selectedItemGrid = inventoryGrid;
                    //if (selectedItem.isOnCursor == false)
                    //{
                    //    PickItemFromGrid(selectedItem.positionOnGrid);
                    //}
                    if (selectedItem.lastGrid != selectedItemGrid)
                    {
                        _playerCarryingWeight.TakeWeight(selectedItem.weight * selectedItem.stackCount);
                    }
                    Destroy(selectedItem.gameObject);
                    selectedItem = null;
                    selectedItemGrid = inventoryGrid;
                }
            }
            else
            {
                selectedItemGrid = inventoryGrid;
                int stackCountRemaining = InsertIntoAvailableStacks(selectedItem, selectedItem.stackCount, true);
                if (stackCountRemaining > 0)
                {
                    //if (selectedItemGrid != selectedItem.lastGrid)
                    //{
                    //    _playerCarryingWeight.AddWeight(selectedItem.weight * (selectedItem.stackCount - stackCountRemaining));
                    //}
                    selectedItem.stackCount = stackCountRemaining;
                    Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(selectedItem);
                    if (positionOnGrid == null)
                    {
                        selectedItemGrid = selectedItem.lastGrid;
                        PlaceItemOnGrid(selectedItem.positionOnGrid);
                        selectedItem = null;
                        return;
                    }

                    SCRIPT_InventoryItem itemToInsert = selectedItem;
                    if (itemToInsert.lastGrid != selectedItemGrid)
                    {
                        _playerCarryingWeight.AddWeight(itemToInsert.weight * itemToInsert.stackCount);
                    }
                    selectedItem.SetOnCursorFlag(false);
                    selectedItem = null;
                    //selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);
                    //itemToInsert.lastGrid = selectedItemGrid;
                    //itemToInsert.positionOnGrid.x = positionOnGrid.Value.x;
                    //itemToInsert.positionOnGrid.y = positionOnGrid.Value.y;
                    //selectedItemGrid.testList.Add(itemToInsert);
                    InsertItem(itemToInsert);

                    selectedItemGrid = containerItemGrid;
                }
                else
                {
                    //if (selectedItemGrid != selectedItem.lastGrid)
                    //{
                    //    _playerCarryingWeight.AddWeight(selectedItem.weight * selectedItem.stackCount);
                    //}
                    Destroy(selectedItem.gameObject);
                    selectedItem = null;
                    selectedItemGrid = containerItemGrid;
                }
            }
        }
        else
        {
            previousGrid = selectedItemGrid;
            if (selectedItemGrid.isPlayerInventory)
            {
                selectedItemGrid = containerItemGrid;
            }
            else
            {
                selectedItemGrid = inventoryGrid;
            }

            Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(selectedItem);
            if (positionOnGrid == null)
            {
                selectedItemGrid = selectedItem.lastGrid;
                PlaceItemOnGrid(selectedItem.positionOnGrid);
                selectedItem = null;
                return;
            }

            SCRIPT_InventoryItem itemToInsert = selectedItem;
            selectedItem.SetOnCursorFlag(false);
            selectedItem = null;

            if (selectedItemGrid != itemToInsert.lastGrid)
            {
                if (selectedItemGrid.isPlayerInventory)
                {
                    _playerCarryingWeight.AddWeight(itemToInsert.weight * itemToInsert.stackCount);
                }
                else
                {
                    _playerCarryingWeight.TakeWeight(itemToInsert.weight * itemToInsert.stackCount);
                }
            }
            InsertItem(itemToInsert);

            selectedItemGrid = previousGrid;
        }
    }

    // Позиция мыши, переведенная из координат экрана в координаты на сетке инвентаря
    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.Width - 1) * SCRIPT_ItemGrid._tileSizeWidth / 2;
            position.y += (selectedItem.Height - 1) * SCRIPT_ItemGrid._tileSizeHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    // TODO: Переделать метод. Он не учитывает другие размеры экрана кроме фулл хд
    public void SetInventoryVisibility(bool isInventoryOpened)
    {
        if (isInventoryOpened == false)
        {
            containerItemGrid = null;
        }
        
        OnInventoryOpened?.Invoke(isInventoryOpened);
        OnStateIconShow?.Invoke(isInventoryOpened);
        _playerMovement.enabled = !isInventoryOpened;
    }

    public void GetItemBack()
    {
        if (selectedItem == null)
        {
            return;
        }

        if (selectedItem.isOnCursor)
        {
            selectedItemGrid = selectedItem.lastGrid;
            PlaceItemOnGrid(selectedItem.positionOnGrid);
        }
    }

    //Протестить функцию
    private void PlaceItemOnGrid(Vector2Int tileGridPosition)
    {
        overlapItem = null;
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem); // TODO: Разобраться, как это работает
        //Debug.Log("Down LMB bool complete ready");
        if (complete)
        {
            if (selectedItem.lastGrid.isPlayerInventory &&
                selectedItemGrid.isPlayerInventory == false)
            {
                _playerCarryingWeight.TakeWeight(selectedItem.weight * selectedItem.stackCount);
            }
            else if (selectedItem.lastGrid.isPlayerInventory == false &&
                selectedItemGrid.isPlayerInventory)
            {
                _playerCarryingWeight.AddWeight(selectedItem.weight * selectedItem.stackCount);
            }
            //Debug.Log("Down LMB weight changed");
            selectedItem.lastGrid = selectedItemGrid;
            selectedItem.SetOnCursorFlag(false);

            if (overlapItem == null)
            {
                selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                selectedItemGrid.testList.Add(selectedItem);
                selectedItem = null;
            } 
            else
            {
                if (overlapItem.name == selectedItem.name)
                {
                    int requiredItemsCount = overlapItem.maxStackCount - overlapItem.stackCount;
                    if (requiredItemsCount != 0 &&
                        overlapItem.isStackable &&
                        selectedItem.isStackable)
                    {
                        if (requiredItemsCount < selectedItem.stackCount)
                        {
                            selectedItem.SetOnCursorFlag(true);
                            overlapItem.stackCount = overlapItem.maxStackCount;
                            selectedItem.stackCount -= requiredItemsCount;
                            overlapItem.UpdateCounter();
                            selectedItem.UpdateCounter();
                        }
                        else
                        {
                            overlapItem.stackCount += selectedItem.stackCount;
                            overlapItem.UpdateCounter();
                            Destroy(selectedItem.gameObject);
                            selectedItem = null;
                        }
                    }
                    else
                    {
                        selectedItemGrid.CleanGridReference(overlapItem);
                        selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                        selectedItemGrid.testList.Add(selectedItem);
                        selectedItemGrid.testList.Remove(overlapItem);
                        overlapItem.SetOnCursorFlag(true);
                        selectedItem = overlapItem;
                        overlapItem = null;
                        itemRectTransform = selectedItem.GetComponent<RectTransform>();
                        itemRectTransform.SetAsLastSibling();
                    }
                }
                else
                {
                    selectedItemGrid.CleanGridReference(overlapItem);
                    selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                    selectedItemGrid.testList.Add(selectedItem);
                    selectedItemGrid.testList.Remove(overlapItem);
                    overlapItem.SetOnCursorFlag(true);
                    selectedItem = overlapItem;
                    overlapItem = null;
                    itemRectTransform = selectedItem.GetComponent<RectTransform>();
                    itemRectTransform.SetAsLastSibling();
                }
            }
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            if (selectedItemGridRect.rect.Overlaps(itemRectTransform.rect) == false)
            {
                itemRectTransform.SetParent(selectedItemGridRect.parent);
            }
            else
            {
                itemRectTransform.SetParent(selectedItemGridRect);
            }

            itemRectTransform.SetAsLastSibling();
            itemRectTransform.position = Input.mousePosition;
        }
    }
}
