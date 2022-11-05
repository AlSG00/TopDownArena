using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : UnityEvent<string>
{

}

public class WeaponAnimationEvents : MonoBehaviour
{
    public AnimationEvent WeaponAnimationEvent = new AnimationEvent();
    public void OnAnimationEvent(string eventName)
    {
        Debug.Log("Did it work?");
        WeaponAnimationEvent.Invoke(eventName);
    }

    public void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "EjectMag":
                Debug.Log("EjectMag");
                weapon.audioSource.PlayOneShot(weapon.ejectMagSound);
                magazineHand = Instantiate(weapon.magazine, leftHand, true);
                weapon.magazine.SetActive(false);
                break;
            case "PutInMag":
                Debug.Log("PutInMag");
                weapon.audioSource.PlayOneShot(weapon.putMagSound);
                magazineHand.SetActive(false);
                break;
            case "GetNewMag":
                Debug.Log("GetNewMag");
                weapon.audioSource.PlayOneShot(weapon.pullOutMagSound);
                magazineHand.SetActive(false);
                break;
            case "InsertNewMag":
                Debug.Log("InsertNewMag");
                weapon.audioSource.PlayOneShot(weapon.InsertMagSound);
                weapon.magazine.SetActive(true);
                Destroy(magazineHand);
                break;
        }
    }
}
