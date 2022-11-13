using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_SunRotation : MonoBehaviour
{
    public int defaultStartTime = 0;
    public float dayCycleDuration = 1;
    public float nightCycleDuration = 1;
    public float switchToNightLightningSpeed = 0.005f;
    public float switchToDayLightningSpeed = 0.005f;
    public Animator sunAnimationController;
    private float rotationSpeedAtDay;
    private float rotationSpeedAtNight;

    private bool isNight = false;

    private bool setDay = false;
    private void Start()
    {
        RenderSettings.ambientIntensity = 0;
        CalñulateSunRotationSpeed();
        sunAnimationController.speed = 1 / dayCycleDuration;

        //if (defaultStartTime != 0)
        //{
        //    sunAnimationController.Play("ANIM_Sun", 0, 1 / (dayCycleDuration + nightCycleDuration) * defaultStartTime);
        //}
        sunAnimationController.Play("ANIM_Sun", 0, 0);
    }

    //private void FixedUpdate()
    //{
    //    if (setDay)
    //    {
    //        RenderSettings.ambientIntensity = Mathf.Lerp(0f, 1f, 5);
    //        if (RenderSettings.ambientIntensity >= 1)
    //        {
    //            setDay = false;

    //        }
    //    }
    //}
    //private void CheckNight()
    //{
    //    if (gameObject.transform.eulerAngles.x > 90 && gameObject.transform.eulerAngles.x < 270)
    //    {
    //        Debug.Log("night");
    //    }
    //    else
    //    {
    //        Debug.Log("day");
    //    }


    //}

    public void CalñulateSunRotationSpeed()
    {

        //rotationSpeedAtDay = 360 / dayCycleDuration;
        //rotationSpeedAtNight = 360 / nightCycleDuration;
    }

    public void SetDay()
    {
        sunAnimationController.speed = 1 / dayCycleDuration;
        StartCoroutine(SetDayIntensity());
        Debug.Log("Day");
    }

    public void SetNight()
    {
        sunAnimationController.speed = 1 / nightCycleDuration;
        StartCoroutine(SetNightIntensity());
        Debug.Log("Night");
    }

    public void FullCycleMark()
    {
        Debug.Log("Full day");
        //RenderSettings.ambientIntensity = 1;
    }

    private IEnumerator SetDayIntensity()
    {
        while (RenderSettings.ambientIntensity < 1)
        {
            yield return RenderSettings.ambientIntensity += switchToDayLightningSpeed;
        }
        Mathf.Clamp(RenderSettings.ambientIntensity, 0, 1);
    }

    private IEnumerator SetNightIntensity()
    {
        while (RenderSettings.ambientIntensity > 0)
        {
            yield return RenderSettings.ambientIntensity -= switchToNightLightningSpeed;
        }
        Mathf.Clamp(RenderSettings.ambientIntensity, 0, 1);
    }
}
