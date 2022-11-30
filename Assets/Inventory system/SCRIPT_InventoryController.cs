using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_InventoryController : MonoBehaviour
{
    [HideInInspector] public SCRIPT_ItemGrid selectedItemGrid;

    SCRIPT_InventoryItem selectedItem;
    SCRIPT_InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<SCRIPT_ItemData> items;
    [SerializeField] GameObject itemPreafab;
    [SerializeField] Transform canvasTransform;

    SCRIPT_InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight = GetComponent<SCRIPT_InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (selectedItemGrid == null)
        {
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    SCRIPT_InventoryItem itemToHighlight;
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {
            inventoryHighlight.SetSize(itemToHighlight);
            inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                iuQWLGDI

                    1:28:11
            }
        }
        else
        {

        }
    }

    private void CreateRandomItem()
    {
        SCRIPT_InventoryItem inventoryItem = Instantiate(itemPreafab).GetComponent<SCRIPT_InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        Debug.Log(selectedItemID);
        inventoryItem.Set(items[selectedItemID]);
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
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
            position.x -= (selectedItem.itemData.width - 1) * SCRIPT_ItemGrid._tileSizeWidth / 2;
            position.y += (selectedItem.itemData.height - 1) * SCRIPT_ItemGrid._tileSizeHeight / 2;
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
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }
}
