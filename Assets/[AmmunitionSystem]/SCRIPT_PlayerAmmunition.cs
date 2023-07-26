using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerAmmunition : MonoBehaviour
{
    public class Ammo
    {
        public int capacity;
        public int left;

        public void AddAmmo(int ammo)
        {
            if (ammo > capacity - left)
            {
                left = capacity;
            }
            else
            {
                left += ammo;
            }
        }

        public int TakeAmmo(int ammo)
        {
            int toReturn = 0;
            if (left == 0)
            {
                Debug.Log("Can't reload");
            }

            if (ammo > left)
            {
                toReturn = left;
                left = 0;
                return toReturn;
            }
            else
            {
                toReturn = ammo;
                left -= ammo;
                return toReturn;
            }
        }

        private bool CheckCapacity()
        {
            // Check current ammo type capacity when trying to get ammo
        }

        private bool CheckRemaining()
        {
            // Check remaining ammo when trying to reload
        }
    }

    public Ammo pistolAmmo = new Ammo();
    public Ammo rifleAmmo = new Ammo();
    public Ammo shotgunAmmo = new Ammo();

    public int pistolAmmoCapacity = 90;
    public int rifleAmmoCapacity = 150;
    public int shotgunAmmoCapacity = 48;

    private void Awake()
    {
        pistolAmmo.capacity = pistolAmmoCapacity;
        pistolAmmo.left = pistolAmmo.capacity;

        rifleAmmo.capacity = rifleAmmoCapacity;
        rifleAmmo.left = rifleAmmo.capacity;

        shotgunAmmo.capacity = shotgunAmmoCapacity;
        shotgunAmmo.left = shotgunAmmo.capacity;
    }
}
