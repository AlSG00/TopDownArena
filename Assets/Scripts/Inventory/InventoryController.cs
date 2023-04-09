using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("References")]
    public SCRIPT_ItemGrid inventoryGrid;
    [HideInInspector] public SCRIPT_ItemGrid selectedItemGrid;
    [HideInInspector] public RectTransform selectedItemGridRect;
    [SerializeField] private Player_Movement _playerMovement;
    [SerializeField] private SCRIPT_PlayerCarryingWeight _playerCarryingWeight;
    [SerializeField] private SCRIPT_InventoryHighlight inventoryHighlight;
    public SCRIPT_InventoryItem pickedInventoryItem;

    public SCRIPT_InventoryItem selectedItem; // выбранный предмет, который уже висит на курсоре

    SCRIPT_InventoryItem overlapItem;
    RectTransform itemRectTranform;

    private bool isHoldingButton = false;
    private bool isCheckingInventory = false;
    public bool isHighlightingStateIcons = false;

    private float buttonHoldTime = 0f;
    [SerializeField] private float timeToHold = 0.3f;

    public List<SCRIPT_InventoryItem> inventoryItemList = new List<SCRIPT_InventoryItem>();

    public delegate void OpenAction();
    public static event OpenAction OnInventoryOpened;

    public delegate void CloseAction();
    public static event CloseAction OnInventoryClosed;

    private void Awake()
    {
        SetInventoryVisibility(false);
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
            PickUpItem(tileGridPosition);
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

        Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().prefab, dropPoint.position, Quaternion.identity);
        inventoryItemList.Remove(pickedInventoryItem);
        _playerCarryingWeight.TakeWeight(pickedInventoryItem.Weight);
        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
    }

    private void RightMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }

        //TODO: По идее, здесь не сработает данное условие, переделать на обращение к InventoryItem
        if (selectedItem.GetComponent<SCRIPT_IItem>().isUsable == false)
        {
            PlaceItem(tileGridPosition);
            return;
        }

        if (selectedItem.useItemAudioSource != null &&
            selectedItem.useItemAudio != null)
        {
            selectedItem.useItemAudioSource.PlayOneShot(selectedItem.useItemAudio);
        }

        selectedItem.GetComponent<SCRIPT_IItem>().Use();

        if (selectedItemGrid != inventoryGrid)
        {
            itemContainer.storedItemList.Remove(pickedItem);
        }
        else
        {
            inventoryItemList.Remove(pickedInventoryItem);
            _playerCarryingWeight.TakeWeight(pickedInventoryItem.Weight);
        }

        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
    }

    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        if (pickedInventoryItem != null)
        {
            InventoryItem item = inventoryItemList.Find(x =>
            x.PositionOnGrid.x == selectedItem.onGridPositionX &&
            x.PositionOnGrid.y == selectedItem.onGridPositionY
            );

            if (item.isRotatable)
            {
                item.IsRotated = !item.IsRotated;
            }
        }
        else if (pickedItem != null)
        {
            SCRIPT_ItemContainer.StoredItem item = itemContainer.storedItemList.Find(x =>
            x.positionOnGrid.x == selectedItem.onGridPositionX &&
            x.positionOnGrid.y == selectedItem.onGridPositionY
            );

            if (item.isRotatable)
            {
                item.isRotated = !item.isRotated;
            }
        }

        selectedItem.Rotated();
    }

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
                selectedItem.itemData.width,
                selectedItem.itemData.height)
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
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

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

            if (itemContainer != null)
            {
                itemContainer.HandleContainerGrid(false);
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

        Vector2Int lastPosition = new Vector2Int(selectedItem.onGridPositionX, selectedItem.onGridPositionY);

        if (pickedItem != null)
        {
            selectedItemGrid = itemContainer.containerGrid;
        }
        else if (pickedInventoryItem != null)
        {
            selectedItemGrid = inventoryGrid;
        }
        PlaceItemOnGrid(lastPosition);
    }

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
                itemRectTranform = selectedItem.GetComponent<RectTransform>();
                itemRectTranform.SetAsLastSibling();
            }

            HandleLists(tileGridPosition);
            pickedItem = null;
            pickedInventoryItem = null;
        }
    }

    // TODO: переименовать метод.
    // Определяет, добавить новый предмет в сетку инвентаря игрока или контейнера
    private void HandleLists(Vector2Int tileGridPosition)
    {
        if (selectedItemGrid != inventoryGrid)
        {
            if (pickedItem == null)
            {
                pickedItem = new SCRIPT_ItemContainer.StoredItem(
                    pickedInventoryItem.Item,
                    0,
                    0,
                    pickedInventoryItem.IsRotated,
                    pickedInventoryItem.Weight,
                    pickedInventoryItem.Width,
                    pickedInventoryItem.Height
                    );
                itemContainer.storedItemList.Add(pickedItem);
                //pickedItem.item = pickedInventoryItem.item;
                //pickedItem.isRotated = pickedInventoryItem.isRotated;
                inventoryItemList.Remove(pickedInventoryItem);
                _playerCarryingWeight.TakeWeight(pickedInventoryItem.Weight);
            }
            pickedItem.positionOnGrid.x = tileGridPosition.x;
            pickedItem.positionOnGrid.y = tileGridPosition.y;

        }
        else
        {
            if (pickedInventoryItem == null)
            {
                Debug.Log(pickedItem);
                pickedInventoryItem = new InventoryItem(
                    pickedItem.item,
                    0,
                    0,
                    pickedItem.isRotated,
                    pickedItem.weight,
                    pickedItem.Width,
                    pickedItem.Height
                    );

                inventoryItemList.Add(pickedInventoryItem);
                //pickedInventoryItem.item = pickedItem.item;
                //pickedInventoryItem.isRotated = pickedItem.isRotated;
                itemContainer.storedItemList.Remove(pickedItem);
                _playerCarryingWeight.AddWeight(pickedItem.weight);
            }

            pickedInventoryItem.PositionOnGrid.x = tileGridPosition.x;
            pickedInventoryItem.PositionOnGrid.y = tileGridPosition.y;
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            if (selectedItemGridRect.rect.Overlaps(itemRectTranform.rect) == false)
            {
                itemRectTranform.SetParent(selectedItemGridRect.parent);
            }
            else
            {
                itemRectTranform.SetParent(selectedItemGridRect);
            }

            itemRectTranform.SetAsLastSibling();
            itemRectTranform.position = Input.mousePosition;
        }
    }
}
