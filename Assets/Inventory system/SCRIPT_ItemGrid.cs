using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ItemGrid : MonoBehaviour
{
    private const float _tileSizeWidth = 32;
    private const float _tileSizeheight = 32;

    RectTransform rectTransform;

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;

        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        // Вычисление положения ячеек на сетке
        tileGridPosition.x = (int)(positionOnTheGrid.x / _tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / _tileSizeheight);

        return tileGridPosition;
    }
}
