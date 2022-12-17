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

    //private List<GameObject> itemList;

    private void Awake()
    {
        inventoryHighlight = GetComponent<SCRIPT_InventoryHighlight>();
        //itemList = new List<GameObject>();
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

    }

    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
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
    }

    public void InsertItemIntoContainer(GameObject item)
    {

        CreateContainerItem(item);
        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);

        функция для вставки предметов в не инициализированный контейнер
        предметы создаются заново
        создаются экземпляры Storeditem и заполняются данными
    }

    public void InsertItemIntoInitializedContainer(GameObject item)
    {
        функция для вставки предметов в уже инициализированный контейнер
        предметы создаются
        данные об их положении считываются из списка у контейнера

        Продумать, как и когда должна производиться запись в список контейнера

        Один из вариантов:
        После каждого перемещения обновляются данные
        Если предмет переносится в инвентарь, то его надо удалять из списка в контейнере???
        
        //CreateContainerItem(item);
        //SCRIPT_InventoryItem itemToInsert = selectedItem;
        //selectedItemGrid.PlaceItem(itemToInsert, itemToInsert.onGridPositionX, itemToInsert.onGridPositionY);
        

        CreateContainerItem(item);
        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        //InsertItem(itemToInsert);

        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (positionOnGrid == null)
        {
            return;
        }
        selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);

        SCRIPT_ItemContainer.StoredItem itemTostore = new SCRIPT_ItemContainer.StoredItem();
        itemTostore.item = selectedItem;
        itemTostore.positionOnGrid.x = positionOnGrid.x;
        itemTostore.positionOnGrid.y = positionOnGrid.y;
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
        //_itemPrefab = null;
       // _itemPrefab = item.GetComponent<SCRIPT_PickableObject>().inventoryPrefab;
        SCRIPT_InventoryItem inventoryItem = Instantiate(item).GetComponent<SCRIPT_InventoryItem>();

        selectedItem = inventoryItem;
        Debug.Log($"Selected item is {selectedItem}");
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();
        inventoryItem.Set(inventoryItem.itemData);
    }

    private void InsertItem(SCRIPT_InventoryItem itemToInsert)
    {
        //Debug.Log("Finding space...");
        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
    
        if (positionOnGrid == null)
        {
            //Debug.Log("No space left in the inventory");
            //Destroy(itemToInsert.gameObject);
            // return false;
            return;
        }
        //Debug.Log("Placing item...");
        selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);
        //return true;
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
        selectedItem.GetComponent<SCRIPT_IItem>().Use();
        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
    }

    private void DropItem()
    {
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
        Debug.Log($"SelectedItem is {selectedItem}");

        Instantiate(selectedItem.GetComponent<SCRIPT_InventoryItem>().prefab, dropPoint.position, Quaternion.identity);
        Destroy(selectedItem.gameObject);
        inventoryHighlight.Show(false);
    }

    //private void ClearGrid()
    //{
    //    if (selectedItemGrid == null)
    //    {
    //        return;
    //    }

    //    for (int i = 0; i < selectedItemGrid._gridSizeWidth; i++)
    //    {
    //        for (int j = 0; j < selectedItemGrid._gridSizeHeight; j++)
    //        {
    //            selectedItem = selectedItemGrid.PickUpItem(i, j);
    //            if (selectedItem != null)
    //            {
    //                Destroy(selectedItem.gameObject);
    //            }
    //        }
    //    }

    //    //selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
    //    //if (selectedItem != null)
    //    //{
    //    //    rectTransform = selectedItem.GetComponent<RectTransform>();
    //    //    rectTransform.SetAsLastSibling();
    //    //}
    //}

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
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
            rectTransform.SetAsLastSibling();
        }
    }
}
