using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Shooting : MonoBehaviour
{
    // TODO: REWORK THIS SCRIPT
    public bool isShooting;
    public GameObject prefab;
    public GameObject barrel;
    public TestRaycastWeapon weapon;
    private void Start()
    {
        isShooting = false;
    }

    void Update()
    {
        ShootInput();                       // Проверка, нажата ли кнопка выстрела
        Gun.Instance.FireFlameUpdate();     // Угасание вспышки от выстрела
    }

    void ShootInput()
    {
        //if (Input.GetButton("Fire1"))
        //{          
        //    Gun.Instance.Shoot();            
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Gun.Instance.Reload();
        //}

        if (Input.GetButtonDown("Fire1"))
        {
            weapon.StartFiring();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            weapon.StopFiring();
        }
    }
}
