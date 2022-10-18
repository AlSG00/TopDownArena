using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_CameraShake : MonoBehaviour
{
    public float positionOffsetX;
    public float positionOffsetY;
    public float rotationOffsetX;
    public float rotationOffsetY;
    public float positionOffsetMagnitude;
    public float rotationOffsetMagnitude;

    //Vector3 originalPosition;
    //Quaternion originalRotation;

    //private void Awake()
    //{
    //    originalPosition = transform.localPosition;
    //    originalRotation = transform.localRotation;
    //    originalRotation.x = 90;
    //}

    //private void FixedUpdate()
    //{
    //    //gameObject.transform.localPosition = new Vector3(
    //    //    Random.Range(-positionOffsetX, positionOffsetX) * positionOffsetMagnitude,
    //    //    Random.Range(-positionOffsetY, positionOffsetY) * positionOffsetMagnitude,
    //    //    originalPosition.z
    //    //    );

    //    gameObject.transform.rotation = new Quaternion(
    //        Random.Range(-rotationOffsetX, rotationOffsetX) * rotationOffsetMagnitude,
    //        Random.Range(-rotationOffsetY, rotationOffsetY) * rotationOffsetMagnitude,
    //        originalRotation.z,
    //        0
    //        );

    //}



}
