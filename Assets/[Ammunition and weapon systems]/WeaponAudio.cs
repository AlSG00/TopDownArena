using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shotSound;
    public AudioClip[] reloadSounds; // For rifle it's 4 sounds to play when reloading

    public void PlayShotSound()
    {
        audioSource.PlayOneShot(shotSound);
    }

    public void PlayReloadSounds(int soundToPlay)
    {
        audioSource.PlayOneShot(reloadSounds[soundToPlay]);
    }
}
