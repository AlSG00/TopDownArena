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

    [SerializeField] private InventoryController inventory;


    private void Update()
    {
        //if(inventory.isCheckingInventory)
        //{
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.isCheckingInventory)
            {
                inventory.TryBindItem(InventoryController.BindSlot.Slot_1);
            }
            else
            {
                inventory.TryUseBindedItem(InventoryController.BindSlot.Slot_1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.TryBindItem(InventoryController.BindSlot.Slot_2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.TryBindItem(InventoryController.BindSlot.Slot_3);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            inventory.TryUnbindItem();
        }

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
