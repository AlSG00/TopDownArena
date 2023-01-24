using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_DoubleColorLightningHandler : MonoBehaviour
{
    [SerializeField] private SCRIPT_SunRotation _sun;

    [SerializeField] private Light[] _lightOrigin;
   
    public Color DaylightningColor;
    public Color NightlightningColor;

    public float daylightIntensity = 1f;
    public float nightlightIntensity = 1f;
    public float intensityIncreasingSpeed = 0.1f;
    public float intensityDecreasingSpeed = 0.1f;
    public float activatingDelay = 0f;
    public float deactivatingDelay = 0f;

    private void OnEnable()
    {
        SCRIPT_SunRotation.DayStarted += SetDayLightning;
        SCRIPT_SunRotation.NightStarted += SetNightLightning;
    }

    private void OnDisable()
    {
        SCRIPT_SunRotation.DayStarted -= SetDayLightning;
        SCRIPT_SunRotation.NightStarted -= SetNightLightning;
    }

    private void Start()
    {
        _sun = GameObject.Find("Sun").GetComponent<SCRIPT_SunRotation>();
        SetLighthningStartColor();
    }

    private void SetDayLightning()
    {
        if (_lightOrigin != null &&
            _lightOrigin.Length != 0)
        {
            for (int i = 0; i < _lightOrigin.Length; i++)
            {
                StartCoroutine(SwapLightColorRoutine(DaylightningColor, _lightOrigin[i], daylightIntensity));
            }
        }
    }

    private void SetNightLightning()
    {
        if (_lightOrigin != null &&
            _lightOrigin.Length != 0)
        {
            for (int i = 0; i < _lightOrigin.Length; i++)
            {
                StartCoroutine(SwapLightColorRoutine(NightlightningColor, _lightOrigin[i], nightlightIntensity));
            }
        }
    }

    private IEnumerator SwapLightColorRoutine(Color lightColor, Light lightOrigin, float targetIntensity)
    {
        yield return new WaitForSeconds(deactivatingDelay);

        while (lightOrigin.intensity > 0)
        {
            yield return lightOrigin.intensity -= intensityDecreasingSpeed;
        }

        yield return lightOrigin.intensity = 0;

        lightOrigin.color = lightColor;

        yield return new WaitForSeconds(activatingDelay);

        while (lightOrigin.intensity < targetIntensity)
        {
            yield return lightOrigin.intensity += intensityIncreasingSpeed;
        }

        yield return lightOrigin.intensity = targetIntensity;
    }

    private void SetLighthningStartColor()
    {
        for (int i = 0; i < _lightOrigin.Length; i++)
        {
            if (_sun.isDay)
            {
                _lightOrigin[i].color = DaylightningColor;
                _lightOrigin[i].intensity = daylightIntensity;
            }
            else
            {
                _lightOrigin[i].color = NightlightningColor;
                _lightOrigin[i].intensity = nightlightIntensity;
            }
        }
    }
}
