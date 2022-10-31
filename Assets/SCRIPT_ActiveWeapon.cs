using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;

public class SCRIPT_ActiveWeapon : MonoBehaviour
{
    //public Rig handIk;
    public SCRIPT_Weapon[] equippedWeapon;

    int activeWeaponIndex;

    public Transform[] weaponSlots;
    public Transform weaponParent;
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

        SCRIPT_Weapon equippedWeapon = GetComponentInChildren<SCRIPT_Weapon>();
        if (equippedWeapon)
        {
            Equip(equippedWeapon);
        }
    }

    public void Equip(SCRIPT_Weapon weaponToEquip)
    {
        int weaponSlotIndex = (int)weaponToEquip.WeaponSlot;
        if (equippedWeapon[weaponSlotIndex])
        {
            Destroy(equippedWeapon[weaponSlotIndex].gameObject);
        }
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
        weaponToEquip.transform.parent = weaponParent;
        weaponToEquip.transform.localPosition = Vector3.zero;
        weaponToEquip.transform.localRotation = Quaternion.identity;

        // StartCoroutine("PlayAnimTest", equippedWeapon[weaponSlotIndex]);
        rigController.Play($"ANIM_Equip_{equippedWeapon[weaponSlotIndex].weaponName}");
        // rigController.SetTrigger("equip");
       // SetActiveWeapon(weaponSlotIndex);
    }

    private void SetActiveWeapon(int weaponSlotIndex)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = weaponSlotIndex;
        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    private IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    private IEnumerator HolsterWeapon(int holsterIndex)
    {
        //rigController.Play($"ANIM_Equip_{weapon.weaponName}");
        var weapon = GetWeapon(holsterIndex);
        if (weapon)
        {
            rigController.SetBool("isHolstered", true);
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    private IEnumerator ActivateWeapon(int activateIndex)
    {
        //rigController.Play($"ANIM_Equip_{weapon.weaponName}");
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
        }
    }

    private SCRIPT_Weapon GetWeapon(int index)
    {
        if (index < 0 || index >= equippedWeapon.Length)
        {
            return null;
        }

        return equippedWeapon[index];
    }
}

