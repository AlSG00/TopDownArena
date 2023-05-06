using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCRIPT_InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public delegate void showAction(bool showInfo);
    public static showAction OnShowItemInfo;

    public string name; // метка имени
    [SerializeField] public SCRIPT_ItemData itemData; // общая инфа (размер в инвентаре, имя, иконка)
    public GameObject objectPrefab; // 3д-объект, который заспавнится, когда предмет будет выброшен

    [Header("Properties")]
    [Range(0, 999)] public float weight; // вес предмета, добавляется игроку
    public bool isRotatable = false; // можно ли вращать предмет 
    public bool isUsable = true; // можно ли использовать предмет

    [Header("Stacking properties")]
    public bool isStackable = false; // можно ли стакать предмет
    public bool isSingleDropping = false; // Можно ли выбросить один предмет, а не весь стак
    [Range(0, 999)] public int stackCount = 0; // текущий размер стака
    [Range(0, 999)] public int maxStackCount = 0; // максимальный размер стака

    [Header("Audio")]
    public AudioClip useItemAudio; // TODO: Переименовать. Звук, воспроизводимый, когда предмет используется
    public AudioSource useItemAudioSource; // источник звука использования предмета

    [Header("UI")]
    public TextMeshProUGUI stackCounter; // UI счетчик предметов в стаке
    
    [HideInInspector] public bool isDropping; //TODO: Возможно могу избавиться. Флаг, чтобы при выкидывании не спавнилось несколько предметов сразу
    [HideInInspector] public bool isRotated = false; // вращался ли предмет
    [HideInInspector] public bool isOnCursor = false;

    public Vector2Int positionOnGrid; // позиция предмета на сетке инвентаря

    public SCRIPT_ItemGrid lastGrid;
    private bool isMouseOverItem = false;
    [SerializeField] private float timeToShowInfo = 0f;
    private float cursorHoldingTime = 0f;

    private void Awake()
    {
        cursorHoldingTime = 0f;
    }

    private void Start()
    {
        useItemAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isMouseOverItem)
        {
            Продумать мезанизм подсказок, как они должны отобрадаться и как подвязать сюда скрипт отслеживания активности курсора, используя ивенты, а не прямые ссылки
        }
        else
        {
            cursorHoldingTime = 0f;
        }
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

        Debug.Log(isRotated);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, isRotated == true ? 90f : 0f);
    }

    public void UpdateCounter()
    {
        if (stackCounter == null)
        {
            GetComponentInChildren<TextMeshProUGUI>();
            if (stackCounter == null)
            {
                return;
            }
        }

        stackCounter.text = stackCount.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Entered");
        isMouseOverItem = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Leaved");
        isMouseOverItem = false;
    }
}
