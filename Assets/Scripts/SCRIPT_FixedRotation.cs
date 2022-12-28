using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_FixedRotation : MonoBehaviour
{
    Vector3 axis;

    private void Awake()
    {
        //axis = new Vector3(
        //    transform.rotation.x,
        //    transform.rotation.y,
        //    180f
        //    );     
    }

    private void Update()
    {
        axis = new Vector3(
            90f,
            transform.rotation.y,
            transform.rotation.z
            );

        transform.rotation = Quaternion.Euler(axis);
    }
}
