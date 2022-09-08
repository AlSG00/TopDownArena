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

    [SerializeField]
    float bulletSpreadValue;

    [SerializeField]
    private TrailRenderer BulletTrail;
    Vector3 BulletSpread;

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
        float test = bulletSpreadValue;
        Vector3 BulletSpread = new Vector3(test, test, test);
    }

    void Update()
    {

    }

    public void Shoot()
    {
        if (lastTimeShot + fireDelay <= Time.time)
        {
            lastTimeShot = Time.time;
            RaycastHit hit;
            Instantiate(sleevePrefab, SleeveEjector.position, SleeveEjector.rotation); // делаем гильзу
            fireFlame.intensity = fireFlameIntensity; // делаем вспышку от выстрела
            SleeveEjector.transform.localEulerAngles = new Vector3(sleeveThrowing_X, Random.Range(sleeveThrowingMinAngle_Y, sleeveThrowingMaxAngle_Y), sleeveThrowing_Z); // делаем разброс гильз
          
            Vector3 direction = GetDirection(); // делаем разброс пуль
            Debug.Log(direction);
            if (Physics.Raycast(gunBarrel.transform.position, direction, out hit))
            {
                
                SpawnTrail2(hit);
                // пускаем пулю
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage); // наносим урон
                }
            }

            GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // эффект от попадания пули
            Destroy(impactObj, 1f);
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = gunBarrel.transform.forward;        
        direction += new Vector3(Random.Range(-bulletSpreadValue, bulletSpreadValue), Random.Range(-bulletSpreadValue, bulletSpreadValue), Random.Range(-bulletSpreadValue, bulletSpreadValue));
      
       // direction.Normalize();

        return direction;
    }

    private IEnumerable SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startposition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startposition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = Hit.point;
        Instantiate(impactEffect, Hit.point, Quaternion.LookRotation(Hit.normal));

        Destroy(Trail.gameObject, Trail.time);
    }

    private void SpawnTrail2(RaycastHit Hit)
    {
        float time = 0;
        TrailRenderer Trail = Instantiate(BulletTrail, gunBarrel.transform.position, Quaternion.identity);

        Vector3 startposition = Trail.transform.position;

       // while (time < 1)
       // {
            Trail.transform.position = Vector3.Lerp(startposition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

        
      //  }
        Trail.transform.position = Hit.point;
     //   Instantiate(impactEffect, Hit.point, Quaternion.LookRotation(Hit.normal));

        Destroy(Trail.gameObject, Trail.time);
    }

    public void FireFlameUpdate()
    {
            fireFlame.intensity -= fireFlameFading;
    }
}
