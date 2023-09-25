using UnityEngine;

public class WeaponItem : MonoBehaviour, SCRIPT_IItem, IEquipable
{
    public bool isUsable { get; set; }

    public Weapon weapon;
    private Weapon _equippedPrefab;

    public static event System.Action<Weapon> OnUseWeapon;

    public void Use()
    {
        Debug.Log($"<color=yellow>Using [{gameObject.name}] weapon...</color>");
        OnUseWeapon?.Invoke(_equippedPrefab);
    }

    public void EquipModel(Transform itemSlotPivot, string slotName)
    {
        _equippedPrefab = Instantiate(weapon);
        _equippedPrefab.bindedSlotPivot = itemSlotPivot;
        _equippedPrefab.bindedSlotName = slotName;
        _equippedPrefab.transform.SetParent(_equippedPrefab.bindedSlotPivot, false);
        _equippedPrefab.transform.localPosition = Vector3.zero;
        _equippedPrefab.transform.localRotation = Quaternion.identity;
    }

    public void UnequipModel()
    {
        Destroy(_equippedPrefab.gameObject);
        _equippedPrefab = null;
    }

    public void ChangeEquippedSlot(Transform itemSlotPivot, string slotName)
    {
        _equippedPrefab.bindedSlotPivot = itemSlotPivot;
        _equippedPrefab.bindedSlotName = slotName;
        _equippedPrefab.transform.SetParent(_equippedPrefab.bindedSlotPivot, false);
        _equippedPrefab.transform.localPosition = Vector3.zero;
        _equippedPrefab.transform.localRotation = Quaternion.identity;
    }
}
