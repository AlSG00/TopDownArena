using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCRIPT_InventoryItem : MonoBehaviour
{
    public string name;
    public SCRIPT_ItemData itemData;
    public GameObject prefab;
    public AudioClip useItemAudio;
    public AudioSource useItemAudioSource;
    public bool isDropping;
    public float weight;
    public bool isStackable = false;
    public int stackCount = 0;
    public int maxStackCount = 0;

    public TextMeshProUGUI stackCounter;

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

    private void Start()
    {
        useItemAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
    }

    internal void Set(SCRIPT_ItemData itemData)
    {
        this.itemData = itemData;
        name = itemData.name;
        GetComponent<Image>().sprite = itemData.icon;

        Vector2 size = new Vector2();
        size.x = itemData.width * SCRIPT_ItemGrid._tileSizeWidth;
        size.y = itemData.height * SCRIPT_ItemGrid._tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    internal void Rotated()
    {
        if (Height == 1 && Width == 1)
        {
            Debug.Log("Not rotatable");
            return;
        }
        isRotated = !isRotated;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, isRotated == true ? 90f : 0f);
    }
}
