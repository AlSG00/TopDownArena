using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_DoubleColorLightningHandler : MonoBehaviour
{
    [SerializeField] private Light[] _lightOrigin;
    [SerializeField] private GameObject[] _lightMesh;

    public Color DaylightningColor;
    public Color NightlightningColor;

    public float daylightIntensity = 1f;
    public float nightlightIntensity = 1f;
    public float intensityIncreasing = 0.1f;
    public float intensityDecreasing = 0.1f;
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
        //if (_lightOrigin.intensity != 0 &&
        //    _lightOrigin.intensity != targetIntensity)
        //{
        //    targetIntensity = _lightOrigin.intensity;
        //}
    }

    private void SetDayLightning()
    {
        for (int i = 0; i < _lightOrigin.Length; i++)
        {
            StartCoroutine(SwapColorRoutine(DaylightningColor, _lightOrigin[i], daylightIntensity));
        }
    }

    private void SetNightLightning()
    {
        for (int i = 0; i < _lightOrigin.Length; i++)
        {
            StartCoroutine(SwapColorRoutine(NightlightningColor, _lightOrigin[i], nightlightIntensity));
        }
    }

    private IEnumerator SwapColorRoutine(Color lightColor, Light lightOrigin, float targetIntensity)
    {
        yield return new WaitForSeconds(deactivatingDelay);
        while (lightOrigin.intensity > 0)
        {
            yield return lightOrigin.intensity -= intensityDecreasing;
        }
        lightOrigin.intensity = 0;

        lightOrigin.color = lightColor;

        yield return new WaitForSeconds(activatingDelay);

        while (lightOrigin.intensity < targetIntensity)
        {
            yield return lightOrigin.intensity += intensityIncreasing;
        }
        lightOrigin.intensity = targetIntensity;
    }
}
