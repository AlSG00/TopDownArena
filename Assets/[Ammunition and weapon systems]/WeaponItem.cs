using UnityEngine;

public class WeaponItem : MonoBehaviour, SCRIPT_IItem, IEquipable
{
    public bool isUsable { get; set; }
    //public InventoryController.BindSlot[] availableSlots { get; set; }

    //public InventoryController.BindSlot[] availableSlots;

    public Weapon weapon;
    private Weapon _equippedPrefab;
    //private bool _isUsing = false;

    //public delegate void UseWeaponAction(Weapon weapon, bool alreadyUsing);
    //public static event UseWeaponAction OnUseWeapon;

    public static event System.Action<Weapon> OnUseWeapon;


    public void Use()
    {
        //if (_isUsing == false)
        //{
        //    _isUsing = true;
        //    Debug.Log($"<color=yellow>Using [{gameObject.name}] weapon...</color>");
        //    OnUseWeapon?.Invoke(_equippedPrefab);
        //}
        //else
        //{
        //    _isUsing = false;
        //}

        //_isUsing = true;
        Debug.Log($"<color=yellow>Using [{gameObject.name}] weapon...</color>");
        OnUseWeapon?.Invoke(_equippedPrefab);

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
