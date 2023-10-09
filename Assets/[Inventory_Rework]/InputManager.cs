using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InventoryController inventory;

    // Combat variables
    public Weapon weapon;
    public SCRIPT_ActiveWeapon activeWeapon;
    public Animator animationController;


    public bool isAiming = false;
    //public bool isReloading = false;

    //public bool isHolstered;
    private float _lastTimeSingleShot;

    // Inventory variables
    private bool isHoldingShiftButton = false;
    private bool isHoldingDropItemButton = false;
    private bool isHoldingCheckStateButton = false;
    private float buttonHoldTime = 0f;
    private float timeToHold = 0.3f;


    

    private void Update()
    {
        MovementInput();
        CombatInput();
        InventoryInput();
    }

    private void MovementInput()
    {
        // TODO: Add a Player_movement logic here
    }

    private void CombatInput()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    isCheckingInventory = !isCheckingInventory;
        //    inventory.HandleInventory(isCheckingInventory);
        //   // HandleInventory(isCheckingInventory);
        //}

        if (weapon)
        {
            if (Input.GetKeyDown(KeyCode.R) && isAiming/* && isReloading == false*/)
            {
              //  isReloading = true;
                weapon.TryReload();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                isAiming = true;
                animationController.SetBool("isAiming", isAiming);
            }

            if (Input.GetButtonUp("Fire2"))
            {
                isAiming = false;
                animationController.SetBool("isAiming", isAiming);
            }

            // TODO: Simplify
            if (Input.GetButton("Fire1"))
            {
                if (isAiming/* && isReloading == false*/)
                {
                    if ((weapon.shotPerformed == false) &&
                        ((_lastTimeSingleShot + weapon.singleShotDelay) <= Time.time))
                    {
                        _lastTimeSingleShot = Time.time;
                        weapon.StartFiring();
                        if (weapon.singleShots)
                        {
                            weapon.shotPerformed = true;
                        }
                    }
                }
                else
                {
                    // TODO: Рукопашная
                }

            }
            else
            {
                weapon.StopFiring();
                weapon.shotPerformed = false;
            }
        }
    }

    private void InventoryInput()
    {
        // TODO: Rework and uncomment
        //if(inventory.isCheckingInventory)
        //{
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.isCheckingInventory)
            {
                inventory.TryBindItem(InventoryController.BindSlot.HolsterSlot);
            }
            else
            {
                inventory.TryUseBindedItem(InventoryController.BindSlot.HolsterSlot);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (inventory.isCheckingInventory)
            {
                inventory.TryBindItem(InventoryController.BindSlot.BeltSlot);
            }
            else
            {
                inventory.TryUseBindedItem(InventoryController.BindSlot.BeltSlot);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (inventory.isCheckingInventory)
            {
                inventory.TryBindItem(InventoryController.BindSlot.BackSlot);
            }
            else
            {
                inventory.TryUseBindedItem(InventoryController.BindSlot.BackSlot);
            }
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
            if (inventory.isCheckingInventory)
            {
                inventory.RightMouseButtonPress();
            }
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

    internal void Equip_2(Weapon weaponToActivate)
    {
        //isHolstered = false;
        weapon = weaponToActivate;/*.GetComponent<SCRIPT_Weapon>();*/
        //muzzleFlame = weaponToActivate.GetComponentInParent<SCRIPT_MuzzleFlame>();
        //ammoShells = weaponToActivate.GetComponentInParent<SCRIPT_AmmoShells>();
    }

    private async void PlayReloadAnimation()
    {
        activeWeapon.ReloadWeapon();
    }
}
