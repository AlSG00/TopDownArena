using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour, SCRIPT_IItem, IEquipable
{
    public bool isUsable { get; set; }

    public Weapon weapon;
    private Weapon _equippedPrefab;

    public static event System.Action<Weapon> OnUseWeapon;

    public void Use()
    {
        Debug.Log("<color=yellow>Using weapon...</color>");

        OnUseWeapon?.Invoke(_equippedPrefab);
        // Draw weapon

    }

    public void EquipModel(Transform itemSlotPivot)
    {
        _equippedPrefab = Instantiate(weapon);

        _equippedPrefab.transform.SetParent(itemSlotPivot, false);
        _equippedPrefab.transform.localPosition = Vector3.zero;
        _equippedPrefab.transform.localRotation = Quaternion.identity;
    }

    public void UnequipModel()
    {
        Destroy(_equippedPrefab.gameObject);
        _equippedPrefab = null;
    }
}
