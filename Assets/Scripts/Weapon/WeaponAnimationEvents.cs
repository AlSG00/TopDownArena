using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//public class AnimationEvent : UnityEvent<string>
//{

//}

/// <summary>
/// »венты дл€ перезар€дки оружи€ (пока только винтовка)
/// </summary>

public class WeaponAnimationEvents : MonoBehaviour
{
    public Transform leftHand;
    private GameObject magazineHand;
    public SCRIPT_ActiveWeapon activeWeapon;

    public void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "EjectMag":
                var weapon = activeWeapon.GetActiveWeapon();
                magazineHand = Instantiate(weapon.magazine, leftHand, true);
                weapon.magazine.SetActive(false);
                break;
            case "PutInMag":
                weapon = activeWeapon.GetActiveWeapon();
                magazineHand.SetActive(false);
                break;
            case "GetNewMag":
                magazineHand.SetActive(true);
                weapon = activeWeapon.GetActiveWeapon();
                
                break;
            case "InsertNewMag":
                weapon = activeWeapon.GetActiveWeapon();
                weapon.magazine.SetActive(true);
                Destroy(magazineHand);
                break;
        }
    }

    public void OnAnimationAudioEvent(string eventName)
    {
        switch (eventName)
        {
            case "EjectMag":
                var weapon = activeWeapon.GetActiveWeapon();
                weapon.audioSource.volume = 0.15f;
                weapon.audioSource.PlayOneShot(weapon.ejectMagSound);
                break;
            case "PutInMag":
                weapon = activeWeapon.GetActiveWeapon();
                weapon.audioSource.volume = 0.65f;
                weapon.audioSource.PlayOneShot(weapon.putMagSound);
                break;
            case "GetNewMag":
                weapon = activeWeapon.GetActiveWeapon();
                weapon.audioSource.PlayOneShot(weapon.pullOutMagSound);
                break;
            case "InsertNewMag":
                weapon = activeWeapon.GetActiveWeapon();
                weapon.audioSource.volume = 0.15f;
                weapon.audioSource.PlayOneShot(weapon.InsertMagSound);
                break;
        }
    }

    public void SetReloadingBool()
    {
        //activeWeapon.isReloading = !activeWeapon.isReloading;
        //var weapon = activeWeapon.GetActiveWeapon();
        //weapon.audioSource.volume = 0.5f;
    }
}
