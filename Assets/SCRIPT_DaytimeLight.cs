using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_DaytimeLight : MonoBehaviour
{
    public bool IsBroken = false;
    public float targetIntensity = 1f;
    public float glowIncreasing = 0.1f;
    public float glowFading = 0.1f;
    private Light _lightOrigin;

    private void OnEnable()
    {
        SCRIPT_SunRotation.DayStarted += TurnOff;
        SCRIPT_SunRotation.NightStarted += TurnOn;
    }

    private void OnDisable()
    {
        SCRIPT_SunRotation.DayStarted -= TurnOff;
        SCRIPT_SunRotation.NightStarted -= TurnOn;
    }

    private void Start()
    {
        _lightOrigin = GetComponent<Light>();
        if (_lightOrigin.intensity != 0 && 
            _lightOrigin.intensity != targetIntensity)
        {
            targetIntensity = _lightOrigin.intensity;
        }
    }

    private void TurnOn()
    {
        //_lightOrigin.intensity = 1;
        StartCoroutine(SmoothFading(false));
    }

    private void TurnOff()
    {
        //_lightOrigin.intensity = 0;
        StartCoroutine(SmoothFading(true));
    }

    private IEnumerator SmoothFading(bool isDay)
    {
        if (!isDay)
        {
            while (_lightOrigin.intensity < targetIntensity)
            {
                yield return _lightOrigin.intensity += glowIncreasing;
            }
            _lightOrigin.intensity = targetIntensity;
        }
        else
        {
            while (_lightOrigin.intensity > 0)
            {
                yield return _lightOrigin.intensity -= glowFading;
            }
            _lightOrigin.intensity = 0;
        }
    }
}
