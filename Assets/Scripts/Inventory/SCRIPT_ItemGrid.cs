using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SCRIPT_ItemGrid : MonoBehaviour
{
    // Хранение данных об инвентаре
    // TODO: Сделать динамически???
    public const float _tileSizeWidth = 64;
    public const float _tileSizeHeight = 64;

    public SCRIPT_InventoryItem[,] inventoryItemSlot;
    public InventoryController inventory;

    public bool isPlayerInventory;

    //public List<SCRIPT_InventoryItem> testItemList = new List<SCRIPT_InventoryItem>();
    // public ItemCollection itemCollection;
    public List<SCRIPT_InventoryItem> testList = new List<SCRIPT_InventoryItem>(); // 


    RectTransform rectTransform;

    public int _gridSizeWidth = 5;
    public int _gridSizeHeight = 5;

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition;

    private void OnEnable()
    {
        InventoryController.OnInventoryOpened += SetVisibility;
    }
    private void OnDisable()
    {
        InventoryController.OnInventoryOpened -= SetVisibility;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Initialize(_gridSizeWidth, _gridSizeHeight);
        SetVisibility(false/*, false*/);
       // itemCollection.itemList = new List<SCRIPT_InventoryItem>();
    }

    internal SCRIPT_InventoryItem PickUpItem(int x, int y)
    {
        // TODO: добавить сюда логику стаков
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
                //inventoryItemSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
                inventoryItemSlot[item.positionOnGrid.x + i, item.positionOnGrid.y + j] = null;
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

    public void Initialize(int width, int height)
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

    public bool returnRotated = false;
    public Vector2Int? FindSpaceForObject(SCRIPT_InventoryItem itemToInsert)
    {
        /*
        // TODO: сюда сделать, чтобы предметы стакались. Может сделать именную метку?
        //if (itemToInsert.isStackable == true)
        //{
            //SCRIPT_InventoryItem itemTostack = inventory.inventoryItemList.Find(item => item.name == itemToInsert.name);
            //if (itemTostack != null &&

            //    itemTostack.stackCount < itemTostack.maxStackCount)
            //{
            //    Написать метод, в котором будет увеличиваться стак и обновляться UI - счетчик;
            //    return;
            //}

            // TODO: продебажить, что здесь будет возвращено, если не найдется ни одного стакаемого объекта
            //inventory.stackableItemsTemporaryList = inventory.inventoryItemList.FindAll(
            //    item => item.name == itemToInsert.name
            //    );


            //if (inventory.stackableItemsTemporaryList != null)
            //{
            //    foreach (int item in inventory.stackableItemsTemporaryList)
            //    {
            //        if (inventory.stackableItemsTemporaryList[item].stackCount < inventory.stackableItemsTemporaryList[item].maxStackCount)
            //        {
            //                Написать метод, в котором будет увеличиваться стак и обновляться UI - счетчик;
            //                return;
            //        }
            //    }
            //}

            // TODO: Прописать следующую логику:
            // если мы подбираем целый стак (например, коробку патронов)
            // то мы набиваем доступные стаки
            // а если стаки кончились
            // то занимаем новую ячейку
            // а если и ячейки кончились, то оставляем остаток стака лежать на земле

            //if (inventory.stackableItemsTemporaryList != null)
            //{
            //int toStack = itemToInsert.stackCount;
            //    for (int i = 0; i < inventoryItemSlot.Length; i++)
            //    {
            //        for (int j = 0; j < inventoryItemSlot.Length; j++)
            //        {
            //            if (inventoryItemSlot[i, j] != null &&
            //                inventoryItemSlot[i, j].isStackable &&
            //                inventoryItemSlot[i, j].stackCount < inventoryItemSlot[i, j].maxStackCount)
            //            {
                            
            //                Написать метод, в котором будет увеличиваться стак и обновляться UI - счетчик;

            //                Здесь прописать какую-нибудь логику, чтобы проверять, добавился ли стак полностью
            //                учесть, что подбираемая пачка не может быть больше, чем максимальный размер стака. Логично, но все же
            //                return;
            //            }
            //        }
            //    }
            //}
        //}
        */

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

        returnRotated = true;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (CheckAvailableSpace(j, i, itemToInsert.Height, itemToInsert.Width))
                {
                    return new Vector2Int(j, i);
                }
            }
        }

        returnRotated = false;
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
        testList.Add(inventoryItem);

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

        //inventoryItem.onGridPositionX = positionX;
        //inventoryItem.onGridPositionY = positionY;

        inventoryItem.positionOnGrid.x = positionX;
        inventoryItem.positionOnGrid.y = positionY;
        Vector2 position = CalculatePositionOnGrid(inventoryItem, positionX, positionY);

        rectTransform.localPosition = position;
        //testList.Add(inventoryItem);
        inventoryItem.UpdateCounter();
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

    //public void ClearGrid()
    //{
    //    int childrenCount = transform.childCount;
    //    for (int i = childrenCount - 1; i >= 0; i--)
    //    {
    //        //Destroy(transform.GetChild(i).gameObject);
    //        DestroyImmediate(transform.GetChild(i).gameObject);
    //    }
    //}

    public void ClearGrid()
    {
        GameObject highlighter = GameObject.Find("Highlighter");
        
        int childrenCount = transform.childCount;
        for (int i = childrenCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).name != "Highlighter")
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        inventoryItemSlot = null;
    }

    public void SetVisibility(bool isVisible/*, bool openingContainer*/)
    {
        RectTransform inventoryRect = rectTransform.GetComponent<RectTransform>();

        Vector2 position = new Vector2();
        if (isPlayerInventory == false && isVisible)
        //    openingContainer == false)
        {
            position.y = 3000;
            inventoryRect.position = position;
            return;
        }

        if (isVisible == true)
        {
            position.y = 630;
        }
        else
        {
            position.y = 3000;
        }

        position.x = inventoryRect.position.x;
        inventoryRect.position = position;
    }

    public void SetContainerGridVisibility(bool isVisible/*, bool openingContainer*/)
    {
        RectTransform inventoryRect = rectTransform.GetComponent<RectTransform>();
        Vector2 position = new Vector2();

        if (isVisible == true)
        {
            position.y = 630;
        }
        else
        {
            position.y = 3000;
            //  alreadyInteracting = false;
        }

        position.x = inventoryRect.position.x;
        inventoryRect.position = position;
    }
}
