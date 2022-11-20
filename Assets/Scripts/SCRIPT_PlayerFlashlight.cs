using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerFlashlight : MonoBehaviour
{

    [SerializeField] private Light _flashlight;
    //[SerializeField] private Light _ambient;
    [SerializeField] private AudioSource _flashlightAudioSource;
    [SerializeField] private AudioClip _turnOnSound;
    [SerializeField] private AudioClip _turnOffSound;
    public float chargeCapacity = 100f;
    public float dischargeSpeed = 0.01f;
    public float regenerationSpeed = 0f;
    private bool _isActive = false;
    public float chargeRemaining;
    public bool consoleDebug = false;
    private void Start()
    {
        if (!_isActive)
        {
            _flashlight.intensity = 0;
        }
        else
        {
            _flashlight.intensity = 4.43f;
        }

        chargeRemaining = chargeCapacity;
    }

    private void FixedUpdate()
    {
        UpdateFlashlightCharge();
    }

    void Update()
    {
        UseFlashlight();
        
    }

    private void UseFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && _isActive == false)
        {
            TurnOn();
        }
        else if (Input.GetKeyDown(KeyCode.F) && _isActive == true)
        {
            TurnOff();
        }
    }

    private void UpdateFlashlightCharge()
    {
        if (_isActive)
        {
            chargeRemaining -= dischargeSpeed;
        }
        else
        {
            chargeRemaining += regenerationSpeed;
            if (chargeRemaining > chargeCapacity)
            {
                chargeRemaining = chargeCapacity;
            }
        }

        if (chargeRemaining <= 0)
        {
            chargeRemaining = 0;
            TurnOff();
        }

        if (consoleDebug)
        {
            Debug.Log($"Flashlight: {chargeRemaining}");
        }
    }

    private void TurnOn()
    {
        if (!_isActive && chargeRemaining > 0)
        {
            _isActive = true;
            _flashlight.intensity = 4.43f;
            _flashlightAudioSource.PlayOneShot(_turnOnSound);
        }
    }

    private void TurnOff()
    {
        if (_isActive)
        {
            _isActive = false;
            _flashlight.intensity = 0;
            _flashlightAudioSource.PlayOneShot(_turnOffSound);
        }
    }
}
