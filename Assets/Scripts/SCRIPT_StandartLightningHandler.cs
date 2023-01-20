using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_StandartLightningHandler : MonoBehaviour
{
    [SerializeField] private Light _lightOrigin;

    public float targetIntensity = 1f;
    public float intensityIncreasing = 0.1f;
    public float intensityDecreasing = 0.1f;
    public float activatingDelay = 0f;
    public float deactivatingDelay = 0f;

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
        if (_lightOrigin.intensity != 0 && 
            _lightOrigin.intensity != targetIntensity)
        {
            targetIntensity = _lightOrigin.intensity;
        }
    }

    private void TurnOn()
    {
        StartCoroutine(SmoothFading(false));
    }

    private void TurnOff()
    {
        StartCoroutine(SmoothFading(true));
    }

    private IEnumerator SmoothFading(bool isDay)
    {
        if (!isDay)
        {
            yield return new WaitForSeconds(activatingDelay);
            while (_lightOrigin.intensity < targetIntensity)
            {
                yield return _lightOrigin.intensity += intensityIncreasing;
            }
            _lightOrigin.intensity = targetIntensity;
        }
        else
        {
            yield return new WaitForSeconds(deactivatingDelay);
            while (_lightOrigin.intensity > 0)
            {
                yield return _lightOrigin.intensity -= intensityDecreasing;
            }
            _lightOrigin.intensity = 0;
        }
    }
}
