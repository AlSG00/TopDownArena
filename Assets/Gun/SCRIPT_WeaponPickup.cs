using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        SCRIPT_ActiveWeapon activeWeapon = other.GetComponent<SCRIPT_ActiveWeapon>();

        if (activeWeapon)
        {
            SCRIPT_Weapon weaponToPickup = Instantiate(weaponPrefab.GetComponent<SCRIPT_Weapon>());
            activeWeapon.Equip(weaponToPickup);
            Player_Shooting player = other.GetComponent<Player_Shooting>();
            player.Equip(weaponToPickup);
        }
    }


}
