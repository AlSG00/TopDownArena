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

    public delegate void showAction(bool showInfo, SCRIPT_InventoryItem item);
    public static showAction OnShowItemInfo;
    public RectTransform itemRectTransform;
    public SCRIPT_ItemGrid lastGrid;

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
    //public AudioSource useItemAudioSource; // источник звука использования предмета
    public AudioClip useItemAudio;
    public AudioClip pickFromGroundAudio;
    public AudioClip pickFromGridAudio;
    public AudioClip placeOnGridAudio;

    [Header("UI")]
    public TextMeshProUGUI stackCounter; // UI счетчик предметов в стаке
    
    [HideInInspector] public bool isDropping; //TODO: Возможно могу избавиться. Флаг, чтобы при выкидывании не спавнилось несколько предметов сразу
    [HideInInspector] public bool isRotated = false; // вращался ли предмет
    [HideInInspector] public bool isOnCursor = false;

    public Vector2Int positionOnGrid; // позиция предмета на сетке инвентаря

    bool isMouseOverItem = false;
    private bool isCursorActive = true;

    private void OnEnable()
    {
        MouseMovementTracker.OnCursorInactive += SetCursorActivityFlag;
    }

    private void OnDisable()
    {
        MouseMovementTracker.OnCursorInactive -= SetCursorActivityFlag;
    }

    //private void Start()
    //{
    //    useItemAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
    //}

    internal void Set(SCRIPT_ItemData itemData)
    {
        this.itemData = itemData;
        name = itemData.name;
        GetComponent<Image>().sprite = itemData.icon;
        Vector2 size = new Vector2();
        size.x = itemData.width * SCRIPT_ItemGrid._tileSizeWidth;
        size.y = itemData.height * SCRIPT_ItemGrid._tileSizeHeight;
        itemRectTransform.sizeDelta = size;
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
        itemRectTransform.rotation = Quaternion.Euler(0, 0, isRotated == true ? 90f : 0f);
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

    private void SetCursorActivityFlag(bool isActive)
    {
        isCursorActive = isActive;
        //if (isMouseOverItem &&
        //    isCursorActive == false &&
        //    isOnCursor == false)
        //{
        //    OnShowItemInfo?.Invoke(true, this);
        //}
    }

    public void SetOnCursorFlag(bool onCursor)
    {
        isOnCursor = onCursor;
        if (isOnCursor)
        {
            OnShowItemInfo?.Invoke(false, this);
        }
        else
        {
            OnShowItemInfo?.Invoke(true, this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isOnCursor == false)
        {
            Debug.Log("OnPointerEnter");
            isMouseOverItem = true;
            OnShowItemInfo?.Invoke(true, this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isOnCursor == false)
        {
            Debug.Log("OnPointerExit");
            isMouseOverItem = false;
            OnShowItemInfo?.Invoke(false, this);
        }
    }
}
