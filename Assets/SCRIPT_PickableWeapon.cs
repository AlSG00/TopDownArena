using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PickableWeapon : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }


    public SCRIPT_Weapon weaponPrefab;
    SCRIPT_ActiveWeapon activeWeapon;

    private void Awake()
    {
        canInteract = false;
        alreadyInteracting = false;
        inInteractionArea = false;
    }

    private void Start()
    {
        activeWeapon = GameObject.Find("_Player").GetComponent<SCRIPT_ActiveWeapon>();
    }

    public void Interact()
    {
        if (activeWeapon)
        {
            SCRIPT_Weapon weaponToPickup = Instantiate(weaponPrefab);
            activeWeapon.Equip(weaponToPickup);
            Destroy(gameObject);
        }
    }


    

    //private void OnTriggerEnter(Collider other)
    //{
    //    SCRIPT_ActiveWeapon activeWeapon = other.GetComponent<SCRIPT_ActiveWeapon>();

    //    if (activeWeapon)
    //    {
    //        {
    //            SCRIPT_Weapon weaponToPickup = Instantiate(weaponPrefab);
    //            activeWeapon.Equip(weaponToPickup);
    //        }
    //        //Player_Shooting player = other.GetComponent<Player_Shooting>();
    //        // player.Equip(weaponToPickup);
    //    }
    //}
}
