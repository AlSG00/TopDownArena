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
        ShootInput();                       // Проверка, нажата ли кнопка выстрела
        Gun.Instance.FireFlameUpdate();     // Угасание вспышки от выстрела
        Gun.Instance.IsReloading();
    }

    void ShootInput()
    {
        if (Input.GetButton("Fire1"))
        {          
            Gun.Instance.Shoot();            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Gun.Instance.Reload();
        }
        //else
        //{
        //    //isShooting = false;
        //}
    }
}
