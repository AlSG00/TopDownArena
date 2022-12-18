using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_InventoryController : MonoBehaviour
{
    public SCRIPT_ItemGrid inventoryGrid;
    public SCRIPT_ItemGrid selectedItemGrid;
    //public SCRIPT_ItemGrid selectedContainer;
    public SCRIPT_ItemContainer itemContainer;
    public SCRIPT_InventoryItem selectedItem;
    SCRIPT_InventoryItem overlapItem;
    RectTransform rectTransform;
    public RectTransform gridRect;
    public Transform dropPoint;

    [SerializeField] List<GameObject> items;
     private GameObject _itemPrefab;
    [SerializeField] Transform canvasTransform;

    SCRIPT_InventoryHighlight inventoryHighlight;

    public bool isCheckingInventory = false;
    public List<InventoryItem> inventoryItemList = new List<InventoryItem>();

    public class InventoryItem
    {
        public GameObject item;
        public Vector2Int positionOnGrid;
        public bool isRotated;
    }

    private void Awake()
    {
        inventoryHighlight = GetComponent<SCRIPT_InventoryHighlight>();
        HandleInventory(false);
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

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightMouseButtonPress();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isCheckingInventory = !isCheckingInventory;
            HandleInventory(isCheckingInventory);
        }
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
            x.positionOnGrid.x == selectedItem.onGridPositionX &&
            x.positionOnGrid.y == selectedItem.onGridPositionY
            );

            item.isRotated = !item.isRotated;
        }
        else if (pickedItem != null)
        {
            SCRIPT_ItemContainer.StoredItem item = itemContainer.storedItemList.Find(x =>
            x.positionOnGrid.x == selectedItem.onGridPositionX &&
            x.positionOnGrid.y == selectedItem.onGridPositionY
            );

            item.isRotated = !item.isRotated;
        }

        selectedItem.Rotated();
    }

    public void InsertItemIntoInventory(GameObject item)
    {
        if (selectedItemGrid == null) 
        {
            Debug.Log("Grid is not selected");
            return;
        }

        CreateItem(item);
        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);

        InventoryItem itemToPick = new InventoryItem();
        itemToPick.item = item.GetComponent<SCRIPT_PickableObject>().inventoryPrefab;
        itemToPick.positionOnGrid.x = itemToInsert.onGridPositionX;
        itemToPick.positionOnGrid.y = itemToInsert.onGridPositionY;
        itemToPick.isRotated = itemToInsert.isRotated;
        inventoryItemList.Add(itemToPick);
        
    }

    //функция для вставки предметов в не инициализированный контейнер
    //предметы создаются заново
    //создаются экземпляры Storeditem и заполняются данными
    public void InsertItemIntoContainer(GameObject item)
    {
        CreateContainerItem(item);
        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;

        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (positionOnGrid == null)
        {
            return;
        }
        selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);

        SCRIPT_ItemContainer.StoredItem itemTostore = new SCRIPT_ItemContainer.StoredItem();
        itemTostore.item = item;
        itemTostore.positionOnGrid.x = positionOnGrid.Value.x;
        itemTostore.positionOnGrid.y = positionOnGrid.Value.y;
        itemContainer.storedItemList.Add(itemTostore);
    }

    //функция для вставки предметов в уже инициализированный контейнер
    //предметы создаются
    //данные об их положении считываются из списка у контейнера

    //Продумать, как и когда должна производиться запись в список контейнера

    //Один из вариантов:
    //После каждого перемещения обновляются данные
    //Если предмет переносится в инвентарь, то его надо удалять из списка в контейнере???
    public void InsertItemIntoInitializedContainer(SCRIPT_ItemContainer.StoredItem storedItem /*GameObject item*/)
    {
        CreateContainerItem(storedItem.item);
        
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

    public void CreateItem(GameObject item)
    {
        _itemPrefab = item.GetComponent<SCRIPT_PickableObject>().inventoryPrefab;
        SCRIPT_InventoryItem inventoryItem = Instantiate(_itemPrefab).GetComponent<SCRIPT_InventoryItem>();

        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();
        inventoryItem.Set(inventoryItem.itemData);
    }

    private void CreateContainerItem(GameObject item)
    {
        SCRIPT_InventoryItem inventoryItem = Instantiate(item).GetComponent<SCRIPT_InventoryItem>();

        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();
        inventoryItem.Set(inventoryItem.itemData);
    }

    private void InsertItem(SCRIPT_InventoryItem itemToInsert)
    {
        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
    
        if (positionOnGrid == null)
        {
            //itemToInsert.Rotated();
            //positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

            //if (positionOnGrid == null)
            //{
            //    return;
            //}
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


    Vector2Int oldPosition;
    SCRIPT_InventoryItem itemToHighlight;
    private void HandleHighlight()
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

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            if (!gridRect.rect.Overlaps(rectTransform.rect))
            {
                rectTransform.SetParent(gridRect.parent);
            }
            else
            {
                rectTransform.SetParent(gridRect);
            }

            rectTransform.SetAsLastSibling();
            rectTransform.position = Input.mousePosition;
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

    private void RightMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }

        if (selectedItem.GetComponent<SCRIPT_IItem>().isUsable == false)
        {
            PlaceItem(tileGridPosition);
            return;
        }

        selectedItem.GetComponent<SCRIPT_IItem>().Use();

        if (selectedItemGrid != inventoryGrid)
        {
            itemContainer.storedItemList.Remove(pickedItem);
        }
        else
        {
            inventoryItemList.Remove(pickedInventoryItem);
        }

        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
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
        //Debug.Log($"SelectedItem is {selectedItem}");

        Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().prefab, dropPoint.position, Quaternion.identity);
        inventoryItemList.Remove(pickedInventoryItem);
        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
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

    private void PlaceItem(Vector2Int tileGridPosition)
    {

        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }

            if (selectedItemGrid != inventoryGrid)
            {
                if (pickedItem == null)
                { 
                    pickedItem = new SCRIPT_ItemContainer.StoredItem();
                    itemContainer.storedItemList.Add(pickedItem);
                    pickedItem.item = pickedInventoryItem.item;
                    pickedItem.isRotated = pickedInventoryItem.isRotated;
                    inventoryItemList.Remove(pickedInventoryItem);
                }
                pickedItem.positionOnGrid.x = tileGridPosition.x;
                pickedItem.positionOnGrid.y = tileGridPosition.y;

            }
            else
            {
                if (pickedInventoryItem == null)
                {
                    pickedInventoryItem = new InventoryItem();
                    inventoryItemList.Add(pickedInventoryItem);
                    pickedInventoryItem.item = pickedItem.item;
                    pickedInventoryItem.isRotated = pickedItem.isRotated;
                    itemContainer.storedItemList.Remove(pickedItem);
                }

                pickedInventoryItem.positionOnGrid.x = tileGridPosition.x;
                pickedInventoryItem.positionOnGrid.y = tileGridPosition.y;

            }
            pickedItem = null;
            pickedInventoryItem = null;
        }
    }

    SCRIPT_ItemContainer.StoredItem pickedItem;
    InventoryItem pickedInventoryItem;
    Vector2Int previousPosition;
    SCRIPT_ItemGrid lastGrid;
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        pickedItem = null;
        pickedInventoryItem = null;
        if (selectedItem != null)
        {
            if (selectedItemGrid != inventoryGrid)
            {
                previousPosition.x = selectedItem.onGridPositionX;
                previousPosition.y = selectedItem.onGridPositionY;

                pickedItem = itemContainer.storedItemList.Find(x =>
                x.positionOnGrid.x == previousPosition.x &&
                x.positionOnGrid.y == previousPosition.y
                );

                //Debug.Log($"Picked {pickedItem.item.name} : {selectedItem.onGridPositionX} : {selectedItem.onGridPositionY}");
            }
            else
            {
                previousPosition.x = selectedItem.onGridPositionX;
                previousPosition.y = selectedItem.onGridPositionY;

                pickedInventoryItem = inventoryItemList.Find(x =>
                x.positionOnGrid.x == previousPosition.x &&
                x.positionOnGrid.y == previousPosition.y
                );

               //Debug.Log($"Picked {pickedInventoryItem.item.name} : {selectedItem.onGridPositionX} : {selectedItem.onGridPositionY}");
            }

            rectTransform = selectedItem.GetComponent<RectTransform>();
            rectTransform.SetAsLastSibling();
        }
    }

    public void HandleInventory(bool isCheckingInventory)
    {
        Vector2 position = new Vector2();
        RectTransform inventoryRect = inventoryGrid.GetComponent<RectTransform>();

        //Destroy(selectedItem);

        if (isCheckingInventory)
        {
            position.y = 630;
        }
        else
        {
            GetItemBack();
            position.y = 3000;

            if (itemContainer != null)
            {
                itemContainer.HandleContainerGrid(false);
            }
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
        PlaceItem(lastPosition);

        //if (pickedInventoryItem != null)
        //{
        //    inventoryGrid.PlaceItem(selectedItem, selectedItem.onGridPositionX, selectedItem.onGridPositionY);
        //    pickedInventoryItem = null;
        //}
        //else if (pickedItem != null)
        //{
        //    itemContainer.containerGrid.PlaceItem(selectedItem, selectedItem.onGridPositionX, selectedItem.onGridPositionY);
        //    pickedItem = null;
        //}
    }
    public void OpenInventory()
    {

    }

    public void CloseInventory()
    {

    }
}
