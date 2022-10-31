using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class Player_Shooting : MonoBehaviour
{

    //public Rig aimLayer;
    public float aimingDuration = 0.1f;
    // TODO: REWORK THIS SCRIPT
    public bool isShooting;
    public bool isAiming;
  //  public GameObject prefab;
  //  public GameObject muzzle;
    //public TestRaycastWeapon weapon;

    // Новое
    public SCRIPT_Weapon weapon;
    public SCRIPT_WeaponSlots weaponSlots;
    public SCRIPT_MuzzleFlame muzzleFlame;
    public SCRIPT_AmmoShells ammoShells;

    private float lastTimeSingleShot; // Задержка перед выстрелом в одиночном режиме стрельбы
    private float tempSpread;         // Запомнить разброс от выстрела, чтобы вернуть его при выходе из прицеливания  
    private bool valueTaken;          

    // TODO: Протестировать лист
    public List<string> forbidAiming;

    public Animator animationController;

    bool isHolstered;
    //public class Weapon
    //{
    //    public GameObject WeaponPref;
    //    public bool isActive;
    //}



    private void Start()
    {
        
        isShooting = false;
        isHolstered = animationController.GetBool("isHolstered");
        //tempSpread = weapon.bulletSpreadValue;
        // valueTaken = false;

        // TOTOD: Подтягивать ссылки на скрипты
    }

    private void FixedUpdate()
    {
        Aim();    
    }

    void LateUpdate()
    {
        ShootInput();                       // Проверка, нажата ли кнопка выстрела
        SwitchWeapon();
        muzzleFlame.FadeFlame();            // Угасание вспышки от выстрела
        weapon.IsReloading();               // Блокировка стрельбы во время перезарядки
    }

    public enum ActiveWeapon
    {
        Holster = 1,
        Belt = 2,
        Back = 3
    }
    private void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetWeapon(ActiveWeapon.Holster);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetWeapon(ActiveWeapon.Belt);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetWeapon(ActiveWeapon.Back);
        }
    }

    private void GetWeapon(ActiveWeapon slot)
    {
        GameObject activeSlot = null;
        try
        {
            switch (slot)
            {
                case ActiveWeapon.Holster:
                    weapon = weaponSlots.holsterSlot.GetComponent<SCRIPT_Weapon>();
                    activeSlot = weaponSlots.holsterSlot;
                    break;

                case ActiveWeapon.Belt:
                    weapon = weaponSlots.beltSlot.GetComponent<SCRIPT_Weapon>();
                    activeSlot = weaponSlots.beltSlot;
                    break;

                case ActiveWeapon.Back:
                    weapon = weaponSlots.backSlot.GetComponent<SCRIPT_Weapon>();
                    activeSlot = weaponSlots.backSlot;
                    break;
            }
        }
        catch
        {
            Debug.Log($"{slot.ToString()}: Weapon is missing");
        }
        muzzleFlame = activeSlot.GetComponent<SCRIPT_MuzzleFlame>();
        ammoShells = activeSlot.GetComponent<SCRIPT_AmmoShells>();
        Debug.Log(weapon.bulletSpreadValue);
    }

    void ShootInput()
    {
        if (weapon)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
        //        animationController.SetTrigger("Reloading");
                weapon.Reload();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                isAiming = true;
                animationController.SetBool("isAiming", isAiming);
                //Aim(true);

                //if (!valueTaken && !forbidAiming.Contains(weapon.name))
                //{
                //    tempSpread = weapon.bulletSpreadValue;
                //    weapon.bulletSpreadValue = 0;
                //    valueTaken = true;
                //}
            }

            if (Input.GetButtonUp("Fire2"))
            {
                isAiming = false;
                animationController.SetBool("isAiming", isAiming);
                // Aim(false);
                //if (valueTaken)
                //{
                //    weapon.bulletSpreadValue = tempSpread;
                //    valueTaken = false;
                //}
            }

            if (Input.GetButton("Fire1"))
            {
                if (isAiming && !isHolstered)
                {
               //     animationController.SetBool("isShooting", true);
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
                    // TODO: Рукопашная
                }

            }
            else
            {
          //      animationController.SetBool("isShooting", false);
                weapon.StopFiring();
                weapon.shotPerformed = false;
            }
        }
        weapon.UpdateBullet(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.X) && !isAiming)
        {
            isHolstered = animationController.GetBool("isHolstered");
            animationController.SetBool("isHolstered", !isHolstered);
            isHolstered = !isHolstered;
        }
    }

    private void Aim()
    {
        //if (isAiming)
        //{
        //    aimLayer.weight += Time.deltaTime / aimingDuration;
        //    //aimLayer.weight = Mathf.Lerp(0, 1, aimingDuration);
        //}
        //else
        //{
        //    //aimLayer.weight = Mathf.Lerp(1, 0, aimingDuration);
        //    aimLayer.weight -= Time.deltaTime / aimingDuration;
        //}
    }

    public void Equip(SCRIPT_Weapon weaponToEquip)
    {
        weapon = weaponToEquip.GetComponent<SCRIPT_Weapon>();
        muzzleFlame = weaponToEquip.GetComponentInParent<SCRIPT_MuzzleFlame>();
        ammoShells = weaponToEquip.GetComponentInParent<SCRIPT_AmmoShells>();
    }    
}
