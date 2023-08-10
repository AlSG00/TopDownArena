using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
