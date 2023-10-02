using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class InventoryController : MonoBehaviour
{
    // TODO: Move to separate file with audio method
    private enum AudioPlayMode
    {
        PickFromGround,
        PickFromGrid,
        PlaceOnGrid,
        Use
    }

    // TODO: Opportunity to exzpand available equippable slots
    public enum BindSlot
    {
        HolsterSlot,
        BeltSlot,
        BackSlot
    }

    public Transform[] holsterSlotOffsetsArray;
    public Transform[] beltSlotOffsetsArray;
    public Transform[] backSlotOffsetsArray;

    #region VARIABLES
    //public Transform[] itemSlotPivots;

    //private Dictionary<BindSlot, SCRIPT_InventoryItem> _bindSlots;
    private Dictionary<BindSlot, EquipmentSlot> _bindSlots;

    //public SCRIPT_InventoryItem bindedItem_1;
    //public SCRIPT_InventoryItem bindedItem_2;
    //public SCRIPT_InventoryItem bindedItem_3;

    public class EquipmentSlot
    {
        public string name;
        public SCRIPT_InventoryItem item;
        public Transform[] pivots;

        public EquipmentSlot(SCRIPT_InventoryItem assignedItem, Transform[] SlotPivotsArray, string slotName)
        {
            name = slotName;
            item = assignedItem;
            pivots = SlotPivotsArray;
        }
    }

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

    [Header("Audio")]
    [SerializeField] private AudioSource inventoryAudioSource;

    [Header("States")]
    public bool isCheckingInventory = false;
    public bool isHighlightingStateIcons = false;
    public bool isDroppingStack = false;
    public delegate void OpenAction(bool isOpened);
    public static event OpenAction OnInventoryOpened;
    public static event OpenAction OnStateIconShow;
    public static event Action OnUnablePickItem;

    #endregion

    #region START INIT

    private void OnEnable()
    {
        SCRIPT_ItemContainer.OnContainerOpen += FillContainerGrid;
        PickableObject.OnItemPick += PickItemFromGround;
        PickableWeapon.OnWeaponPick += PickItemFromGround;
    }

    private void OnDisable()
    {
        SCRIPT_ItemContainer.OnContainerOpen -= FillContainerGrid;
        PickableObject.OnItemPick -= PickItemFromGround;
        PickableWeapon.OnWeaponPick -= PickItemFromGround;
    }

    private void Awake()
    {
        try
        {
            _bindSlots = new Dictionary<BindSlot, EquipmentSlot>()
            {
                { BindSlot.HolsterSlot, new(null, holsterSlotOffsetsArray, "Holster")},
                { BindSlot.BeltSlot, new(null, beltSlotOffsetsArray, "Belt")},
                { BindSlot.BackSlot, new(null, backSlotOffsetsArray, "Back")}
            };
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void Start()
    {
        inventoryGrid.testList = inventoryItemList;
        isCheckingInventory = false;
        SetInventoryVisibility(isCheckingInventory);
    }

    #endregion

    private void Update()
    {
        ItemIconDrag();
        HandleItemHighlight();
    }

    #region DROP LOGIC

    internal void HandleItemDrop()
    {
        //isHoldingDropItemButton = false;
        if (isDroppingStack)
        {
            isDroppingStack = false;
        }
        else
        {
            TryUnbindItem();
            DropItem();
        }
    }

    internal void HandleStackDrop(ref float buttonHoldTime, float timeToHold)
    {
        buttonHoldTime += Time.deltaTime;
        if (buttonHoldTime >= timeToHold)
        {
            buttonHoldTime = 0;
            isDroppingStack = true;
            DropStack();
        }
    }

    // TODO: Incapsulate in InventoryItem or something else
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
            //if (selectedItem.isDividable)
            //{
            //    for (int i = 0; i < selectedItem.stackCount; i++)
            //    {
            //        Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
            //    }
            //    selectedItemGrid.testList.Remove(selectedItem);
            //    UpdateCarryingWeight(selectedItem, true);
            //    Destroy(selectedItem.gameObject);
            //    inventoryHighlight.Show(false);
            //}
            //else
            //{
            //    GameObject droppedItem = Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
            //    PickableObject droppedItemData = droppedItem.GetComponent<PickableObject>();
            //    droppedItemData.stackCount = selectedItem.stackCount;
            //    selectedItemGrid.testList.Remove(selectedItem);
            //    UpdateCarryingWeight(selectedItem, true);
            //    Destroy(selectedItem.gameObject);
            //    inventoryHighlight.Show(false);
            //}

            // NEW: Without isDividable
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
            //if (selectedItem.isDividable)
            //{
            //    if (selectedItem.stackCount == 1)
            //    {
            //        selectedItemGrid.testList.Remove(selectedItem);
            //        UpdateCarryingWeight(selectedItem, false);
            //        itemInfoWindow.SetVisibility(false, selectedItem);
            //        Destroy(selectedItem.gameObject);
            //        inventoryHighlight.Show(false);
            //    }
            //    else
            //    {
            //        selectedItem.isDropping = false;
            //        selectedItem.stackCount--;
            //        selectedItem.UpdateCounter();
            //        UpdateCarryingWeight(selectedItem, false);
            //    }
            //    Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
            //}
            //else
            //{
            //    GameObject droppedItem = Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
            //    PickableObject droppedItemData = droppedItem.GetComponent<PickableObject>();
            //    droppedItemData.stackCount = selectedItem.stackCount;
            //    selectedItemGrid.testList.Remove(selectedItem);
            //    UpdateCarryingWeight(selectedItem, true);
            //    itemInfoWindow.SetVisibility(false, selectedItem);
            //    Destroy(selectedItem.gameObject);
            //    inventoryHighlight.Show(false);
            //}

            // NEW: without isDividable
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
        selectedItemGrid.PickFromGrid(selectedItem.positionOnGrid.x, selectedItem.positionOnGrid.y);


        //if (selectedItem.isDividable)
        //{
        //    for (int i = 0; i < selectedItem.stackCount; i++)
        //    {
        //        Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
        //    }
        //    selectedItemGrid.testList.Remove(selectedItem);
        //    UpdateCarryingWeight(selectedItem, true);
        //    Destroy(selectedItem.gameObject);
        //    inventoryHighlight.Show(false);
        //}
        //else
        //{
        //    GameObject droppedItem = Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
        //    PickableObject droppedItemData = droppedItem.GetComponent<PickableObject>();
        //    droppedItemData.stackCount = selectedItem.stackCount;
        //    selectedItemGrid.testList.Remove(selectedItem);
        //    UpdateCarryingWeight(selectedItem, true);
        //    Destroy(selectedItem.gameObject);
        //    inventoryHighlight.Show(false);
        //}

        // NEW: without isDividable
        // TODO: This part seems can to be moved to a separate method
        for (int i = 0; i < selectedItem.stackCount; i++)
        {
            Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().objectPrefab, itemDropPoint.position, Quaternion.identity);
        }
        selectedItemGrid.testList.Remove(selectedItem);
        UpdateCarryingWeight(selectedItem, true);
        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
    }

    #endregion

    #region STATE ICONS
    // TODO: Move to separate file
    internal void HandleStateIconButtonHolding(ref float buttonHoldTime, float timeToHold)
    {
        buttonHoldTime += Time.deltaTime;
        if (buttonHoldTime >= timeToHold
            && isHighlightingStateIcons == false)
        {
            Debug.Log("<color=green>Show icons</color>");
            buttonHoldTime = 0;
            isHighlightingStateIcons = true;
            OnStateIconShow?.Invoke(true);
        }
    }

    // TODO: Move to a a separate file. Rename method
    internal void HandleStateIconsVisibility(ref float buttonHoldTime, float timeToHold)
    {
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

    #endregion

    // TODO: Check if it can be refactored
    public int InsertIntoAvailableStacks(SCRIPT_InventoryItem itemToStack, int stackCount, bool addToInventory)
    {
        if (itemToStack.isStackable == false)
        {
            return stackCount;
        }

        if (addToInventory)
        {
            selectedItemGrid = inventoryGrid;
        }

        int leftToStack = stackCount;


        // in this giant cycle we searching same items on grid and adding values to their stack count
        for (int i = 0; i < selectedItemGrid._gridSizeHeight; i++)
        {
            for (int j = 0; j < selectedItemGrid._gridSizeWidth; j++)
            {
                if (selectedItemGrid.inventoryItemSlot[j, i] != null &&
                    selectedItemGrid.inventoryItemSlot[j, i].isStackable &&
                    selectedItemGrid.inventoryItemSlot[j, i].name == itemToStack.name &&
                    selectedItemGrid.inventoryItemSlot[j, i].stackCount < selectedItemGrid.inventoryItemSlot[j, i].maxStackCount)
                {
                    int finalStackCount = leftToStack + selectedItemGrid.inventoryItemSlot[j, i].stackCount;

                    if (finalStackCount < selectedItemGrid.inventoryItemSlot[j, i].maxStackCount)
                    {
                        // Update carrying weight if item moved from inventory to container or backwards
                        if (addToInventory && (selectedItemGrid != itemToStack.lastGrid))
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

    private void UpdateCarryingWeight(SCRIPT_InventoryItem item, bool dropFullStack)
    {
        if (item.lastGrid.isPlayerInventory == false)
        {
            return;
        }

        //if (item.isDividable)
        //{
        //    if (dropFullStack)
        //    {
        //        _playerCarryingWeight.TakeWeight(item.weight * item.stackCount);
        //    }
        //    else
        //    {
        //        _playerCarryingWeight.TakeWeight(item.weight);
        //    }
        //}
        //else
        //{
        //    _playerCarryingWeight.TakeWeight(item.weight * item.stackCount);
        //}

        // NEW: without isDividable
        if (dropFullStack)
        {
            _playerCarryingWeight.TakeWeight(item.weight * item.stackCount);
        }
        else
        {
            _playerCarryingWeight.TakeWeight(item.weight);
        }
    }

    // TODO: Method is too big. Refactor (No(Yes))
    internal void RightMouseButtonPress()
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
                //if (secondItem.name == selectedItem.name &&
                //    selectedItem.isStackable &&
                //    selectedItem.isDividable)
                if (secondItem.name == selectedItem.name &&
                    selectedItem.isStackable)
                {
                    if (secondItem.stackCount >= secondItem.maxStackCount)
                    {
                        return;
                    }

                    secondItem.stackCount++;
                    selectedItem.stackCount--;
                    secondItem.UpdateCounter();
                    selectedItem.UpdateCounter();
                    PlayItemAudio(selectedItem, AudioPlayMode.PlaceOnGrid);
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
                    //}
                    //else
                    //{
                    //    return;
                    //}
                }
                else
                {
                    return;
                }
            }
            else
            {
                //if (selectedItem.stackCount > 1 &&
                //    selectedItem.isDividable)
                if (selectedItem.stackCount > 1)
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

                PlayItemAudio(selectedItem, AudioPlayMode.PlaceOnGrid);
            }

            return;
        }

        if (selectedItem.isUsable == false && selectedItem.isOnCursor)
        {
            PlaceItemOnGrid(tileGridPosition);
            return;
        }

        if (selectedItem.isUsable)
        {
            selectedItem.GetComponent<SCRIPT_IItem>().Use();
        }
        else
        {
            return;
        }

        if (selectedItemGrid.isPlayerInventory && selectedItem.isConsumable)
        {
            _playerCarryingWeight.TakeWeight(selectedItem.weight);

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
        }
        PlayItemAudio(selectedItem, AudioPlayMode.Use);
        selectedItem = null;
    }

    private int PickItemFromGround(SCRIPT_InventoryItem item, int stackCount)
    {
        selectedItemGrid = inventoryGrid;
        int stackCountRemaining = InsertIntoAvailableStacks(item, stackCount, true);
        if (stackCountRemaining > 0)
        { 
            Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(item);
            if (positionOnGrid == null)
            {
                if (stackCount == stackCountRemaining)
                {
                    OnUnablePickItem?.Invoke();
                }
                else
                {
                    PlayItemAudio(item, AudioPlayMode.PickFromGround);
                }

                //stackCount = stackCountRemaining;
                return stackCount;
            }
            
            InsertItemIntoInventory(item, stackCountRemaining);
        }
        PlayItemAudio(item, AudioPlayMode.PickFromGround);
        return 0;
    }

    // Create, place on inventory grid and add some weight
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



    // Return free grid coordinates and calculate on-screen pixel position to place sprite
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





    internal void LeftMouseButtonPress()
    {
        if (isCheckingInventory == false)
        {
            return;
        }

        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickItemFromGrid(tileGridPosition);
            selectedItem.GetComponent<Image>().raycastTarget = false;
            PlayItemAudio(selectedItem, AudioPlayMode.PickFromGrid);
        }
        else
        {
            SCRIPT_InventoryItem itemToPlace = selectedItem;
            PlaceItemOnGrid(tileGridPosition);
            PlayItemAudio(itemToPlace, AudioPlayMode.PlaceOnGrid);
        }
    }

    internal void MoveItemFast()
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
                TryUnbindItem();
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
                    
                    selectedItem.isOnCursor = false;
                    itemInfoWindow.SetVisibility(false, selectedItem);
                    PlayItemAudio(selectedItem, AudioPlayMode.PickFromGrid);
                    selectedItem = null;

                    InsertItem(itemToInsert);
                    itemToInsert.GetComponent<Image>().raycastTarget = true;
                    selectedItemGrid = inventoryGrid;
                }
                else
                {
                    itemInfoWindow.SetVisibility(false, selectedItem);
                    if (selectedItem.lastGrid != selectedItemGrid)
                    {
                        _playerCarryingWeight.TakeWeight(selectedItem.weight * selectedItem.stackCount);
                    }
                    selectedItem.GetComponent<Image>().raycastTarget = true;
                    PlayItemAudio(selectedItem, AudioPlayMode.PickFromGrid);
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
                    
                    //itemInfoWindow.SetVisibility(false, selectedItem);
                    //selectedItem.SetOnCursorFlag(false);
                    selectedItem.isOnCursor = false;
                    itemInfoWindow.SetVisibility(false, selectedItem);
                    PlayItemAudio(selectedItem, AudioPlayMode.PickFromGrid);
                    selectedItem = null;
                    InsertItem(itemToInsert);
                    itemToInsert.GetComponent<Image>().raycastTarget = true;
                    selectedItemGrid = containerItemGrid;
                }
                else
                {
                    itemInfoWindow.SetVisibility(false, selectedItem);
                    // itemInfoWindow.Disable();
                    selectedItem.GetComponent<Image>().raycastTarget = true;
                    PlayItemAudio(selectedItem, AudioPlayMode.PickFromGrid);
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
            //selectedItem.SetOnCursorFlag(false);
            selectedItem.isOnCursor = false;
            itemInfoWindow.SetVisibility(false, selectedItem);
            //  itemInfoWindow.Disable();
            //itemInfoWindow.SetVisibility(false, selectedItem);
            PlayItemAudio(selectedItem, AudioPlayMode.PickFromGrid);
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
            itemToInsert.GetComponent<Image>().raycastTarget = true;
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






    #region PLACING ITEM ON GRID

    //TODO: Test
    private void PlaceItemOnGrid(Vector2Int tileGridPosition)
    {
        overlapItem = null;
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem); // TODO: Разобраться, как это работает
        if (complete == false)
        {
            return;
        }

        if (selectedItem.lastGrid.isPlayerInventory &&
            selectedItemGrid.isPlayerInventory == false)
        {
            TryUnbindItem();
            _playerCarryingWeight.TakeWeight(selectedItem.weight * selectedItem.stackCount);
        }
        else if (selectedItem.lastGrid.isPlayerInventory == false &&
            selectedItemGrid.isPlayerInventory)
        {
            _playerCarryingWeight.AddWeight(selectedItem.weight * selectedItem.stackCount);
        }

        длывардлфыраджолфыврадж
        selectedItem.lastGrid = selectedItemGrid;
        selectedItem.OnCursor(false);
        selectedItem.GetComponent<Image>().raycastTarget = true;

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
                if ((requiredItemsCount != 0) && overlapItem.isStackable && selectedItem.isStackable)
                {
                    if (selectedItem.stackCount > requiredItemsCount)
                    {
                        selectedItem.OnCursor(true);
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
                    SwapOverlappedItem(tileGridPosition);
                }
            }
            else
            {
                SwapOverlappedItem(tileGridPosition);
            }
        }
    }

    private void PickItemFromGrid(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickFromGrid(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            selectedItem.OnCursor(true);
            selectedItem.lastGrid = selectedItemGrid;
            selectedItemGrid.testList.Remove(selectedItem);
            itemRectTransform = selectedItem.GetComponent<RectTransform>();
            itemRectTransform.SetAsLastSibling();
        }
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

    private void SwapOverlappedItem(Vector2Int tileGridPosition)
    {
        selectedItemGrid.CleanGridReference(overlapItem);
        selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
        selectedItemGrid.testList.Add(selectedItem);
        selectedItemGrid.testList.Remove(overlapItem);
        overlapItem.OnCursor(true);
        selectedItem = overlapItem;
        overlapItem = null;
        itemRectTransform = selectedItem.GetComponent<RectTransform>();
        itemRectTransform.SetAsLastSibling();
    }

    #endregion

    private void PlayItemAudio(SCRIPT_InventoryItem item, AudioPlayMode audioMode)
    {
        if (inventoryAudioSource == null)
        {
            return;
        }
        
        switch(audioMode)
        {
            case AudioPlayMode.PickFromGround:
                if (item.pickFromGroundAudio != null)
                {
                    inventoryAudioSource.PlayOneShot(item.pickFromGroundAudio);
                }
                break;

            case AudioPlayMode.PickFromGrid:
                if (item.pickFromGridAudio != null)
                {
                    inventoryAudioSource.PlayOneShot(item.pickFromGridAudio);
                }
                break;

            case AudioPlayMode.PlaceOnGrid:
                if (item.placeOnGridAudio != null)
                {
                    inventoryAudioSource.PlayOneShot(item.placeOnGridAudio);
                }
                break;

            case AudioPlayMode.Use:
                if (item.useItemAudio != null)
                {
                    inventoryAudioSource.PlayOneShot(item.useItemAudio);
                }
                break;
        }
    }




    #region INVENTORY GRAPHICS AND UI

    // Initialize size, in-hierarchy position and some data for the created sprite
    public void CreateItemForUi(SCRIPT_InventoryItem item)
    {
        SCRIPT_InventoryItem inventoryItem = Instantiate(item);
        selectedItem = inventoryItem;
        itemRectTransform = inventoryItem.GetComponent<RectTransform>();
        itemRectTransform.SetParent(canvasTransform);
        itemRectTransform.SetAsLastSibling();
        inventoryItem.Init(inventoryItem.itemData);
    }

    internal void HandleMoreItemInfoVisibility()
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

    internal void RotateItem()
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

    private void ItemIconDrag()
    {
        if (selectedItem == null)
        {
            return;
        }

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

    Vector2Int oldPosition;
    SCRIPT_InventoryItem itemToHighlight;
    private void HandleItemHighlight()
    {
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

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

    #endregion

    // Cache info about an item under the cursor
    private SCRIPT_InventoryItem TryReceiveItem()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        SCRIPT_InventoryItem itemToBind = selectedItemGrid.PickReference(tileGridPosition.x, tileGridPosition.y);
        return itemToBind;
    }


    // TODO: Add logic for equip weapon when binded
    public static Action OnRebindWeapon;
    internal void TryBindItem(BindSlot bindSlot)
        {
        if (selectedItem != null && selectedItem.isOnCursor)
        {
            Debug.Log("<color=orange>Can't bind. There is item on the cursor.</color>");
            return;
        }

        SCRIPT_InventoryItem itemToBind = TryReceiveItem();

        if (itemToBind == null)
        {
            Debug.Log("<color=orange>Nothing to bind.</color>");
            return;
        }

        if (itemToBind.isEquippable == false)
        {
            Debug.Log("<color=orange>Item is not equipable.</color>");
            return;
        }

        if (itemToBind.permittedSlots.Contains(bindSlot) == false)
        {
            Debug.Log("<color=orange>Item can't be equipped at chosen slot.</color>");
            return;
        }

        // Trying to bind item
        if (itemToBind.isBinded)
        {
            // If binding to slot where this item is already binded, then do nothing
            if (itemToBind.bindedSlot == bindSlot)
            {
                Debug.Log("<color=orange>Already binded at chosen slot.</color>");
                return;
            }
            else
            {
                UnbindItem(itemToBind);

                
            }
        }

        // TODO: Here need to write check functions:
        // - check if item's already binded on this slot
        // - check if item's already binded at some other slot
        // - check if something else's already binded on this slot

        _bindSlots[bindSlot].item = itemToBind;
        itemToBind.BindKey(); // TODO: There is also need to add special icon that will appear in ui slots

        // TODO: It's very important for pivot to have the same name as a weapon.
         
        Transform pivot = _bindSlots[bindSlot].pivots.Single(pivot => pivot.name.Contains(itemToBind.name));
        string slotName = _bindSlots[bindSlot].name;
        itemToBind.bindedSlot = bindSlot;

        Debug.Log($"<color=yellow>Prefered pivot offset is {pivot}.</color>");

        _bindSlots[bindSlot].item.GetComponent<IEquipable>().EquipModel(pivot, slotName);

        Debug.Log($"<color=green>Binded on {_bindSlots[bindSlot].name}.</color>");
        
        itemToBind = null;
    }

    // TODO: Test
    internal void TryUnbindItem()
    {
        if (selectedItem != null && selectedItem.isOnCursor)
        {
            Debug.Log("<color=orange>Can't unbind. There is item on the cursor.</color>");
            return;
        }

        SCRIPT_InventoryItem itemToBind = TryReceiveItem();
        if (itemToBind == null)
        {
            Debug.Log("<color=orange>Nothing to unbind.</color>");
        }

        if (itemToBind.isBinded)
        {
            UnbindItem(itemToBind);
        }
    }

    private void UnbindItem(SCRIPT_InventoryItem itemToBind)
    {
        itemToBind.UnbindKey();
        if (itemToBind.isEquippable)
        {
            _bindSlots[itemToBind.bindedSlot].item.GetComponent<IEquipable>().UnequipModel();
        }

        if (_bindSlots[itemToBind.bindedSlot].item != null)
        {
            //UnbindItem(_bindSlots[bindSlot].item);
            _bindSlots[itemToBind.bindedSlot].item = null;
            OnRebindWeapon?.Invoke();
        }
    }


    internal void TryUseBindedItem(BindSlot bindSlot)
    {
        if (_bindSlots[bindSlot] != null)
        {
            _bindSlots[bindSlot].item.GetComponent<SCRIPT_IItem>().Use();
        }
    }

    #region CONTAINER LOGIC

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

    // Used to fill container when first initializing it.
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

    // used to fill already initialized container
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

    #endregion
}