using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

// TODO: Rework it... With heist!
public class SCRIPT_ActiveWeapon : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    public Transform ActiveWeaponPivot;
    public Animator rigController;
    private Weapon _activeWeapon;

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
        SCRIPT_Weapon equippedWeapon = GetComponentInChildren<SCRIPT_Weapon>();
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
    
    private async void TestHolsterWeapon()
    {
        rigController.Play($"Weapon_Holster_{_activeWeapon.bindedSlotPivot.name}");

        do
        {
            await Task.Delay(10);
        }
        while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        SetWeaponParent(_activeWeapon.bindedSlotPivot);

        rigController.Play($"Weapon_Holster_On{_activeWeapon.bindedSlotName}");
        Debug.Log($"<color=yellow>Holstered [{_activeWeapon.gameObject.name}]</color>");

        похоже тут не проигрывается анимация weapon unarmed после убирания оружия
        _activeWeapon = null;
    }    

    private async void TestDrawWeapon(Weapon weaponToDraw)
    {
        rigController.SetBool("[temp]", false);
        if (weaponToDraw == null)
        {
            throw new System.Exception("<color=red>Weapon to draw is null</color>");
        }

        _activeWeapon = weaponToDraw;

        // PROBLEM: async operation will not stop after destroying script,
        // so potentially this function can be a problem in the future.
        Debug.Log($"Weapon_Draw_From{_activeWeapon.bindedSlotName}");
        rigController.Play($"Weapon_Draw_From{_activeWeapon.bindedSlotName}");
        do
        {
            await Task.Delay(10);
        }
        while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        SetWeaponParent(ActiveWeaponPivot);
        Debug.Log($"Weapon_Draw_{weaponToDraw.bindedSlotPivot.name}");
        rigController.Play($"Weapon_Draw_{weaponToDraw.bindedSlotPivot.name}");
        GetComponent<InputManager>().Equip_2(_activeWeapon);

        Debug.Log($"<color=yellow>Drawed [{_activeWeapon.gameObject.name}]</color>");
    }

    private void TestRemoveWeapon(Weapon weaponToRemove)
    {
        SetWeaponParent(_activeWeapon.bindedSlotPivot);
        _activeWeapon = null;
    }

    private void SetWeaponParent(Transform parent)
    {
        _activeWeapon.transform.SetParent(parent, false);
        _activeWeapon.transform.localPosition = Vector3.zero;
        _activeWeapon.transform.localRotation = Quaternion.identity;
    }
}

