using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;

public class SCRIPT_ActiveWeapon : MonoBehaviour
{
    public Rig handIk;
    SCRIPT_Weapon weapon;
    public Transform weaponParent;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;

    public Animator rigController;
  //  public AnimatorOverrideController animatorOverride;

    private void Start()
    {
        rigController = GetComponent<Animator>();
     //   animatorOverride = playerAnimator.runtimeAnimatorController as AnimatorOverrideController;

        SCRIPT_Weapon equippedWeapon = GetComponentInChildren<SCRIPT_Weapon>();
        if (equippedWeapon)
        {
            Equip(equippedWeapon);
        }
    }

    private void Update()
    {
        if (!weapon)
        {
           // playerAnimator.SetLayerWeight(1, 0.0f);
            handIk.weight = 0.0f;
        }
    }

    public void Equip(SCRIPT_Weapon weaponToEquip)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = weaponToEquip;
        handIk.weight = 1.0f;
   //     playerAnimator.SetLayerWeight(1, 1.0f);
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        //rigController.Play("ANIM_Equip_" + weapon.weaponName);
      //  Invoke(nameof(SetAnimationDelayer), 0.001f);
    }

    //void SetAnimationDelayer()
    //{
    //    animatorOverride["ANIM_Weapon_Idle"] = weapon.weaponIdleAnimation;
    //    animatorOverride["ANIM_Weapon_Aim"] = weapon.weaponAimAnimation;
    //}

    //[ContextMenu("Save weapon idle pose")]
    //void SaveWeaponIdlePose()
    //{
    //    GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
    //    recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
    //    recorder.TakeSnapshot(0.0f);
    //    recorder.SaveToClip(weapon.weaponIdleAnimation);
    //}

    //[ContextMenu("Save weapon aim pose")]
    //void SaveWeaponAimPose()
    //{
    //    GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
    //    recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
    //    recorder.TakeSnapshot(0.0f);
    //    recorder.SaveToClip(weapon.weaponAimAnimation);
    //}
}

