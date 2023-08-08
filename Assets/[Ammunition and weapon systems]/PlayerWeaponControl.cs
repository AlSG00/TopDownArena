using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponControl : MonoBehaviour
{
    public float aimingDuration = 0.1f;
    // TODO: REWORK THIS SCRIPT
    public bool isShooting = false;

    public bool isAiming = false;

    public bool isCheckingStats = false;

    //public bool isCheckingInventory = false;

    //public GameObject inventory;

    public WeaponAnimationEvents animationEvents;

    // Новое
    
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

    // Этой штуке нечего здесь делать. Переместить!
    private InventoryController inventory;

    private void Start()
    {
        inventory = GameObject.Find("_PlayerCamera").GetComponent<InventoryController>();
        //HandleInventory(false);
        TryGetComponent<SCRIPT_ActiveWeapon>(out activeWeapon);
        isShooting = false;
        Equip(weapon);
        //animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    void LateUpdate()
    {
        CombatInput();                       // Проверка, нажата ли кнопка выстрела
        //SwitchWeapon();
        //muzzleFlame.FadeFlame();            // Угасание вспышки от выстрела
        //weapon.IsReloading();               // Блокировка стрельбы во время перезарядки
    }



    /// <summary>
    /// ////////
    /// </summary>
    public SCRIPT_Weapon weapon;
    void CombatInput()
    {
        
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
            }

            if (Input.GetButtonUp("Fire2"))
            {
                isAiming = false;
                animationController.SetBool("isAiming", isAiming);
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
            // Hide weapon in the holster
            GetComponent<SCRIPT_ActiveWeapon>().ToggleActiveWeapon();
        }
    }

    public void Equip(SCRIPT_Weapon weaponToEquip)
    {
        isHolstered = false;
        weapon = weaponToEquip;/*.GetComponent<SCRIPT_Weapon>();*/
        muzzleFlame = weaponToEquip.GetComponentInParent<SCRIPT_MuzzleFlame>();
        ammoShells = weaponToEquip.GetComponentInParent<SCRIPT_AmmoShells>();
    }
}
