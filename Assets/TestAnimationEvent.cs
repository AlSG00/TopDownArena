using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationEvent : MonoBehaviour
{
    //private bool isFinished = false;
    [SerializeField]
    private bool playOnce;
    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    private AudioClip clip;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!playOnce || !isFinished)
    //    if (other.CompareTag("Player"))
    //    {
    //            sound.PlayOneShot(clip);
    //    }
    //}

    private void Activate()
    {
        sound.PlayOneShot(clip);
    }
}
