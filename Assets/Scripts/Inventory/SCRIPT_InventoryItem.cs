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

    public string name; // метка имени
    [SerializeField] public SCRIPT_ItemData itemData; // общая инфа (размер в инвентаре, имя, иконка)
    public GameObject objectPrefab; // 3д-объект, который заспавнится, когда предмет будет выброшен

    [Header("Properties")]
    [Range(0, 999)] public float weight; // вес предмета, добавляется игроку
    public bool isRotatable = false; // можно ли вращать предмет 
    public bool isUsable = true; // можно ли использовать предмет

    [Header("Stacking properties")]
    public bool isStackable = false; // можно ли стакать предмет
    [Range(0, 999)] public int stackCount = 0; // текущий размер стака
    [Range(0, 999)] public int maxStackCount = 0; // максимальный размер стака

    [Header("Audio")]
    public AudioClip useItemAudio; // TODO: Переименовать. Звук, воспроизводимый, когда предмет используется
    public AudioSource useItemAudioSource; // источник звука использования предмета

    [Header("UI")]
    public TextMeshProUGUI stackCounter; // UI счетчик предметов в стаке
    
    [HideInInspector] public bool isDropping; //TODO: Возможно могу избавиться. Флаг, чтобы при выкидывании не спавнилось несколько предметов сразу
    [HideInInspector] public bool isRotated = false; // вращался ли предмет
    
    
    //public int onGridPositionX; // TODO: Наверное, от этих двух int'ов можно избавиться, раз уж тут есть PositionOnGrid
    //public int onGridPositionY;
    public Vector2Int positionOnGrid; // позиция предмета на сетке инвентаря

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

    public void UpdateCounter(int count)
    {
        stackCounter.text = count.ToString();
    }
}
