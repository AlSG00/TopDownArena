using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    private void Start()
    {
        Show(false);
    }

    public void Show(bool isShowing)
    {
        highlighter.gameObject.SetActive(isShowing);
    }

    public void SetSize(SCRIPT_InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.Width * SCRIPT_ItemGrid._tileSizeWidth;
        size.y = targetItem.Height * SCRIPT_ItemGrid._tileSizeHeight;
        highlighter.sizeDelta = size;
    }

    public void SetPosition(SCRIPT_ItemGrid targetGrid, SCRIPT_InventoryItem targetItem)
    {
        // highlighter.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2 position = targetGrid.CalculatePositionOnGrid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY
            );

        highlighter.localPosition = position;
    }

    public void SetParent(SCRIPT_ItemGrid targetGrid)
    {
        if (targetGrid == null)
        {
            return;
        }

        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
        highlighter.SetAsFirstSibling();
    }

    public void SetPosition(SCRIPT_ItemGrid targetGrid, SCRIPT_InventoryItem targetItem, int positionX, int positionY)
    {
        Vector2 position = targetGrid.CalculatePositionOnGrid(
            targetItem,
            positionX,
            positionY
            );

        highlighter.localPosition = position;
    }
}
