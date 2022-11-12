using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
//using UnityEditor.Animations;

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

    public enum WeaponSlot
    {
        Holster = 0,
        Belt = 1,
        Back = 2
    }

    private void Start()
    {
        //  rigController = GetComponent<Animator>();
        playerShooting = GetComponent<Player_Shooting>();
        SCRIPT_Weapon equippedWeapon = GetComponentInChildren<SCRIPT_Weapon>();
        activeWeaponIndex = (int)equippedWeapon.WeaponSlot;
        if (equippedWeapon)
        {
            Equip(equippedWeapon);
        }
    }

    private void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);

        if (!weapon.isReloading)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Holster unavailable");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetActiveWeapon(SCRIPT_ActiveWeapon.WeaponSlot.Belt);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetActiveWeapon(SCRIPT_ActiveWeapon.WeaponSlot.Back);
            }
        }
    }

    public void Equip(SCRIPT_Weapon weaponToEquip)
    {
        GetComponent<Player_Shooting>().Equip(weaponToEquip);
        int weaponSlotIndex = (int)weaponToEquip.WeaponSlot;
        //if (sdfsdf[weaponSlotIndex])
        //{
        //    Destroy(equippedWeapon[weaponSlotIndex].gameObject);
        //}



        //var weapon = GetWeapon(weaponSlotIndex);

        //if (allWeapons[weaponSlotIndex] &&
        //    allWeapons[weaponSlotIndex].weaponName == weaponToEquip.weaponName)
        //{
        //    //    Destroy(allWeapons[weaponSlotIndex].gameObject);
        //    Debug.Log("Already have this weapon");
        //    return;
        //}
        //allWeapons[weaponSlotIndex] = weaponToEquip;



        var weapon = GetWeapon(weaponSlotIndex);

        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = weaponToEquip;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;



        //else if (rigController.GetBool("isHolstering") == false)
        //{
        //    rigController.SetBool("isHolstering", true);
        //}
        //equippedWeapon[weaponSlotIndex] = weaponToEquip;
        ////  handIk.weight = 1.0f;
        ////     playerAnimator.SetLayerWeight(1, 1.0f);
        ////weapon.transform.parent = weaponSlots[weaponSlotIndex];
        //equippedWeapon[weaponSlotIndex].transform.parent = weaponParent;
        //equippedWeapon[weaponSlotIndex].transform.localPosition = Vector3.zero;
        //equippedWeapon[weaponSlotIndex].transform.localRotation = Quaternion.identity;

        // equippedWeapon[weaponSlotIndex] = weaponToEquip;
        //  handIk.weight = 1.0f;
        //     playerAnimator.SetLayerWeight(1, 1.0f);
        //weapon.transform.parent = weaponSlots[weaponSlotIndex];



        //weaponToEquip.transform.parent = weaponParent;
        //weaponToEquip.transform.localPosition = Vector3.zero;
        //weaponToEquip.transform.localRotation = Quaternion.identity;



        // StartCoroutine("PlayAnimTest", equippedWeapon[weaponSlotIndex]);
        //rigController.Play($"ANIM_Equip_{weapon.weaponName}");
        // rigController.SetTrigger("equip");
        //SetActiveWeapon(weaponSlotIndex);
        equippedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(weaponToEquip.WeaponSlot);
        //activeWeaponIndex = weaponSlotIndex;
    }

    public void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        if (!isSwitchingWeapon)
        {
            isSwitchingWeapon = true;
            int holsterIndex = activeWeaponIndex;
            int activateIndex = (int)weaponSlot;

            if (holsterIndex == activateIndex)
            {
                Debug.Log("Already equipped");
                isSwitchingWeapon = false;
                return;
            }

            StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
        }
    }

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

