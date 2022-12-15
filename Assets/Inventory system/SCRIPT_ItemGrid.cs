using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ItemGrid : MonoBehaviour
{
    // Хранение данных об инвентаре

    public const float _tileSizeWidth = 64;
    public const float _tileSizeHeight = 64;

    SCRIPT_InventoryItem[,] inventoryItemSlot;

    RectTransform rectTransform;

    public int _gridSizeWidth = 5;
    public int _gridSizeHeight = 5;

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

    // Чистим инвентарь от ссылок на предмет, когда пертаскиваем или выкидываем предмет
    private void CleanGridReference(SCRIPT_InventoryItem item)
    {
        for (int i = 0; i < item.Width; i++)
        {
            for (int j = 0; j < item.Height; j++)
            {
                inventoryItemSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
            }
        }
    }

    internal SCRIPT_InventoryItem GetItem(int x, int y)
    {
        if (x >= 0 &&
            y >= 0 &&
            x < _gridSizeWidth &&
            y < _gridSizeHeight)
        {
            return inventoryItemSlot[x, y];
        }

        return null;
    }

    public void Init(int width, int height)
    {
        inventoryItemSlot = new SCRIPT_InventoryItem[width, height];
        Vector2 size = new Vector2(width * _tileSizeWidth, height * _tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;
       // Debug.Log($"Position on the grid: {positionOnTheGrid.x} : {positionOnTheGrid.y}");


        // Вычисление положения ячеек на сетке
        tileGridPosition.x = (int)(positionOnTheGrid.x / _tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / _tileSizeHeight);

        if (positionOnTheGrid.x < 0)
        {
            tileGridPosition.x -= 1;
        }

        if (positionOnTheGrid.y < 0)
        {
            tileGridPosition.y -= 1;
        }

       // Debug.Log($"Position: {tileGridPosition.x} : {tileGridPosition.y}");
        return tileGridPosition;
    }

    //bool returnRotated;
    public Vector2Int? FindSpaceForObject(SCRIPT_InventoryItem itemToInsert)
    {
       // returnRotated = false;
       
        int height = _gridSizeHeight - itemToInsert.Height + 1;
        int width = _gridSizeWidth - itemToInsert.Width + 1;

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (CheckAvailableSpace(i, j, itemToInsert.Width, itemToInsert.Height))
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        // Пытаемся найти место повторно, но с повернутым предметом
        //for (int j = 0; j < height; j++)
        //{
        //    for (int i = 0; i < width; i++)
        //    {
        //        if (CheckAvailableSpace(i, j, itemToInsert.Height, itemToInsert.Width))
        //        {
        //            returnRotated = true;
        //            return new Vector2Int(i, j);
        //        }
        //    }
        //}

        return null;
    }

    public bool PlaceItem(SCRIPT_InventoryItem inventoryItem, int positionX, int positionY, ref SCRIPT_InventoryItem overlapItem)
    {
        if (BoundaryCheck(positionX, positionY, inventoryItem.Width, inventoryItem.Height) == false)
        {
            return false;
        }

        if (OverlapCheck(positionX, positionY, inventoryItem.Width, inventoryItem.Height, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, positionX, positionY);

        return true;
    }

    public void PlaceItem(SCRIPT_InventoryItem inventoryItem, int positionX, int positionY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int i = 0; i < inventoryItem.Width; i++)
        {
            for (int j = 0; j < inventoryItem.Height; j++)
            {
                inventoryItemSlot[positionX + i, positionY + j] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = positionX;
        inventoryItem.onGridPositionY = positionY;
        Vector2 position = CalculatePositionOnGrid(inventoryItem, positionX, positionY);

        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePositionOnGrid(SCRIPT_InventoryItem inventoryItem, int positionX, int positionY)
    {
        Vector2 position = new Vector2();
        position.x = positionX * _tileSizeWidth + _tileSizeWidth * inventoryItem.Width / 2;
        position.y = -(positionY * _tileSizeHeight + _tileSizeHeight * inventoryItem.Height / 2);
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

    private bool CheckAvailableSpace(int positionX, int positionY, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (inventoryItemSlot[positionX + i, positionY + j] != null)
                {
                    return false;
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

    public bool BoundaryCheck(int positionX, int positionY, int width, int height)
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

    public void ClearGrid()
    {

    }
}
