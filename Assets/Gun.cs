using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    Transform gunBarrel;

    [SerializeField]
    Transform SleeveEjector;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    GameObject sleevePrefab;

    [SerializeField]
    float sleeveThrowing_X;

    [SerializeField]
    float sleeveThrowingMinAngle_Y;

    [SerializeField]
    float sleeveThrowingMaxAngle_Y;

    [SerializeField]
    float sleeveThrowing_Z;

    [SerializeField]
    float fireDelay;

    [SerializeField]
    Light fireFlame;

    [SerializeField]
    float fireFlameIntensity;

    [SerializeField]
    float fireFlameFading;
    [SerializeField]
    float fireFlameRange;

    private float lastTimeShot;

    public static Gun Instance;

    public bool isShooting;

    private void Awake()
    {
        Instance = GetComponent<Gun>();
        fireFlame.intensity = 0;
        fireFlame.range = fireFlameRange;
    }


    public void Shoot()
    {
        if (lastTimeShot + fireDelay <= Time.time)
        {
            lastTimeShot = Time.time;
            Instantiate(projectilePrefab, gunBarrel.position, gunBarrel.rotation);
            Instantiate(sleevePrefab, SleeveEjector.position, SleeveEjector.rotation);            
            fireFlame.intensity = fireFlameIntensity;
            SleeveEjector.transform.localEulerAngles = new Vector3(sleeveThrowing_X, Random.Range(sleeveThrowingMinAngle_Y, sleeveThrowingMaxAngle_Y), sleeveThrowing_Z);
        }
    }

    public void FireFlameUpdate()
    {
        fireFlame.intensity -= fireFlameFading;
    }
}
