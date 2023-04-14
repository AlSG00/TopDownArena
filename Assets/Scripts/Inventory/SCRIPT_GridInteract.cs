using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SCRIPT_ItemGrid))]
public class SCRIPT_GridInteract : MonoBehaviour, IPointerEnterHandler//S, //IPointerExitHandler*/
{
    InventoryController inventoryController;
    SCRIPT_ItemGrid itemGrid;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<SCRIPT_ItemGrid>();
        inventoryController.selectedItemGridRect = inventoryController.inventoryGrid.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = itemGrid;
        inventoryController.selectedItemGridRect = inventoryController.selectedItemGrid.GetComponent<RectTransform>();
    }

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    inventoryController.selectedItemGrid = null;
    //}
}
