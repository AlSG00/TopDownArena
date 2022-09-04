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

    [SerializeField]
    float damage;

    private float lastTimeShot;

    public static Gun Instance;

    public bool isShooting;

    int count = 0;

    public GameObject impactEffect;
    public GameObject impactEffect2;
    public GameObject impactEffect3;

    private void Awake()
    {
        Instance = GetComponent<Gun>();
        fireFlame.intensity = 0;
        fireFlame.range = fireFlameRange;
    }

    void Update()
    {
        //if (Input.GetButton("Fire1"))
        //{
        //    Shoot();
        //}
      //  FireFlameUpdate();
    }

    public void Shoot()
    {
        if (lastTimeShot + fireDelay <= Time.time)
        {
            lastTimeShot = Time.time;
            RaycastHit hit;
            Instantiate(sleevePrefab, SleeveEjector.position, SleeveEjector.rotation);
            fireFlame.intensity = fireFlameIntensity;
            SleeveEjector.transform.localEulerAngles = new Vector3(sleeveThrowing_X, Random.Range(sleeveThrowingMinAngle_Y, sleeveThrowingMaxAngle_Y), sleeveThrowing_Z);
            if (Physics.Raycast(gunBarrel.transform.position, gunBarrel.transform.forward, out hit))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }

            GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObj, 1f);
        }
    }

    //public void Shoot()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(gunBarrel.transform.position, gunBarrel.transform.forward, out hit))
    //    {
    //        Debug.Log(hit.transform.name);
    //    }
    //}

    //public void Shoot()
    //{
    //    if (lastTimeShot + fireDelay <= Time.time)
    //    {
    //        lastTimeShot = Time.time;
    //        Instantiate(projectilePrefab, gunBarrel.position, gunBarrel.rotation);
    //        Instantiate(sleevePrefab, SleeveEjector.position, SleeveEjector.rotation);            
    //        fireFlame.intensity = fireFlameIntensity;
    //        SleeveEjector.transform.localEulerAngles = new Vector3(sleeveThrowing_X, Random.Range(sleeveThrowingMinAngle_Y, sleeveThrowingMaxAngle_Y), sleeveThrowing_Z);
    //    }
    //}

    public void FireFlameUpdate()
    {
     //   while (fireFlame.intensity > 0)
     //   {
            fireFlame.intensity -= fireFlameFading;
     //   }
    }
}
