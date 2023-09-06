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
    public static event showAction OnShowItemInfo;
    public RectTransform itemRectTransform;
    public SCRIPT_ItemGrid lastGrid;

    public string name; // метка имени
    [SerializeField] public SCRIPT_ItemData itemData; // общая инфа (размер в инвентаре, имя, иконка)
    public GameObject objectPrefab; // 3д-объект, который заспавнится, когда предмет будет выброшен

    [Header("Properties")]
    [Range(0, 999)] public float weight; // вес предмета, добавляется игроку
    public bool isRotatable = false; // можно ли вращать предмет 
    public bool isUsable = false; // можно ли использовать предмет
    public bool isConsumable = false;
    public bool isEquippable = false;


    public InventoryController.BindSlot[] permittedSlots;
    public bool isBinded = false;
    public InventoryController.BindSlot bindedSlot;
    //public bool isVisualizable = false;

    [Header("Stacking properties")]
    public bool isStackable = false; // можно ли стакать предмет
    //public bool isDividable = false; // Можно ли выбросить один предмет, а не весь стак
    [Range(0, 999)] public int stackCount = 0; // текущий размер стака
    [Range(0, 999)] public int maxStackCount = 0; // максимальный размер стака

    // TODO: Move to separate script
    [Header("Audio")]
    //public AudioSource useItemAudioSource; // источник звука использования предмета
    public AudioClip useItemAudio;
    public AudioClip pickFromGroundAudio;
    public AudioClip pickFromGridAudio;
    public AudioClip placeOnGridAudio;

    [Header("UI")]
    public TextMeshProUGUI stackCounter; // UI счетчик предметов в стаке
    public Image bindIndicator;
    [HideInInspector] public bool isDropping; //TODO: Возможно могу избавиться. Флаг, чтобы при выкидывании не спавнилось несколько предметов сразу
    [HideInInspector] public bool isRotated = false; // вращался ли предмет
    [HideInInspector] public bool isOnCursor = false;

    public Vector2Int positionOnGrid; // позиция предмета на сетке инвентаря

    bool isMouseOverItem = false;
    private bool _isCursorActive = true;

    #region INIT

    private void OnEnable()
    {
        MouseMovementTracker.OnCursorInactive += IsCursorActive;
    }

    private void OnDisable()
    {
        MouseMovementTracker.OnCursorInactive -= IsCursorActive;
    }

    internal void Init(SCRIPT_ItemData itemData)
    {
        this.itemData = itemData;
        name = itemData.name;
        GetComponent<Image>().sprite = itemData.icon;
        Vector2 size = new Vector2();
        size.x = itemData.width * SCRIPT_ItemGrid._tileSizeWidth;
        size.y = itemData.height * SCRIPT_ItemGrid._tileSizeHeight;
        itemRectTransform.sizeDelta = size;
    }

    #endregion

    // TODO: Change rotation direction
    internal void Rotated()
    {
        if (Height == 1 && Width == 1)
        {
            Debug.Log("Not rotatable");
            return;
        }

        isRotated = !isRotated;
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

    private void IsCursorActive(bool isActive)
    {
        _isCursorActive = isActive;
    }

    public void OnCursor(bool onCursor)
    {
        isOnCursor = onCursor;
        OnShowItemInfo?.Invoke(!isOnCursor, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isOnCursor == false)
        {
            isMouseOverItem = true;
            OnShowItemInfo?.Invoke(true, this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isOnCursor == false)
        {
            isMouseOverItem = false;
            OnShowItemInfo?.Invoke(false, this);
        }
    }
    
    internal void BindKey()
    {
        if (isBinded == false)
        {
            isBinded = true;
            bindIndicator.enabled = true;
        }
        // TODO: Enable sprite indicating that current item is binded
        // (draw a frame around the item and put item in the GUI Slot)
    }

    internal void UnbindKey()
    {
        if (isBinded)
        {
            isBinded = false;
            bindIndicator.enabled = false;
        }
        // TODO: Disable sprite indicating that current item bindnd
        // (hide a frame around the item and erase item from the GUI Slot)
    }

    // TODO: Unbind item after drop
    // TODO: Unbind item after moving to container
    // TODO: Unbind item after fully consuming it
}
