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

    private void Start()
    {
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

        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
    }

    [ContextMenu("Save weapon pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponAnimation);
    }
}

