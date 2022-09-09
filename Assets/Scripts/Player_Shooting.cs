using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Shooting : MonoBehaviour
{
    public bool isShooting;
    public GameObject prefab;
    public GameObject barrel;

    private void Start()
    {
        isShooting = false;
    }

    void Update()
    {
        ShootInput();
        Gun.Instance.FireFlameUpdate();
    }

    void ShootInput()
    {
        if (Input.GetButton("Fire1"))
        {
            //isShooting = true;
            Gun.Instance.Shoot();
            //Instantiate(prefab, barrel.transform.position, barrel.transform.rotation);
        }
        else
        {
            //isShooting = false;
        }
    }
}
