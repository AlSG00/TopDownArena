using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_CameraMoving : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetOffset;
    [SerializeField]
    private float movementSpeed;
    private Transform _target;

    private Quaternion originRotation;

    public float rotationOffsetX;
    public float rotationOffsetY;
    public float rotationOffsetMagnitude;
    public float shakingDelay;

    private float shakeTime;
    private void Awake()
    {
        Transform player = GameObject.Find("Player").transform;
        Transform _player = player.transform.GetChild(0);
        _target = _player;
        originRotation = transform.rotation;
        //originRotation.x = 90;
       // StartCoroutine(ShakeCamera());
    }

    private void FixedUpdate()
    {
        MoveCamera();
        ShakeCamera();
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position + targetOffset, movementSpeed * Time.deltaTime);
    }

    private void ShakeCamera()
    {
        if (shakeTime + shakingDelay <= Time.time)
        {
          //  shakeTime = Time.time;
          ////  Random.Range(-rotationOffsetX, rotationOffsetX);

          //  transform.rotation = new Quaternion(
          //       originRotation.x,
          //       Mathf.Lerp(-Random.Range(-rotationOffsetY, rotationOffsetY), Random.Range(-rotationOffsetY, rotationOffsetY), 1),
          //      //   Mathf.Lerp(90 - rotationOffsetY, 90 + rotationOffsetY, 1),
          //      originRotation.z, 0);

        }
        
    }



}
