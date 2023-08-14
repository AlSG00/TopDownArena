using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    private bool isHoldingShiftButton = false;
    private bool isHoldingDropItemButton = false;
    private bool isHoldingCheckStateButton = false;
    private float buttonHoldTime = 0f;
    private float timeToHold = 0.3f;

    [SerializeField] InventoryController inventory;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isHoldingShiftButton = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isHoldingShiftButton = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            inventory.RotateItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isHoldingShiftButton)
            {
                inventory.MoveItemFast();
            }
            else
            {
                inventory.LeftMouseButtonPress();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            inventory.RightMouseButtonPress();
        }

        if (Input.GetMouseButtonDown(2))
        {
            inventory.HandleMoreItemInfoVisibility();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isHoldingDropItemButton = true;
            buttonHoldTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            isHoldingDropItemButton = false;
            inventory.HandleItemDrop();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isHoldingCheckStateButton = true;
            buttonHoldTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isHoldingCheckStateButton = false;
            inventory.HandleStateIconsVisibility(ref buttonHoldTime, timeToHold);
        }

        if (isHoldingCheckStateButton && inventory.isCheckingInventory == false)
        {
            inventory.HandleStateIconButtonHolding(ref buttonHoldTime, timeToHold);
        }

        if (isHoldingDropItemButton && inventory.isCheckingInventory)
        {
            inventory.HandleStackDrop(ref buttonHoldTime, timeToHold);
        }
    }
}
