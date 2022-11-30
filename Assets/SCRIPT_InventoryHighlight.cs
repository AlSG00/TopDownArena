using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Show(bool isShowing)
    {
        highlighter.gameObject.SetActive(isShowing);
    }
    public void SetSize(SCRIPT_InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * SCRIPT_ItemGrid._tileSizeWidth;
        size.y = targetItem.itemData.height * SCRIPT_ItemGrid._tileSizeHeight;
        highlighter.sizeDelta = size;
    }

    public void SetPosition(SCRIPT_ItemGrid targetGrid, SCRIPT_InventoryItem targetItem)
    {
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2 position = targetGrid.CalculatePositionOnGrid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY
            );

        highlighter.localPosition = position;
    }
}
