using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Flashlight : MonoBehaviour
{

    [SerializeField]
    private Light flashlight;
    [SerializeField]
    private Light ambient;
    [SerializeField]
    private AudioSource flashlightAudioSource;
    [SerializeField]
    private AudioClip turnOnSound;
    [SerializeField]
    private AudioClip turnOffSound;

    // Update is called once per frame
    void Update()
    {
        UseFlashlight();
    }

    private void UseFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && flashlight.enabled == false)
        {
            flashlight.enabled = true;
            flashlightAudioSource.PlayOneShot(turnOnSound);
        }
        else if (Input.GetKeyDown(KeyCode.F) && flashlight.enabled == true)
        {
            flashlight.enabled = false;
            flashlightAudioSource.PlayOneShot(turnOffSound);
        }
    }
}
