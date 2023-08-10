using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shotSound;
    
    public void PlayShotSound()
    {
        audioSource.PlayOneShot(shotSound);
    }
}
