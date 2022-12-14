using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class Player_Shooting : MonoBehaviour
{
    public float aimingDuration = 0.1f;
    // TODO: REWORK THIS SCRIPT
    public bool isShooting = false;

    public bool isAiming = false;

    public bool isCheckingStats = false;

    public bool isCheckingInventory = false;

    public GameObject inventory;

    public WeaponAnimationEvents animationEvents;

    // Новое
    public SCRIPT_Weapon weapon;
    [HideInInspector] public SCRIPT_MuzzleFlame muzzleFlame;
    public SCRIPT_AmmoShells ammoShells;

    private float lastTimeSingleShot; // Задержка перед выстрелом в одиночном режиме стрельбы
    private float tempSpread;         // Запомнить разброс от выстрела, чтобы вернуть его при выходе из прицеливания  
    private bool valueTaken;          

    public Animator animationController;

    public bool isHolstered;

    public SCRIPT_ActiveWeapon activeWeapon;
    public Transform leftHand;
    
    private GameObject magazineHand;

    private void Start()
    {
        HandleInventory(false);
        TryGetComponent<SCRIPT_ActiveWeapon>(out activeWeapon);
        isShooting = false;
        Equip(weapon);
        //animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    private void FixedUpdate()
    {
       // Aim();    
    }

    void LateUpdate()
    {
        ShootInput();                       // Проверка, нажата ли кнопка выстрела
        //SwitchWeapon();
        //muzzleFlame.FadeFlame();            // Угасание вспышки от выстрела
        //weapon.IsReloading();               // Блокировка стрельбы во время перезарядки
    }

    void ShootInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isCheckingInventory = !isCheckingInventory;
            HandleInventory(isCheckingInventory);
        }

        if (weapon)
        {
            if (Input.GetKeyDown(KeyCode.R) && isAiming && !activeWeapon.isReloading)
            {
                animationController.SetTrigger("Reload");
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
                isHolstered = animationController.GetBool("isHolstered");
                if (isAiming && !isHolstered && !activeWeapon.isReloading)
                {
                    // animationController.SetBool("isShooting", true);
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
             // animationController.SetBool("isShooting", false);
                weapon.StopFiring();
                weapon.shotPerformed = false;
            }
        }
        if (weapon)
        {
            weapon.UpdateBullet(Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.X) && !isAiming)
        {
            GetComponent<SCRIPT_ActiveWeapon>().ToggleActiveWeapon();
        }
    }

    private void HandleInventory(bool isActive)
    {
        Vector2 position = new Vector2();
        RectTransform inventoryRect = inventory.GetComponent<RectTransform>();

        if (isActive)
        {
            position.y = 630;
        }
        else
        {
            position.y = 3000;
        }
        position.x = inventoryRect.position.x;

        inventoryRect.position = position;

    }

    public void Equip(SCRIPT_Weapon weaponToEquip)
    {
        isHolstered = false;
        weapon = weaponToEquip;/*.GetComponent<SCRIPT_Weapon>();*/
        muzzleFlame = weaponToEquip.GetComponentInParent<SCRIPT_MuzzleFlame>();
        ammoShells = weaponToEquip.GetComponentInParent<SCRIPT_AmmoShells>();
    }

    //public void OnAnimationEvent(string eventName)
    //{
    //    Debug.Log("AnimationEventCalled");
    //    switch (eventName)
    //    {
    //        case "EjectMag":
    //            Debug.Log("EjectMag");
    //            weapon.audioSource.PlayOneShot(weapon.ejectMagSound);
    //            magazineHand = Instantiate(weapon.magazine, leftHand, true);
    //            weapon.magazine.SetActive(false);
    //            break;
    //        case "PutInMag":
    //            Debug.Log("PutInMag");
    //            weapon.audioSource.PlayOneShot(weapon.putMagSound);
    //            magazineHand.SetActive(false);
    //            break;
    //        case "GetNewMag":
    //            Debug.Log("GetNewMag");
    //            weapon.audioSource.PlayOneShot(weapon.pullOutMagSound);
    //            magazineHand.SetActive(false);
    //            break;
    //        case "InsertNewMag":
    //            Debug.Log("InsertNewMag");
    //            weapon.audioSource.PlayOneShot(weapon.InsertMagSound);
    //            weapon.magazine.SetActive(true);
    //            Destroy(magazineHand);
    //            break;
    //    }
    //}
}
