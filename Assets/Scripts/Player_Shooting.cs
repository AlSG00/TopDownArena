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
    public SCRIPT_MuzzleFlame muzzleFlame;
    public SCRIPT_AmmoShells ammoShells;

    private float lastTimeSingleShot;
    private float tempSpread;
    private bool valueTaken;

    public List<string> forbidAiming;

    private void Start()
    {
        isShooting = false;
        tempSpread = weapon.bulletSpreadValue;
        valueTaken = false;
    }

    void LateUpdate()
    {
        ShootInput();                       // Проверка, нажата ли кнопка выстрела
        muzzleFlame.FadeFlame();            // Угасание вспышки от выстрела
        weapon.IsReloading();               // Блокировка стрельбы во время перезарядки
    }

    void ShootInput()
    {
        //if (Input.GetButton("Fire1"))
        //{          
        //    Gun.Instance.Shoot();            
        //}

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
        Debug.Log($"TempSpread {tempSpread}");
        Debug.Log($"BulletSpread {weapon.bulletSpreadValue}");
        //if (Input.GetButtonUp("Fire2"))
        //{
        //    weapon.bulletSpreadValue = tempSpread;
        //}

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

        //if (weapon.isFiring)
        //{
        //    weapon.UpdateFiring(Time.deltaTime);
        //}
        weapon.UpdateBullet(Time.deltaTime);
       // ammoShells.UpdateShells();

        //if (Input.GetButtonUp("Fire1"))
        //{
        //    weapon.StopFiring();
        // //   weapon.shotPerformed = false;
        //}
    }
}
