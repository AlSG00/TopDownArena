using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_SunRotation : MonoBehaviour
{
    [Range(0,1)] public float defaultStartTime = 0f;
    public float dayCycleDuration = 1f;
    public float nightCycleDuration = 1f;
    public float switchToNightIntensitySpeed = 0.005f;
    public float switchToDayIntensitySpeed = 0.005f;
    public Animator sunAnimationController;

    public delegate void DaytimeAction();
    public static event DaytimeAction DayStarted;
    public static event DaytimeAction NightStarted;

    private Light sun;

    public bool isDay = false;

    private void Start()
    {
        RenderSettings.ambientIntensity = 0;
        sunAnimationController.speed = 1 / dayCycleDuration;
        sun = GetComponentInChildren<Light>();
        sunAnimationController.Play("ANIM_Sun", 0, defaultStartTime);

        if (defaultStartTime >= 0.5)
        {
            isDay = false;
        }
        else
        {
            isDay = true;
        }

        AnimationState state;
        AnimatorClipInfo clipInfo;
    }

    public void SetDay()
    {
        sunAnimationController.speed = 1 / dayCycleDuration;
        StartCoroutine(SetDayAmbient());
        Debug.Log("Day");
        isDay = true;
        DayStarted?.Invoke();
    }

    public void SetNight()
    {
        sunAnimationController.speed = 1 / nightCycleDuration;
        StartCoroutine(SetNightAmbient());
        Debug.Log("Night");
        isDay = false;
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
