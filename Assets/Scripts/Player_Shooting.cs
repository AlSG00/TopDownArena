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
        muzzleFlame.FadeFlame();   // Угасание вспышки от выстрела
    }

    void ShootInput()
    {
        //if (Input.GetButton("Fire1"))
        //{          
        //    Gun.Instance.Shoot();            
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Gun.Instance.Reload();
        //}

        if (Input.GetButtonDown("Fire1"))
        {
            weapon.StartFiring();
        }

        if (weapon.isFiring)
        {
            weapon.UpdateFiring(Time.deltaTime);
        }
        weapon.UpdateBullet(Time.deltaTime);
       // ammoShells.UpdateShells();

        if (Input.GetButtonUp("Fire1"))
        {
            weapon.StopFiring();
        }
    }
}
