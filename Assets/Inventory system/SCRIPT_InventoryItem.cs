using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_InventoryItem : MonoBehaviour
{
    public SCRIPT_ItemData itemData;
    public GameObject prefab;
    public bool isDropping;
    public float weight;

    public int Height
    {
        get
        {
            if (isRotated == false)
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }

    public int Width
    {
        get
        {
            if (isRotated == false)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool isRotated = false;
    public bool isUsable = true;

    //public void Start()
    //{
    //    isRotated = false;
    //}

    internal void Set(SCRIPT_ItemData itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * SCRIPT_ItemGrid._tileSizeWidth;
        size.y = itemData.height * SCRIPT_ItemGrid._tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    internal void Rotated()
    {
        isRotated = !isRotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, isRotated == true ? 90f : 0f);
    }
}
