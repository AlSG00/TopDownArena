using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerAmmunition : MonoBehaviour
{

    AmmoCollection ammoCollection;

    

    private void Start()
    {
        ammoCollection.InitializeAmmoTypeCollection();
    }

    
    [System.Serializable]
    public class AmmoCollection
    {
        public Dictionary<AmmoType, Ammo> AmmoTypeCollection;

        public Ammo pistolAmmo = new Ammo();
        public Ammo rifleAmmo = new Ammo();
        public Ammo shotgunAmmo = new Ammo();
        public Ammo energyAmmo = new Ammo();
        public Ammo magnumAmmo = new Ammo();

        internal void InitializeAmmoTypeCollection()
        {
            AmmoTypeCollection = new()
            {
                { AmmoType.pistolAmmo, pistolAmmo },
                { AmmoType.pistolAmmo, rifleAmmo },
                { AmmoType.pistolAmmo, shotgunAmmo },
                { AmmoType.pistolAmmo, energyAmmo },
                { AmmoType.pistolAmmo, magnumAmmo }
            };
        }
    }

    public enum AmmoType
    {
        pistolAmmo,
        rifleAmmo,
        shotgunAmmo,
        energyAmmo,
        magnumAmmo
    }


    [System.Serializable]
    public class Ammo
    {
        public int capacity;
        public int left;

        // TODO: DELETE. Test constructor for ammunition debug.
        public Ammo()
        {
            capacity = 999;
            left = capacity;
        }

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

        //private bool CheckCapacity(int ammo)
        //{
        //    // Check current ammo type capacity when trying to get ammo
        //}

        //private bool CheckRemaining(int ammo)
        //{
        //    // Check remaining ammo when trying to reload
        //}
    }
}
