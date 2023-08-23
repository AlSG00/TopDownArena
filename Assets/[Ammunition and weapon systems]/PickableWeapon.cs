using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableWeapon : MonoBehaviour, IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    [Range(1, 1)] public int stackCount;

    //public SCRIPT_Weapon weaponPrefab;
    //SCRIPT_ActiveWeapon activeWeapon;
    public SCRIPT_InventoryItem inventoryItem;

    public delegate int PickAction(SCRIPT_InventoryItem item, int stackCount);
    public static event PickAction OnWeaponPick;


    private void Awake()
    {
        canInteract = false;
        alreadyInteracting = false;
        inInteractionArea = false;
    }

    private void Start()
    {
        // TODO: Get rid of this line
        //activeWeapon = GameObject.Find("_Player").GetComponent<SCRIPT_ActiveWeapon>();
    }

    public void Interact()
    {
        Debug.Log("<Color=yellow>Interact</color>");
        OnWeaponPick?.Invoke(inventoryItem, stackCount);
        Destroy(gameObject);
    }

    // TODO: Implement input key holding to activate this method
    //public void InteractAndUse()
    //{
    //    if (activeWeapon)
    //    {

    //        SCRIPT_Weapon weaponToPickup = Instantiate(weaponPrefab);
    //        activeWeapon.Equip(weaponToPickup);
    //        Destroy(gameObject);
    //    }
    //}
}
