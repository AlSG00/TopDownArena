using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ItemGrid : MonoBehaviour
{
    public const float _tileSizeWidth = 32;
    public const float _tileSizeHeight = 32;

    SCRIPT_InventoryItem[,] inventoryItemSlot;

    RectTransform rectTransform;

    [SerializeField] private int _gridSizeWidth = 5;
    [SerializeField] private int _gridSizeHeight = 5;

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(_gridSizeWidth, _gridSizeHeight);
    }

    internal SCRIPT_InventoryItem PickUpItem(int x, int y)
    {
        SCRIPT_InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
        {
            return null;
        }

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(SCRIPT_InventoryItem item)
    {
        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                inventoryItemSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
            }
        }
    }

    internal SCRIPT_InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new SCRIPT_InventoryItem[width, height];
        Vector2 size = new Vector2(width * _tileSizeWidth, height * _tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;

        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        // Вычисление положения ячеек на сетке
        tileGridPosition.x = (int)(positionOnTheGrid.x / _tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / _tileSizeHeight);

        return tileGridPosition;
    }

    public bool PlaceItem(SCRIPT_InventoryItem inventoryItem, int positionX, int positionY, ref SCRIPT_InventoryItem overlapItem)
    {
        if (BoundaryCheck(positionX, positionY, inventoryItem.itemData.width, inventoryItem.itemData.height) == false)
        {
            return false;
        }

        if (OverlapCheck(positionX, positionY, inventoryItem.itemData.width, inventoryItem.itemData.height, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int i = 0; i < inventoryItem.itemData.width; i++)
        {
            for (int j = 0; j < inventoryItem.itemData.height; j++)
            {
                inventoryItemSlot[positionX + i, positionY + j] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = positionX;
        inventoryItem.onGridPositionY = positionY;
        Vector2 position = CalculatePositionOnGrid(inventoryItem, positionX, positionY);

        rectTransform.localPosition = position;

        return true;
    }

    public Vector2 CalculatePositionOnGrid(SCRIPT_InventoryItem inventoryItem, int positionX, int positionY)
    {
        Vector2 position = new Vector2();
        position.x = positionX * _tileSizeWidth + _tileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(positionY * _tileSizeHeight + _tileSizeHeight * inventoryItem.itemData.height / 2);
        return position;
    }

    private bool OverlapCheck(int positionX, int positionY, int width, int height, ref SCRIPT_InventoryItem overlapItem)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (inventoryItemSlot[positionX + i, positionY + j] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[positionX + i, positionY + j];

                    }
                    else
                    {
                        if (overlapItem != inventoryItemSlot[positionX + i, positionY + j])
                        {
                            return false;
                        }
                    }
                } 
            }
        }

        return true;
    }

    bool PositionCheck(int positionX, int positionY)
    {
        if (positionX < 0 || positionY < 0)
        {
            return false;
        }

        if (positionX >= _gridSizeWidth || positionY >= _gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    bool BoundaryCheck(int positionX, int positionY, int width, int height)
    {
        if (PositionCheck(positionX, positionY) == false)
        {
            return false;
        }

        positionX += width - 1;
        positionY += height - 1;

        if (PositionCheck(positionX, positionY) == false)
        {
            return false;
        }

        return true;
    }
}
