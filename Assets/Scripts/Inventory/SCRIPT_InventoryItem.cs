using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCRIPT_InventoryItem : MonoBehaviour
{
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

    // Временно напишу шпаргалки

    public string name; // метка имени
    public SCRIPT_ItemData itemData; // общая инфа (размер в инвентаре, имя, иконка)
    public GameObject objectPrefab; // 3д-объект, который заспавнится, когда предмет будет выброшен
    public AudioClip useItemAudio; // TODO: Переименовать. Звук, воспроизводимый, когда предмет используется
    public AudioSource useItemAudioSource; // источник звука использования предмета
    public bool isDropping; //TODO: Возможно могу избавиться. Флаг, чтобы при выкидывании не спавнилось несколько предметов сразу
    public float weight; // вес предмета, добавляется игроку
    public bool isStackable = false; // можно ли стакать предмет
    public bool isRotatable = false; // можно ли вращать предмет 
    public int stackCount = 0; // текущий размер стака
    public int maxStackCount = 0; // максимальный размер стака
    public bool isRotated = false; // вращался ли предмет
    public bool isUsable = true; // можно ли использовать предмет
    public TextMeshProUGUI stackCounter; // UI счетчик предметов в стаке
    public int onGridPositionX; // TODO: Наверное, от этих двух int'ов можно избавиться, раз уж тут есть PositionOnGrid
    public int onGridPositionY;
    public Vector2 positionOnGrid; // позиция предмета на сетке инвентаря

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
