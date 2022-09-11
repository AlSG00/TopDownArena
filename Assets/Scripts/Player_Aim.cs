using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Aim : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 targetOffset;
    [SerializeField]
    private float movementSpeed;

    void Start()
    {

    }

    void FixedUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, movementSpeed * Time.deltaTime);
    }


}
