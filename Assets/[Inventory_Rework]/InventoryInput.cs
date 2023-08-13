using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    private bool isHoldingShiftButton = false;




    private void Update()
    {
        //ItemIconDrag();

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
            RotateItem();
        }

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        //HandleItemHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            if (isHoldingShiftButton)
            {
                MoveItemFast();
            }
            else
            {
                LeftMouseButtonPress();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RightMouseButtonPress();
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (itemInfoWindow.isShowingDetails)
            {
                itemInfoWindow.ShowDetails(false);
            }
            else
            {
                itemInfoWindow.ShowDetails(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isHoldingDropItemButton = true;
            buttonHoldTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            isHoldingDropItemButton = false;
            if (isDroppingStack)
            {
                isDroppingStack = false;
            }
            else
            {
                DropItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isHoldingCheckStateButton = true;
            buttonHoldTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            HandleStateIconsVisibility();
        }

        if (isHoldingCheckStateButton &&
            isCheckingInventory == false)
        {
            buttonHoldTime += Time.deltaTime;
            if (buttonHoldTime >= timeToHold
                && isHighlightingStateIcons == false)
            {
                buttonHoldTime = 0;
                isHighlightingStateIcons = true;
                OnStateIconShow?.Invoke(true);
            }
        }

        if (isHoldingDropItemButton &&
            isCheckingInventory)
        {
            buttonHoldTime += Time.deltaTime;
            if (buttonHoldTime >= timeToHold)
            {
                buttonHoldTime = 0;
                isDroppingStack = true;
                DropStack();
            }
        }
    }
}
