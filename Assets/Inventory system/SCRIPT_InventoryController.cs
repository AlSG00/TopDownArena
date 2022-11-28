using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_InventoryController : MonoBehaviour
{
    [HideInInspector] public SCRIPT_ItemGrid selectedItemGrid;

    private void Update()
    {
        if (selectedItemGrid == null)
        {
            return;
        }

        Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
    }
}
