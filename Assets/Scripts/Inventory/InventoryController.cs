using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("References")]
    public SCRIPT_ItemGrid inventoryGrid;
    public SCRIPT_ItemGrid selectedItemGrid;
    [HideInInspector] public RectTransform selectedItemGridRect;

    [SerializeField] private Player_Movement _playerMovement;
    [SerializeField] private SCRIPT_PlayerCarryingWeight _playerCarryingWeight;
    [SerializeField] private SCRIPT_InventoryHighlight inventoryHighlight;


    public SCRIPT_ItemContainer itemContainer;
    
    [SerializeField] private Transform canvasTransform;
    
    RectTransform itemRectTransform;
    [SerializeField] private Transform itemDropPoint;
    private bool isHoldingButton = false;
    public bool isCheckingInventory = false;
    public bool isHighlightingStateIcons = false;

    private float buttonHoldTime = 0f;
    [SerializeField] private float timeToHold = 0.3f;

    public List<SCRIPT_InventoryItem> inventoryItemList = new List<SCRIPT_InventoryItem>();


    public delegate void OpenAction(bool isOpened);
    public static event OpenAction OnInventoryOpened;
    //public static event OpenAction OnInventoryClosed;

    public delegate void ShowStateAction();
    public static event OpenAction OnStateIconShow;


    public SCRIPT_InventoryItem selectedItem; // выбранный предмет, который уже висит на курсоре
    SCRIPT_InventoryItem overlapItem; // TODO: вспомнить, зачем это

    public SCRIPT_ItemGrid testPickedItemsGrid; // Сетка, с которой был взят текущий предмет
    public SCRIPT_ItemGrid containerItemGrid;

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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Work in progress");
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
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isHoldingButton = true;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            HandleStateIconsVisibility();
        }

        // TODO: Как нибудь переделать эту страшную штуку
        if (isHoldingButton &&
            isCheckingInventory == false)
        {
            buttonHoldTime += Time.deltaTime;
            if (buttonHoldTime >= timeToHold
                && isHighlightingStateIcons == false)
            {
                isHighlightingStateIcons = true;
                OnStateIconShow?.Invoke(true);
            }
        }
    }

    public int InsertIntoAvailableStacks(SCRIPT_InventoryItem itemToStack, int stackCount)
    {
        int leftToStack = stackCount;

        for (int i = 0; i < inventoryGrid._gridSizeHeight; i++)
        {
            for (int j = 0; j < inventoryGrid._gridSizeWidth; j++)
            {
                if (inventoryGrid.inventoryItemSlot[i, j] != null &&
                    inventoryGrid.inventoryItemSlot[i, j].isStackable &&
                    inventoryGrid.inventoryItemSlot[i, j].name == itemToStack.name &&
                    inventoryGrid.inventoryItemSlot[i, j].stackCount < inventoryGrid.inventoryItemSlot[i, j].maxStackCount)
                {
                    // TODO: Отладить и убедиться, что вычисления верные
                    // int leftToStackTemp = inventoryGrid.inventoryItemSlot[i, j].maxStackCount - (inventoryGrid.inventoryItemSlot[i, j].stackCount + leftToStack);

                    //// тут неверное считаются стаки. Отладить

                    // if (leftToStackTemp >= 0)
                    // {
                    //     inventoryGrid.inventoryItemSlot[i, j].stackCount += leftToStack;
                    //     inventoryGrid.inventoryItemSlot[i, j].UpdateCounter();
                    //     _playerCarryingWeight.AddWeight(leftToStack * inventoryGrid.inventoryItemSlot[i, j].weight);
                    //     return 0;
                    // }
                    // else
                    // {

                    //     leftToStack = leftToStack - (inventoryGrid.inventoryItemSlot[i, j].maxStackCount - inventoryGrid.inventoryItemSlot[i, j].stackCount);
                    //     leftToStack = Mathf.Abs(leftToStack);
                    //     float weightToAdd = inventoryGrid.inventoryItemSlot[i, j].maxStackCount - inventoryGrid.inventoryItemSlot[i, j].stackCount + leftToStack;
                    //     _playerCarryingWeight.AddWeight(weightToAdd);
                    //     inventoryGrid.inventoryItemSlot[i, j].stackCount = inventoryGrid.inventoryItemSlot[i, j].maxStackCount;
                    //     //_playerCarryingWeight.AddWeight(weightToAdd);


                    // }

                    int temp = leftToStack + inventoryGrid.inventoryItemSlot[i, j].stackCount;

                    if (temp < inventoryGrid.inventoryItemSlot[i, j].maxStackCount)
                    {
                        _playerCarryingWeight.AddWeight(leftToStack * inventoryGrid.inventoryItemSlot[i, j].weight);
                        inventoryGrid.inventoryItemSlot[i, j].stackCount += leftToStack;
                        inventoryGrid.inventoryItemSlot[i, j].UpdateCounter();
                        return 0;
                    }
                    else
                    {
                        int valueToFillStack = inventoryGrid.inventoryItemSlot[i, j].maxStackCount - inventoryGrid.inventoryItemSlot[i, j].stackCount;
                        leftToStack -= valueToFillStack;
                        inventoryGrid.inventoryItemSlot[i, j].stackCount += valueToFillStack;
                        _playerCarryingWeight.AddWeight(valueToFillStack * inventoryGrid.inventoryItemSlot[i, j].weight);
                    }

                    inventoryGrid.inventoryItemSlot[i, j].UpdateCounter();
                }
            }
        }

        return leftToStack;
    }

    private void HandleStateIconsVisibility()
    {
        isHoldingButton = false;
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
                UpdateCarryingWeight(selectedItem);
                Destroy(selectedItem.gameObject);
                inventoryHighlight.Show(false);
            }
            else
            {
                GameObject droppedItem = Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
                PickableObject droppedItemData = droppedItem.GetComponent<PickableObject>();
                droppedItemData.stackCount = selectedItem.stackCount;
                selectedItemGrid.testList.Remove(selectedItem);
                UpdateCarryingWeight(selectedItem);
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
                    UpdateCarryingWeight(selectedItem);
                    Destroy(selectedItem.gameObject);
                    inventoryHighlight.Show(false);
                }
                else
                {
                    selectedItem.isDropping = false;
                    selectedItem.stackCount--;
                    selectedItem.UpdateCounter();
                    UpdateCarryingWeight(selectedItem);
                }
                Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
            }
            else
            {
                GameObject droppedItem = Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
                PickableObject droppedItemData = droppedItem.GetComponent<PickableObject>();
                droppedItemData.stackCount = selectedItem.stackCount;
                selectedItemGrid.testList.Remove(selectedItem);
                UpdateCarryingWeight(selectedItem);
                Destroy(selectedItem.gameObject);
                inventoryHighlight.Show(false);
            }
        }

        selectedItem = null;
    }

    private void UpdateCarryingWeight(SCRIPT_InventoryItem item)
    {
        if (item.lastGrid.isPlayerInventory)
        {
            if (item.isSingleDropping)
            {
                _playerCarryingWeight.TakeWeight(item.weight);
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
        // TODO: сделать логику для стаков
        if (selectedItem == null)
        {
            selectedItem = selectedItemGrid.inventoryItemSlot[tileGridPosition.x, tileGridPosition.y];
        }

        if (selectedItem.isOnCursor)
        {
            return;
        }

        //TODO: По идее, здесь не сработает данное условие, переделать на обращение к InventoryItem
        if (selectedItem.GetComponent<SCRIPT_IItem>().isUsable == false)
        {
            PlaceItemOnGrid(tileGridPosition);
            return;
        }

        if (selectedItem.useItemAudioSource != null &&
            selectedItem.useItemAudio != null)
        {
            selectedItem.useItemAudioSource.PlayOneShot(selectedItem.useItemAudio);
        }

        //Проверить здесь, чтобы вес предметов менялся правильно, когда исользуешь их из стака
        selectedItem.GetComponent<SCRIPT_IItem>().Use();

        //selectedItemGrid.testList.Remove(selectedItem);
        if (selectedItemGrid.isPlayerInventory == true)
        {
            _playerCarryingWeight.TakeWeight(selectedItem.weight);
        }

        // TODO: Сделать, чтобы вес уменьшался
        if (selectedItem.stackCount > 1)
        {
            selectedItem.stackCount--;
            selectedItem.UpdateCounter();
        }
        else
        {
            selectedItemGrid.testList.Remove(selectedItem); // НОВОЕ
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
            selectedItem.isOnCursor = true;
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
        //inventoryGrid.testList.Add(itemToInsert);
        _playerCarryingWeight.AddWeight(itemToInsert.weight * itemToInsert.stackCount);
    }

    public void CreateItemForUi(SCRIPT_InventoryItem item)
    {
        SCRIPT_InventoryItem inventoryItem = Instantiate(item/*.uiPrefab.GetComponent<SCRIPT_InventoryItem>()*/);
        //Destroy(item.gameObject);
        selectedItem = inventoryItem;
        itemRectTransform = inventoryItem.GetComponent<RectTransform>();
        itemRectTransform.SetParent(canvasTransform);
        itemRectTransform.SetAsLastSibling();
        inventoryItem.Set(inventoryItem.itemData);
    }

    private void FillContainerGrid(bool isInitialized, List<SCRIPT_InventoryItem> storedItemList, SCRIPT_ItemGrid containerGrid)
    {
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

    //public void InsertItemIntoContainer(List<SCRIPT_InventoryItem> storedItemList)
    //{
    //   // sdf
    //    //selectedItemGrid.testList = new List<SCRIPT_InventoryItem>();
    //    foreach (var item in storedItemList)
    //    {
    //        CreateItemForUi(item);
    //        SCRIPT_InventoryItem itemToInsert = selectedItem;
    //        selectedItem = null;
    //        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
    //        if (positionOnGrid == null)
    //        {
    //            return;
    //        }
    //        selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);
    //        item.positionOnGrid.x = positionOnGrid.Value.x;
    //        item.positionOnGrid.y = positionOnGrid.Value.y;
    //        selectedItemGrid.testList.Add(item);
    //    }
    //}

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
        //Начать отладку отсюда. Нажать на предмет из контейнера и посмотреть, что будет
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
            // TODO: Добавить логику для стаков
            PlaceItemOnGrid(tileGridPosition);
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
            //Vector2Int lastPosition = new Vector2Int(selectedItem.positionOnGrid.x, selectedItem.positionOnGrid.y);
            selectedItemGrid = selectedItem.lastGrid;
           // selectedItemGrid.testList.Add(selectedItem);
            PlaceItemOnGrid(selectedItem.positionOnGrid);
        }
    }

    //Протестить функцию
    private void PlaceItemOnGrid(Vector2Int tileGridPosition)
    {
        overlapItem = null;
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem); // TODO: Разобраться, как это работает
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

            selectedItem.lastGrid = selectedItemGrid;
            selectedItem.isOnCursor = false;

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
                    if (requiredItemsCount != 0)
                    {
                        if (requiredItemsCount < selectedItem.stackCount)
                        {
                            selectedItem.isOnCursor = true;
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
                    selectedItem = overlapItem;
                    overlapItem = null;
                    itemRectTransform = selectedItem.GetComponent<RectTransform>();
                    itemRectTransform.SetAsLastSibling();
                }
            }
        }
    }

    //private void PlaceItemOnGrid(Vector2Int tileGridPosition)
    //{
    //    overlapItem = null;
    //    bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem); // TODO: Разобраться, как это работает
    //    if (complete)
    //    {
    //        if (selectedItem.lastGrid.isPlayerInventory &&
    //            selectedItemGrid.isPlayerInventory == false)
    //        {
    //            _playerCarryingWeight.TakeWeight(selectedItem.weight * selectedItem.stackCount);
    //        }
    //        else if (selectedItem.lastGrid.isPlayerInventory == false &&
    //            selectedItemGrid.isPlayerInventory)
    //        {
    //            _playerCarryingWeight.AddWeight(selectedItem.weight * selectedItem.stackCount);
    //        }

    //        selectedItem.lastGrid = selectedItemGrid;
    //        selectedItem.isOnCursor = false;
    //        selectedItem = null;
    //        if (overlapItem != null)
    //        {
    //            selectedItem = overlapItem;
    //            overlapItem = null;
    //            itemRectTransform = selectedItem.GetComponent<RectTransform>();
    //            itemRectTransform.SetAsLastSibling();
    //        }
    //    }
    //}

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
