using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour, SCRIPT_IItem, IEquipable
{
    public bool isUsable { get; set; }

    Weapon weapon;

    public void EquipModel(Transform itemSlotPivot)
    {
        Instantiate(weapon);

        weapon.transform.SetParent(itemSlotPivot, false);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
    }

    public void Use()
    {
        Debug.Log("<color=yellow>Using weapon...</color>");
        

        // Draw weapon

    }
}
