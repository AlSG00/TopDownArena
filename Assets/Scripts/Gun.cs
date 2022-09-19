using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    Transform gunBarrel;                    // Дуло. Точка, где создается пуля
    [SerializeField]
    Transform SleeveEjector;                // Ежектор. Точка, где создаются гильзы
    [SerializeField]
    GameObject projectilePrefab;            // Префаб пули
    [SerializeField]
    GameObject sleevePrefab;                // Префаб гильзы
    [SerializeField]
    float sleeveThrowing_X;                 // Х-угол выброса гильзы
    [SerializeField]
    float sleeveThrowingMinAngle_Y;         // минимальный У-угол выброса гильзы
    [SerializeField]
    float sleeveThrowingMaxAngle_Y;         // максимальный У-угол выброса гильзы
    [SerializeField]
    float sleeveThrowing_Z;                 // Z-угол выброса гильзы
    [SerializeField]
    float fireDelay;                        // Задержка между выстрелами
    [SerializeField]
    Light fireFlame;                        // Источник света для вспышки от выстрела
    [SerializeField]
    float fireFlameIntensity;               // Интенсивность вспышки от выстрела
    [SerializeField]
    float fireFlameFading;                  // Скорость затухания вспышки от выстрела
    [SerializeField]
    float fireFlameRange;                   // Дальность свечения вспышки от выстрела                          
    [SerializeField]
    float bulletSpreadValue;                // Разброс пуль
    [SerializeField]
    private int magCapacity;                // Емкость магазина
    [SerializeField]
    private int ammoStock;                  // Боезапас
    public int currentAmmo;
    public int currentStock;
    Vector3 BulletSpread;
    private float lastTimeShot;
    public static Gun Instance;
    public bool isShooting;
    public GameObject impactEffect;        // Тестовые переменные для добавления эффектов от попадания пули в различные поверхности
    public GameObject impactEffect2;
    public GameObject impactEffect3;

    public AudioClip reloadSound;
    public AudioClip shotSound;
    public AudioSource audioSource;
    public AmmoCounterTest ammoCounter;

    private bool isReloading;
    private void Awake()
    {        
        Instance = GetComponent<Gun>();
        fireFlame.intensity = 0;
        fireFlame.range = fireFlameRange;
        isReloading = false;
        //currentAmmo = magCapacity;
        //currentStock = ammoStock;
      //  audioSource.clip = shotSound;
        ammoCounter.SetCurrentAmmo(currentAmmo, currentStock);
    }

    public void Shoot()
    {
        if ((lastTimeShot + fireDelay <= Time.time) 
            && (currentAmmo > 0)
            && !isReloading)
        {
            lastTimeShot = Time.time;
            audioSource.PlayOneShot(shotSound);
            Instantiate(sleevePrefab, SleeveEjector.position, SleeveEjector.rotation); // делаем гильзу       
            Instantiate(projectilePrefab, gunBarrel.transform.position, gunBarrel.transform.rotation);
            fireFlame.intensity = fireFlameIntensity; // делаем вспышку от выстрела
            SleeveEjector.transform.localEulerAngles = new Vector3(
                sleeveThrowing_X, 
                Random.Range(sleeveThrowingMinAngle_Y, sleeveThrowingMaxAngle_Y), 
                sleeveThrowing_Z
                ); // делаем разброс гильз

            
            currentAmmo--;
            ammoCounter.SetCurrentAmmo(currentAmmo, currentStock);

            
        }
    }

    public void Reload()
    {
        int toFill = magCapacity - currentAmmo;        
        if (toFill > 0 && currentStock > 0)
        {
            isReloading = true;
            audioSource.PlayOneShot(reloadSound);

            //yield return new WaitForSecondsRealtime(3);

            if (currentStock >= toFill)
            {
                currentStock -= toFill;
                currentAmmo = magCapacity;
            }
            else
            {
                currentAmmo += currentStock;
                currentStock = 0;
            }
            ammoCounter.SetCurrentAmmo(currentAmmo, currentStock);
        }
        else
        {
            Debug.Log("Can't reload");
        }

    }

    public void IsReloading()
    {
        if (isReloading == true && !audioSource.isPlaying)
            isReloading = false;
    }

    public void AddAmmo(int ammo)
    {
        currentStock += ammo;
        if (currentStock > ammoStock)
            currentStock = ammoStock;

        ammoCounter.SetCurrentAmmo(currentAmmo, currentStock);
    }

    public Vector3 GetDirection()
    {
        Vector3 direction = gunBarrel.transform.forward;        
        direction += new Vector3(
            Random.Range(-bulletSpreadValue, bulletSpreadValue), 
            Random.Range(-bulletSpreadValue, bulletSpreadValue), 
            Random.Range(-bulletSpreadValue, bulletSpreadValue)
            );
      
        direction.Normalize();

        return direction;
    }    

    public void FireFlameUpdate()
    {
        fireFlame.intensity -= fireFlameFading; 
    }

    //public void Shoot()
    //{
    //    if (lastTimeShot + fireDelay <= Time.time)
    //    {
    //        lastTimeShot = Time.time;
    //        Instantiate(sleevePrefab, SleeveEjector.position, SleeveEjector.rotation); // делаем гильзу       
    //        Instantiate(projectilePrefab, gunBarrel.transform.position, gunBarrel.transform.rotation);
    //        fireFlame.intensity = fireFlameIntensity; // делаем вспышку от выстрела
    //        SleeveEjector.transform.localEulerAngles = new Vector3(sleeveThrowing_X, Random.Range(sleeveThrowingMinAngle_Y, sleeveThrowingMaxAngle_Y), sleeveThrowing_Z); // делаем разброс гильз

    //        //  Vector3 direction = GetDirection(); // делаем разброс пуль
    //        //Debug.Log(direction);
    //        //   if (Physics.Raycast(gunBarrel.transform.position, direction, out hit))
    //        //   {

    //        //  SpawnTrail2(hit);
    //        // пускаем пулю
    //        // Target target = hit.transform.GetComponent<Target>();
    //        //  if (target != null)
    //        //  {
    //        //      target.TakeDamage(damage); // наносим урон
    //        //    }
    //        //  }

    //        // GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // эффект от попадания пули
    //        //  Destroy(impactObj, 1f);
    //    }
    //}
}
