using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Shooting : MonoBehaviour
{
    // TODO: REWORK THIS SCRIPT
    public bool isShooting;
    public GameObject prefab;
    public GameObject barrel;
    //public TestRaycastWeapon weapon;

    // Новое
    public SCRIPT_Weapon weapon;
    public SCRIPT_WeaponSlots weaponSlots;
    public SCRIPT_MuzzleFlame muzzleFlame;
    public SCRIPT_AmmoShells ammoShells;

    private float lastTimeSingleShot;
    private float tempSpread;
    private bool valueTaken;

    public List<string> forbidAiming;

    public class Weapon
    {
        public GameObject WeaponPref;
        public bool isActive;
    }



    private void Start()
    {
        isShooting = false;
        tempSpread = weapon.bulletSpreadValue;
        valueTaken = false;

        // TOTOD: Подтягивать ссылки на скрипты
    }

    void LateUpdate()
    {
        ShootInput();                       // Проверка, нажата ли кнопка выстрела
        SwitchWeapon();
        muzzleFlame.FadeFlame();            // Угасание вспышки от выстрела
        weapon.IsReloading();               // Блокировка стрельбы во время перезарядки
    }

    private void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = weaponSlots.holsterSlot.GetComponent<SCRIPT_Weapon>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = weaponSlots.beltSlot.GetComponent<SCRIPT_Weapon>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = weaponSlots.backSlot.GetComponent<SCRIPT_Weapon>();
        }
    }

    void ShootInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
        }

        if (Input.GetButton("Fire2"))
        {
            // TODO: Может возникнуть проблема при реализации смены оружия
            // Каждый раз подтягивать зависимость??????????????
            if (!valueTaken && !forbidAiming.Contains(weapon.name))
            {
                
                tempSpread = weapon.bulletSpreadValue;
                weapon.bulletSpreadValue = 0;
                valueTaken = true;
            }
        }
        else
        {
            weapon.bulletSpreadValue = tempSpread;
            valueTaken = false;
        }

        if (Input.GetButton("Fire1"))
        {
            if (!weapon.shotPerformed && lastTimeSingleShot + weapon.singleShotDelay <= Time.time)
            {
                lastTimeSingleShot = Time.time;
                weapon.StartFiring();
                if (weapon.singleShots)
                {
                    weapon.shotPerformed = true;
                }
            }
        }
        else
        {
            weapon.StopFiring();
            weapon.shotPerformed = false;
        }

        weapon.UpdateBullet(Time.deltaTime);
    }
}
