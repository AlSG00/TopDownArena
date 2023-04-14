using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("References")]
    public SCRIPT_ItemGrid inventoryGrid;
    /*[HideInInspector]*/ public SCRIPT_ItemGrid selectedItemGrid;
    [HideInInspector] public RectTransform selectedItemGridRect;

    [SerializeField] private Player_Movement _playerMovement;
    [SerializeField] private SCRIPT_PlayerCarryingWeight _playerCarryingWeight;
    [SerializeField] private SCRIPT_InventoryHighlight inventoryHighlight;
    public SCRIPT_InventoryItem pickedInventoryItem; // Предмет с инвентаря игрока

    public SCRIPT_ItemContainer itemContainer;

    //public SCRIPT_ItemContainer itemContainer;
    public SCRIPT_InventoryItem selectedItem; // выбранный предмет, который уже висит на курсоре
    [SerializeField] private Transform canvasTransform;
    SCRIPT_InventoryItem overlapItem;
    RectTransform itemRectTransform;
    [SerializeField] private Transform itemDropPoint;
    private bool isHoldingButton = false;
    private bool isCheckingInventory = false;
    public bool isHighlightingStateIcons = false;

    private float buttonHoldTime = 0f;
    [SerializeField] private float timeToHold = 0.3f;

    public List<SCRIPT_InventoryItem> inventoryItemList = new List<SCRIPT_InventoryItem>();
    public List<SCRIPT_InventoryItem> stackableItemsTemporaryList = new List<SCRIPT_InventoryItem>();

    public delegate void OpenAction();
    public static event OpenAction OnInventoryOpened;

    public delegate void CloseAction();
    public static event CloseAction OnInventoryClosed;

    //private void OnEnable()
    //{

    //}

    //private void OnDisable()
    //{

    //}

    public SCRIPT_InventoryItem testPickedItem;
    public SCRIPT_ItemGrid testPickedItemsGrid;
    public SCRIPT_ItemGrid containerItemGrid;

    //public ItemCollection itemCollection;

    private void Awake()
    {
        SetInventoryVisibility(false);
    }

    private void Start()
    {
        inventoryGrid.testList = inventoryItemList;
    }

    private void Update()
    {
        
        ItemIconDrag();

        Debug.Log("Biba_1");
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        Debug.Log("Biba_2");
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        Debug.Log("Biba_3");
        HandleItemHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //LeftMouseButtonWithShift();
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
            Debug.Log("Govno");
            HandleStateIconsVisibility();
        }

        if (isHoldingButton &&
            isCheckingInventory == false)
        {
            buttonHoldTime += Time.deltaTime;
            if (buttonHoldTime >= timeToHold
                && isHighlightingStateIcons == false)
            {
                isHighlightingStateIcons = true;
                OnInventoryOpened?.Invoke();
            }
        }
    }

    public int InsertIntoAvailableStacks(SCRIPT_InventoryItem itemToStack, int stackCount)
    {
        //stackableItemsTemporaryList = inventoryItemList.FindAll(
        //        item => item.name == itemToStack.name
        //        );

        // TODO: Проверить, что тут вернется, если FindAll не найдет ни одного совпадения



        //foreach (int item in stackableItemsTemporaryList)
        //{

        //    Раскидать подбираемый объект по доступным стакам
        //        Потом возвращать обратно целое число оставшихся предметов в стаке
        //        Если 0 то збс, иначе проводить целиком процедуру добавления в инвентарь
        //}
        
        int leftToStack = stackCount;
        //for (int i = 0; i < inventoryGrid.inventoryItemSlot.Length; i++)
        //{
        //    for (int j = 0; j < inventoryGrid.inventoryItemSlot; j++)
        //    {
        for (int i = 0; i < inventoryGrid._gridSizeHeight; i++)
        {
            for (int j = 0; j < inventoryGrid._gridSizeWidth; j++)
            {
                if (inventoryGrid.inventoryItemSlot[i, j] != null &&
                    inventoryGrid.inventoryItemSlot[i, j].isStackable &&
                    inventoryGrid.inventoryItemSlot[i, j].name == itemToStack.name &&
                    inventoryGrid.inventoryItemSlot[i, j].stackCount < inventoryGrid.inventoryItemSlot[i, j].maxStackCount)
                {
                    я остановился вчера здесь
                    // TODO: Отладить и убедиться, что вычисления верные
                    int leftToStackTemp = inventoryGrid.inventoryItemSlot[i, j].maxStackCount - (inventoryGrid.inventoryItemSlot[i, j].stackCount + leftToStack);

                    if (leftToStackTemp >= 0)
                    {
                        inventoryGrid.inventoryItemSlot[i, j].stackCount += leftToStack;
                        return 0;
                    }
                    else
                    {
                        leftToStack -= inventoryGrid.inventoryItemSlot[i, j].maxStackCount - inventoryGrid.inventoryItemSlot[i, j].stackCount;
                        leftToStack = Mathf.Abs(leftToStack);
                        inventoryGrid.inventoryItemSlot[i, j].stackCount = inventoryGrid.inventoryItemSlot[i, j].maxStackCount;
                        //leftToStack = Mathf.Abs(leftToStackTemp);
                    }

                    //Написать метод, в котором будет увеличиваться стак и обновляться UI - счетчик;

                    //Здесь прописать какую - нибудь логику, чтобы проверять, добавился ли стак полностью
                    //  учесть, что подбираемая пачка не может быть больше, чем максимальный размер стака.Логично, но все же
                    //        return;
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
            SetInventoryVisibility(isCheckingInventory);
        }
        else
        {
            if (isCheckingInventory == false)
            {
                OnInventoryClosed?.Invoke();
            }
        }

        buttonHoldTime = 0f;
    }

    private void DropItem()
    {
        if (selectedItemGrid != inventoryGrid)
        {
            return;
        }

        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            PickItemFromGrid(tileGridPosition);
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

        Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
        inventoryItemList.Remove(pickedInventoryItem);
        _playerCarryingWeight.TakeWeight(pickedInventoryItem.weight);
        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
    }

    private void RightMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        // TODO: сделать логику для стаков
        if (selectedItem == null)
        {
            PickItemFromGrid(tileGridPosition);
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

        selectedItem.GetComponent<SCRIPT_IItem>().Use();

        //if (selectedItemGrid != inventoryGrid)
        //{
        //    itemContainer.storedItemList.Remove(pickedItem);
        //}
        //else
        //{
        //    inventoryItemList.Remove(pickedInventoryItem);
        //    _playerCarryingWeight.TakeWeight(pickedInventoryItem.weight);
        //}

        //selectedItemGrid.testItemList.Remove(pickedInventoryItem);
        selectedItemGrid.testList.Remove(pickedInventoryItem);
        if (selectedItemGrid.isPlayerInventory == true)
        {
            _playerCarryingWeight.TakeWeight(pickedInventoryItem.weight);
        }


        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
    }

    //private void PlaceItem(Vector2Int tileGridPosition)
    //{

    //    bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
    //    if (complete)
    //    {
    //        selectedItem = null;
    //        if (overlapItem != null)
    //        {
    //            selectedItem = overlapItem;
    //            overlapItem = null;
    //            itemRectTransform = selectedItem.GetComponent<RectTransform>();
    //            itemRectTransform.SetAsLastSibling();
    //        }

    //        HandleLists(tileGridPosition);
            
    //        pickedItem = null;
    //        pickedInventoryItem = null;
    //    }
    //}

    // Учитывать, что в будущем этот метод используется для взятия всего стака
    Vector2Int previousPosition;
    private void PickItemFromGrid(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        //pickedItem = null;
        pickedInventoryItem = null;
        if (selectedItem != null)
        {
            //if (selectedItemGrid != inventoryGrid)
            //{
            //    previousPosition.x = selectedItem.onGridPositionX;
            //    previousPosition.y = selectedItem.onGridPositionY;

            //    pickedItem = itemContainer.storedItemList.Find(x =>
            //    x.positionOnGrid.x == previousPosition.x &&
            //    x.positionOnGrid.y == previousPosition.y
            //    );
            //}
            //else
            //{
            //    previousPosition.x = selectedItem.onGridPositionX;
            //    previousPosition.y = selectedItem.onGridPositionY;

            //    pickedInventoryItem = inventoryItemList.Find(x =>
            //    x.PositionOnGrid.x == previousPosition.x &&
            //    x.PositionOnGrid.y == previousPosition.y
            //    );
            //}

            //    previousPosition.x = selectedItem.onGridPositionX;
            //    previousPosition.y = selectedItem.onGridPositionY;

            //    pickedItem = itemContainer.storedItemList.Find(x =>
            //    x.positionOnGrid.x == previousPosition.x &&
            //    x.positionOnGrid.y == previousPosition.y
            //    );

            previousPosition.x = selectedItem.positionOnGrid.x;
            previousPosition.y = selectedItem.positionOnGrid.y;
            testPickedItem = selectedItemGrid.testList.Find(x =>
            x.positionOnGrid.x == previousPosition.x &&
            x.positionOnGrid.y == previousPosition.y
            );

            //НОВОЕ
            selectedItemGrid.testList.Remove(testPickedItem);

            itemRectTransform = selectedItem.GetComponent<RectTransform>();
            itemRectTransform.SetAsLastSibling();
        }
    }

    public void InsertItemIntoInventory(SCRIPT_InventoryItem item)
    {
        if (selectedItemGrid == null)
        {
            Debug.Log("Grid is not selected");
            return;
        }

        CreateItemForUi(item);
        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;

        InsertItem(itemToInsert);

        //InventoryItem itemToPick = new InventoryItem(
        //    item.GetComponent<SCRIPT_PickableObject>().inventoryPrefab,
        //    itemToInsert.onGridPositionX,
        //    itemToInsert.onGridPositionY,
        //    itemToInsert.isRotated,
        //    itemToInsert.weight,
        //    itemToInsert.Width,
        //    itemToInsert.Height
        //    );

        //itemToPick.inventoryItem = itemToInsert;
        //itemToPick.Name = itemToInsert.name;
        //itemToPick.maxStackCount = itemToInsert.maxStackCount;

        //inventoryItemList.Add(itemToPick);
        //_playerCarryingWeight.AddWeight(itemToPick.Weight);


        //inventoryItemList.Add(itemToInsert);
        // TODO: Продумать здесь логику на случай, если в инвентаре будет несколько разных сеток
        inventoryGrid.testList.Add(itemToInsert);
        _playerCarryingWeight.AddWeight(itemToInsert.weight);
    }

    //public void CreateItem(GameObject item)
    public void CreateItemForUi(SCRIPT_InventoryItem item)
    {
        //_itemPrefab = item.GetComponent<SCRIPT_PickableObject>().inventoryPrefab;
        //SCRIPT_InventoryItem inventoryItem = Instantiate(_itemPrefab).GetComponent<SCRIPT_InventoryItem>();

        //selectedItem = inventoryItem;
        //rectTransform = inventoryItem.GetComponent<RectTransform>();
        //rectTransform.SetParent(canvasTransform);
        //rectTransform.SetAsLastSibling();
        //inventoryItem.Set(inventoryItem.itemData);

        // _itemPrefab = item.GetComponent<SCRIPT_PickableObject>().inventoryPrefab;
        SCRIPT_InventoryItem inventoryItem = Instantiate(item);

        selectedItem = inventoryItem;
        itemRectTransform = inventoryItem.GetComponent<RectTransform>();
        itemRectTransform.SetParent(canvasTransform);
        itemRectTransform.SetAsLastSibling();
        inventoryItem.Set(inventoryItem.itemData);
    }

    public void InsertItemIntoContainer(SCRIPT_InventoryItem item)
    {
        //CreateContainerItem(item);

        CreateItemForUi(item);

        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;

        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (positionOnGrid == null)
        {
            return;
        }
        selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);

        //SCRIPT_ItemContainer.StoredItem itemTostore = new SCRIPT_ItemContainer.StoredItem(
        //    item,
        //    positionOnGrid.Value.x,
        //    positionOnGrid.Value.y,
        //    itemToInsert.isRotated,
        //    itemToInsert.weight,
        //    itemToInsert.Width,
        //    itemToInsert.Height
        //    );

        //itemTostore.item = item;
        //itemTostore.positionOnGrid.x = positionOnGrid.Value.x;
        //itemTostore.positionOnGrid.y = positionOnGrid.Value.y;
        selectedItemGrid.testList.Add(itemToInsert);
    }

    private void InsertItem(SCRIPT_InventoryItem itemToInsert)
    {
        //if (itemToInsert.isStackable)
        //{
        //    попытаться простакать предмет
        //    //SCRIPT_InventoryItem itemToStack = inventoryItemList.Find(item => item.name == itemToInsert.name);
        //    //if (itemToStack != null)
        //    //{
        //    //    if (itemToStack.TryAddToStack())
        //    //    {
        //    //        itemToInsert.stackCounter.text = itemToStack.stackCount.ToString();
        //    //        return;

        //    //        // На память, а то кодил в полудрёме:
        //    //        // по задумке пытаемся найти в инвентаре предмет с такой же меткой имени
        //    //        // если нашли и его можно застакать, то стакаем
        //    //        // если стакнется, то вернется true
        //    //        // если не стакнется то по классике будет пытаться искать свободное место
        //    //    }
        //    //}
        //}

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
    }

    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        // TODO: если смогу избавиться от InventoryItem, то здесь можно будет все упростить
        //if (pickedInventoryItem != null)
        //{
        //    //SCRIPT_InventoryItem item = inventoryItemList.Find(x =>
        //    //x.positionOnGrid.x == selectedItem.onGridPositionX &&
        //    //x.positionOnGrid.y == selectedItem.onGridPositionY
        //    //);

        //    SCRIPT_InventoryItem item = inventoryItemList.Find(x =>
        //    x.positionOnGrid.x == selectedItem.positionOnGrid.x &&
        //    x.positionOnGrid.y == selectedItem.positionOnGrid.y
        //    );

        //    if (item.isRotatable)
        //    {
        //        item.isRotated = !item.isRotated;
        //    }
        //}
        //else if (pickedItem != null)
        //{
        //    SCRIPT_ItemContainer.StoredItem item = itemContainer.storedItemList.Find(x =>
        //    x.positionOnGrid.x == selectedItem.onGridPositionX &&
        //    x.positionOnGrid.y == selectedItem.onGridPositionY
        //    );

        //    if (item.isRotatable)
        //    {
        //        item.isRotated = !item.isRotated;
        //    }
        //}

        if (testPickedItem != null)
        {
            SCRIPT_InventoryItem item = testPickedItemsGrid.testList.Find(x =>
            x.positionOnGrid.x == selectedItem.positionOnGrid.x &&
            x.positionOnGrid.y == selectedItem.positionOnGrid.y
            );

            if (item.isRotatable)
            {
                item.isRotated = !item.isRotated;
            }
        }

        selectedItem.Rotated();
    }

    //Возникнет путаница между SelectedItem и testPickedItem.Уже сам запутался :D. Разобраться;

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
            //inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(
            //    positionOnGrid.x,
            //    positionOnGrid.y,
            //    selectedItem.itemData.width,
            //    selectedItem.itemData.height)
            //    );

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
        Vector2 position = new Vector2();
        RectTransform inventoryRect = inventoryGrid.GetComponent<RectTransform>(); // TODO: убрать GetComponent

        if (isInventoryOpened)
        {
            _playerMovement.enabled = false;
            position.y = 630;
            OnInventoryOpened?.Invoke();
        }
        else
        {
            GetItemBack();
            position.y = 3000;

            if (containerItemGrid != null)
            {
                //containerItemGrid.HandleContainerGrid(false);
                containerItemGrid.SetVisibility(false);
            }

            OnInventoryClosed?.Invoke();
            _playerMovement.enabled = true;
        }
        position.x = inventoryRect.position.x;
        inventoryRect.position = position;
    }

    public void GetItemBack()
    {
        if (selectedItem == null)
        {
            return;
        }

        //Vector2Int lastPosition = new Vector2Int(selectedItem.onGridPositionX, selectedItem.onGridPositionY);
        Vector2Int lastPosition = new Vector2Int(selectedItem.positionOnGrid.x, selectedItem.positionOnGrid.y);

        //if (pickedItem != null)
        //{
        //    selectedItemGrid = itemContainer.containerGrid;
        //}
        //else if (pickedInventoryItem != null)
        //{
        //    selectedItemGrid = inventoryGrid;
        //}

        selectedItemGrid = testPickedItemsGrid;

        PlaceItemOnGrid(lastPosition);
    }

    //Протестить функцию
    private void PlaceItemOnGrid(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                itemRectTransform = selectedItem.GetComponent<RectTransform>();
                itemRectTransform.SetAsLastSibling();
            }

            //HandleLists(tileGridPosition);

            //Нужно переделать код так, чтобы предметы удалялись из списка в момент поднятия, а не перемещения
            // НОВОЕ
            selectedItemGrid.testList.Add(testPickedItem);
            testPickedItem.positionOnGrid.x = tileGridPosition.x;
            testPickedItem.positionOnGrid.y = tileGridPosition.y;
            //pickedItem = null;
            pickedInventoryItem = null;
        }
    }

    public void InsertItemIntoInitializedContainer(/*SCRIPT_ItemContainer.StoredItem*/SCRIPT_InventoryItem storedItem /*GameObject item*/)
    {
        //CreateContainerItem(storedItem.item);
        CreateItemForUi(storedItem);

        if (storedItem.isRotated)
        {
            RotateItem();
        }

        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;

        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (positionOnGrid == null)
        {
            return;
        }

        selectedItemGrid.PlaceItem(itemToInsert, storedItem.positionOnGrid.x, storedItem.positionOnGrid.y);
    }

    // TODO: переименовать метод.
    // Определяет, добавить новый предмет в сетку инвентаря игрока или контейнера
    //private void HandleLists(Vector2Int tileGridPosition)
    //{
    //    if (selectedItemGrid != inventoryGrid)
    //    {
    //        if (pickedItem == null)
    //        {
    //            pickedItem = new SCRIPT_ItemContainer.StoredItem(
    //                pickedInventoryItem.Item,
    //                0,
    //                0,
    //                pickedInventoryItem.IsRotated,
    //                pickedInventoryItem.Weight,
    //                pickedInventoryItem.Width,
    //                pickedInventoryItem.Height
    //                );
    //            itemContainer.storedItemList.Add(pickedItem);
    //            //pickedItem.item = pickedInventoryItem.item;
    //            //pickedItem.isRotated = pickedInventoryItem.isRotated;
    //            inventoryItemList.Remove(pickedInventoryItem);
    //            _playerCarryingWeight.TakeWeight(pickedInventoryItem.weight);
    //        }
    //        pickedItem.positionOnGrid.x = tileGridPosition.x;
    //        pickedItem.positionOnGrid.y = tileGridPosition.y;

    //    }
    //    else
    //    {
    //        if (pickedInventoryItem == null)
    //        {
    //            Debug.Log(pickedItem);
    //            pickedInventoryItem = new InventoryItem(
    //                pickedItem.item,
    //                0,
    //                0,
    //                pickedItem.isRotated,
    //                pickedItem.weight,
    //                pickedItem.Width,
    //                pickedItem.Height
    //                );

    //            inventoryItemList.Add(pickedInventoryItem);
    //            //pickedInventoryItem.item = pickedItem.item;
    //            //pickedInventoryItem.isRotated = pickedItem.isRotated;
    //            itemContainer.storedItemList.Remove(pickedItem);
    //            _playerCarryingWeight.AddWeight(pickedItem.weight);
    //        }

    //        pickedInventoryItem.PositionOnGrid.x = tileGridPosition.x;
    //        pickedInventoryItem.PositionOnGrid.y = tileGridPosition.y;
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
