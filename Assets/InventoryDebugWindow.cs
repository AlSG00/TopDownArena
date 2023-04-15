using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDebugWindow : MonoBehaviour
{
    public List<Image> debugIconsList = new List<Image>();
    public SCRIPT_ItemGrid inventoryGrid;
    Image[,] debugIconsArray = new Image[5, 5];
    private void Start()
    {
        int count = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                debugIconsArray[i, j] = debugIconsList[count];
                count++;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (inventoryGrid.inventoryItemSlot[i, j] != null)
                {
                    debugIconsArray[j, i].color = Color.green;
                }
                else
                {
                    debugIconsArray[j, i].color = Color.black;
                }
               
            }
        }
    }
}
