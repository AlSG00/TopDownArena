using System.Collections;
using UnityEngine;

// TODO: Rework it... With heist!
public class SCRIPT_ActiveWeapon : MonoBehaviour
{
    //public Rig handIk;
    public SCRIPT_Weapon[] equippedWeapons;
    public bool isSwitchingWeapon = false;
    public bool isReloading = false;
    int activeWeaponIndex;
    private Player_Shooting playerShooting;
    public Transform[] weaponSlots;
    //public Transform weaponParent;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;

    public Animator rigController;

    private Weapon _activeWeapon;

    //public enum WeaponSlot
    //{
    //    Holster = 0,
    //    Belt = 1,
    //    Back = 2
    //}

    private void OnEnable()
    {
        WeaponItem.OnUseWeapon += TestActivateWeapon;
    }

    private void OnDisable()
    {
        WeaponItem.OnUseWeapon -= TestActivateWeapon;
    }

    private void Start()
    {
        //  rigController = GetComponent<Animator>();
        playerShooting = GetComponent<Player_Shooting>();
        SCRIPT_Weapon equippedWeapon = GetComponentInChildren<SCRIPT_Weapon>();
       // activeWeaponIndex = (int)equippedWeapon.WeaponSlot;

        //if (equippedWeapon)
        //{
        //    Equip(equippedWeapon);
        //}
    }

    //private void Update()
    //{
    //    var weapon = GetWeapon(activeWeaponIndex);

    //    // Move to InputController
    //    if (weapon != null && !weapon.isReloading)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Alpha1))
    //        {
    //            Debug.Log("Holster unavailable");
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Alpha2))
    //        {
    //            SetActiveWeapon(SCRIPT_ActiveWeapon.WeaponSlot.Belt);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Alpha3))
    //        {
    //            SetActiveWeapon(SCRIPT_ActiveWeapon.WeaponSlot.Back);
    //        }
    //    }
    //}









    // Don't delete yet

    //public void Equip(SCRIPT_Weapon weaponToEquip)
    //{
    //    GetComponent<Player_Shooting>().Equip(weaponToEquip);
    //    int weaponSlotIndex = (int)weaponToEquip.WeaponSlot;

    //    var weapon = GetWeapon(weaponSlotIndex);

    //    if (weapon)
    //    {
    //        Destroy(weapon.gameObject);
    //    }

    //    weapon = weaponToEquip;
    //    weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
    //    weapon.transform.localPosition = Vector3.zero;
    //    weapon.transform.localRotation = Quaternion.identity;

    //    equippedWeapons[weaponSlotIndex] = weapon;

    //    SetActiveWeapon(weaponToEquip.WeaponSlot);
    //}

    //public void SetActiveWeapon(WeaponSlot weaponSlot)
    //{
    //    if (!isSwitchingWeapon)
    //    {
    //        isSwitchingWeapon = true;
    //        int holsterIndex = activeWeaponIndex;
    //        int activateIndex = (int)weaponSlot;

    //        if (holsterIndex == activateIndex)
    //        {
    //            Debug.Log("Already equipped");
    //            isSwitchingWeapon = false;
    //            return;
    //        }

    //        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    //    }
    //}

    private IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;

    }

    private IEnumerator HolsterWeapon(int holsterIndex)
    {
        playerShooting.isHolstered = true;
        var weapon = GetWeapon(holsterIndex);
        if (weapon)
        {
            rigController.SetBool("isHolstered", true);
            do
            {
                yield return new WaitForSeconds(0.3f); 
            }
            while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    private IEnumerator ActivateWeapon(int activateIndex)
    {
        var weapon = GetWeapon(activateIndex);
        if (weapon)
        {
            
            rigController.SetBool("isHolstered", false);
            rigController.Play($"ANIM_Equip_{weapon.weaponName}");
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            
            isSwitchingWeapon = false;
            GetComponent<Player_Shooting>().Equip(weapon);
            playerShooting.isHolstered = false;
        }
    }

    private void TestActivateWeapon(Weapon weaponToActivate)
    {
         
        if ((_activeWeapon != null) && (weaponToActivate == _activeWeapon))
        {
            Debug.Log($"<color=yellow>Start weapon holstering...</color>");
            TestHolsterWeapon();
            
        }
        else if ((_activeWeapon != null) && (weaponToActivate != _activeWeapon))
        {
            Debug.Log($"<color=yellow>Start weapon switching...</color>");
            TestHolsterWeapon();
            TestDrawWeapon(weaponToActivate);
        }
        else
        {
            Debug.Log($"<color=yellow>Start weapon drawing...</color>");
            TestDrawWeapon(weaponToActivate);
        }
    }
    
    private void TestHolsterWeapon()
    {
        playerShooting.isHolstered = true;
        rigController.SetBool("isHolstered", true);
        Debug.Log($"<color=yellow>Holstered [{_activeWeapon.gameObject.name}]</color>");
        _activeWeapon = null;
    }    

    private void TestDrawWeapon(Weapon weaponToDraw)
    {
        _activeWeapon = weaponToDraw;
        rigController.SetBool("isHolstered", false);
        rigController.Play($"ANIM_Equip_{_activeWeapon.weaponName}");
        isSwitchingWeapon = false;
        GetComponent<Player_Shooting>().Equip_2(_activeWeapon);
        playerShooting.isHolstered = false;
        Debug.Log($"<color=yellow>Drawed [{_activeWeapon.gameObject.name}]</color>");
    }



    private SCRIPT_Weapon GetWeapon(int index)
    {
        if (index < 0 || index >= equippedWeapons.Length)
        {
            return null;
        }

        return equippedWeapons[index];
    }

    public void ToggleActiveWeapon()
    {
        bool isHolstered = rigController.GetBool("isHolstered");
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    public SCRIPT_Weapon GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }
}

