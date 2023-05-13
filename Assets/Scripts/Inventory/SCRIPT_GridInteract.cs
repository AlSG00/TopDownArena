using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SCRIPT_ItemGrid))]
public class SCRIPT_GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryController inventoryController;
    public SCRIPT_ItemGrid itemGrid;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<SCRIPT_ItemGrid>();
        inventoryController.selectedItemGridRect = inventoryController.inventoryGrid.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       // Debug.Log($"Pointer entered {gameObject.name}");
        inventoryController.selectedItemGrid = itemGrid;
        inventoryController.selectedItemGridRect = inventoryController.selectedItemGrid.GetComponent<RectTransform>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"Pointer leaved {gameObject.name}");
    }
}
