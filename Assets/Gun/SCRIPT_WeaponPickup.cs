using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_WeaponPickup : MonoBehaviour
{
    public SCRIPT_Weapon weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        SCRIPT_ActiveWeapon activeWeapon = other.GetComponent<SCRIPT_ActiveWeapon>();
    }

    private void PickWeapon()
    {
        SCRIPT_ActiveWeapon activeWeapon;

        SCRIPT_Weapon weaponToPick = Instantiate(weaponPrefab);
    //    activeWeapon.Equip(weaponToPick);
    }
}
