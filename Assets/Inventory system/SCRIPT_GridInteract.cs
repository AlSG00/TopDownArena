using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SCRIPT_ItemGrid))]
public class SCRIPT_GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    SCRIPT_InventoryController inventoryController;
    SCRIPT_ItemGrid itemGrid;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(SCRIPT_InventoryController)) as SCRIPT_InventoryController;
        itemGrid = GetComponent<SCRIPT_ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = itemGrid;
        inventoryController.gridRect = inventoryController.selectedItemGrid.GetComponent<RectTransform>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = null;
    }
}
