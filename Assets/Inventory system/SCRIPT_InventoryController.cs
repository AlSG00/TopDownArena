using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_InventoryController : MonoBehaviour
{
    public SCRIPT_ItemGrid inventoryGrid;
    public SCRIPT_ItemGrid selectedItemGrid;
    public SCRIPT_ItemGrid selectedContainer;

    SCRIPT_InventoryItem selectedItem;
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

        if (Input.GetMouseButtonDown(1))
        {
            RightMouseButtonPress();
        }

        if (Input.GetKeyDown(KeyCode.Q))
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
            // return false;
            return;
        }

        CreateItem(item);
        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
        //if (InsertItem(itemToInsert))
        //{
        //    return true;
        //}

        //return false;
    }

    private void InsertItem(SCRIPT_InventoryItem itemToInsert)
    {
        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
    
        if (positionOnGrid == null)
        {
            //Debug.Log("No space left in the inventory");
            //Destroy(itemToInsert.gameObject);
            // return false;
            return;
        }

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
