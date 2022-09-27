using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Aim : MonoBehaviour
{
    [SerializeField]
    private GameObject trgt;
    [SerializeField]
    private Vector3 targetOffset;
    [SerializeField]
    private float movementSpeed;
    private Transform _target;
    private void Awake()
    {
        //trgt = GameObject.Find("Player");
        //_target = trgt.transform;
        Transform temp = GameObject.Find("Player").transform;
        _target = temp;
    }

    void FixedUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position + targetOffset, movementSpeed * Time.deltaTime);
    }


}
