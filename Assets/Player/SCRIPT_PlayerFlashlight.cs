using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SCRIPT_PlayerFlashlight : MonoBehaviour
{
    [Header("Light source")]
    [SerializeField] private Light _flashlight;

    [Header("Sound")]
    [SerializeField] private AudioSource _flashlightAudioSource;
    [SerializeField] private AudioClip _turnOnSound;
    [SerializeField] private AudioClip _turnOffSound;

    [Header("UI")]
    [SerializeField] private Image FlashlightChargeRadialBar;
    [SerializeField] private float _radialBarAppearanceSpeed = 1f;
    [SerializeField] private float _radialBarDissapearingSpeed = 1f;
    [SerializeField] private float _hideDelay = 1f;
    private float _hideTime;

    [Header("Parameters")]
    public float chargeCapacity = 100f;
    public float dischargeSpeed = 0.01f;
    public float regenerationSpeed = 0f;
    [HideInInspector] public float chargeRemaining;
    private bool _isActive = false;
    
    [Space]
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
        StartCoroutine(HideRadialBar());
        chargeRemaining = chargeCapacity;
        FlashlightChargeRadialBar.fillAmount = chargeRemaining;
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

            if (regenerationSpeed != 0 &&
                chargeRemaining != chargeCapacity)
            {
                _hideTime = Time.time;
            }
        }

        if (chargeRemaining <= 0)
        {
            chargeRemaining = 0;
            TurnOff();
        }

        FlashlightChargeRadialBar.fillAmount = chargeRemaining / 100f;

        if (consoleDebug)
        {
            Debug.Log($"Flashlight: {chargeRemaining} : {chargeRemaining / 100f}");
        }

        if (_isActive == false)
        {
            if (_hideTime + _hideDelay <= Time.time)
            {
                _hideTime = Time.time;

                StopAllCoroutines();
                StartCoroutine(HideRadialBar());
            }
        }
    }

    private IEnumerator HideRadialBar()
    {
        while (FlashlightChargeRadialBar.color.a > 0)
        {
            yield return FlashlightChargeRadialBar.color = new Color(
                FlashlightChargeRadialBar.color.r,
                FlashlightChargeRadialBar.color.g,
                FlashlightChargeRadialBar.color.b,
                FlashlightChargeRadialBar.color.a - _radialBarDissapearingSpeed
                );
        }

        yield return FlashlightChargeRadialBar.color = new Color(
                FlashlightChargeRadialBar.color.r,
                FlashlightChargeRadialBar.color.g,
                FlashlightChargeRadialBar.color.b,
                0f
                );
    }

    private IEnumerator ShowRadialBar()
    {
        while (FlashlightChargeRadialBar.color.a < 1)
        {
            yield return FlashlightChargeRadialBar.color = new Color(
                FlashlightChargeRadialBar.color.r,
                FlashlightChargeRadialBar.color.g,
                FlashlightChargeRadialBar.color.b,
                FlashlightChargeRadialBar.color.a + _radialBarAppearanceSpeed
                );
        }

        yield return FlashlightChargeRadialBar.color = new Color(
                FlashlightChargeRadialBar.color.r,
                FlashlightChargeRadialBar.color.g,
                FlashlightChargeRadialBar.color.b,
                1f
                );
    }

    private void TurnOn()
    {
        if (!_isActive && chargeRemaining > 0)
        {
            _isActive = true;
            _flashlight.intensity = 4.43f;
            _flashlightAudioSource.PlayOneShot(_turnOnSound);

            StopAllCoroutines();
            StartCoroutine(ShowRadialBar());
        }
    }

    private void TurnOff()
    {
        if (_isActive)
        {
            _isActive = false;
            _flashlight.intensity = 0;
            _flashlightAudioSource.PlayOneShot(_turnOffSound);

            _hideTime = Time.time;
        }
    }
}
