using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_SunRotation : MonoBehaviour
{
    public int defaultStartTime = 0;
    public float dayCycleDuration = 1;
    public float nightCycleDuration = 1;
    public Animator sunAnimationController;
    private float rotationSpeedAtDay;
    private float rotationSpeedAtNight;

    private bool isNight = false;
    private void Start()
    {
        CalñulateSunRotationSpeed();
        sunAnimationController.speed = 1 / dayCycleDuration;

        //if (defaultStartTime != 0)
        //{
        //    sunAnimationController.Play("ANIM_Sun", 0, 1 / (dayCycleDuration + nightCycleDuration) * defaultStartTime);
        //}
        sunAnimationController.Play("ANIM_Sun", 0, 0);
    }

    private void Update()
    {
        //Debug.Log($"Euler {gameObject.transform.eulerAngles.x % 360}");
        //if (!isNight)
        //{
        //    gameObject.transform.Rotate(Vector3.right, rotationSpeedAtDay * Time.deltaTime, Space.World);
        //}
        //else
        //{
        //    gameObject.transform.Rotate(Vector3.right, rotationSpeedAtNight * Time.deltaTime);
        //}
      //  CheckNight();

      //  if ()
    }

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
        Debug.Log("Day");
    }

    public void SetNight()
    {
        sunAnimationController.speed = 1 / nightCycleDuration;
        Debug.Log("Night");
    }

    public void FullCycleMark()
    {
        Debug.Log("Full day");
    }
}
