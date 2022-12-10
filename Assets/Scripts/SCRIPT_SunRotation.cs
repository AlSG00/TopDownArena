using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_SunRotation : MonoBehaviour
{
    public int defaultStartTime = 0;
    public float dayCycleDuration = 1;
    public float nightCycleDuration = 1;
    public float switchToNightIntensitySpeed = 0.005f;
    public float switchToDayIntensitySpeed = 0.005f;
    public Animator sunAnimationController;

    public delegate void DaytimeAction();
    public static event DaytimeAction DayStarted;
    public static event DaytimeAction NightStarted;

    private Light sun;
    private void Start()
    {
        RenderSettings.ambientIntensity = 0;
        sunAnimationController.speed = 1 / dayCycleDuration;
        sun = GetComponentInChildren<Light>();
        sunAnimationController.Play("ANIM_Sun", 0, 0);
    }

    public void SetDay()
    {
        sunAnimationController.speed = 1 / dayCycleDuration;
        StartCoroutine(SetDayAmbient());
        Debug.Log("Day");
        DayStarted?.Invoke();
    }

    public void SetNight()
    {
        sunAnimationController.speed = 1 / nightCycleDuration;
        StartCoroutine(SetNightAmbient());
        Debug.Log("Night");
        NightStarted?.Invoke();
    }

    private IEnumerator SetDayAmbient()
    {
        while (RenderSettings.ambientIntensity < 1)
        {
            yield return RenderSettings.ambientIntensity += switchToDayIntensitySpeed;
        }
        RenderSettings.ambientIntensity = 1;
    }

    private IEnumerator SetNightAmbient()
    {
        while (RenderSettings.ambientIntensity > 0)
        {
            yield return RenderSettings.ambientIntensity -= switchToNightIntensitySpeed;
        }
        RenderSettings.ambientIntensity = 0; 
    }
}
