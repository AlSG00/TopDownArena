using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_InventoryController : MonoBehaviour
{
    /*[HideInInspector]*/ public SCRIPT_ItemGrid selectedItemGrid;

    //public SCRIPT_ItemGrid SelectedItemGrid
    //{
    //    get => selectedItemGrid;
    //    set
    //    {
    //        selectedItemGrid = value;
    //        inventoryHighlight.SetParent(value);
    //    }
    //}

    SCRIPT_InventoryItem selectedItem;
    SCRIPT_InventoryItem overlapItem;
    RectTransform rectTransform;
    public RectTransform gridRect;

    [SerializeField] List<SCRIPT_ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    SCRIPT_InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight = GetComponent<SCRIPT_InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    CreateRandomItem();
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    InsertRandomItem();
        //}

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

        Debug.Log("Creating item...");
        CreateItem(item);
        SCRIPT_InventoryItem itemToInsert = selectedItem;
        selectedItem = null;

        Debug.Log("Inserting item...");
        InsertItem(itemToInsert);

        Debug.Log("Item inserted...");
    }

    private void InsertItem(SCRIPT_InventoryItem itemToInsert)
    {
        Vector2Int? positionOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
    
        if (positionOnGrid == null)
        {
            return;
        }

        selectedItemGrid.PlaceItem(itemToInsert, positionOnGrid.Value.x, positionOnGrid.Value.y);
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
                selectedItem.Width,
                selectedItem.Height)
                );

            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);

        }
    }

    //private void CreateRandomItem(GameObject item)
    //{
    //    SCRIPT_InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<SCRIPT_InventoryItem>();
    //    selectedItem = inventoryItem;

    //    rectTransform = inventoryItem.GetComponent<RectTransform>();
    //    rectTransform.SetParent(canvasTransform);
    //    rectTransform.SetAsLastSibling();

    //    inventoryItem.Set(items[selectedItemID]);
    //}

    private void CreateItem(GameObject item)
    {
        SCRIPT_InventoryItem inventoryItem = item.GetComponent<SCRIPT_InventoryItem>();
        selectedItem = inventoryItem;

        Debug.Log("Selected item " + selectedItem);

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
