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

    private void Start()
    {
        isShooting = false;
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

        if (Input.GetButton("Fire1"))
        {
            if (!weapon.shotPerformed)
            {
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
