using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_Weapon : MonoBehaviour
{
    class Bullet
    {
        public float lifeTime;          // По истечении указанного времени пуля удалится, если ни с чем не столкнется
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public string weaponName;

    public bool isFiring = false;
    public float fireRate = 25;
    public float bulletSpeed = 1000f;
    public float impactForce;
    public float damage;
    public float bulletSpreadValue;
    public GameObject hitEffectMetal;
    public GameObject hitEffectConcrete;


    
    //public ParticleSystem hitEffectMetal;
    //public ParticleSystem hitEffectConcrete;
    public TrailRenderer tracerEffect;
    public Transform muzzle;
    public LayerMask activeLayers;
    private Ray ray;
    private RaycastHit hitInfo;
    private float accumulatedTime;
    private List<Bullet> bullets = new List<Bullet>();
    private float maxLifeTime = 3f;
    public SCRIPT_MuzzleFlame muzzleFlame;
    public SCRIPT_AmmoShells ammoShells;
    private bool isReloading;
    private float lastTimeShot;
    private float fireDelay;
    public AudioClip reloadSound;
    public AudioClip shotSound;
    public AudioSource audioSource;
    [SerializeField]
    private int magCapacity = 30;
    [SerializeField]
    private int ammoStock = 150;                  // Боезапас
    public int currentAmmo = 30;
    public int currentStock = 90;
    public AmmoCounterTest ammoCounter;
    public bool singleShots;
    public bool shotPerformed;
    public int projectilesPerShot = 1;
    private ParticleSystem hitEffect;
    public float singleShotDelay = 0.3f;


    public AnimationClip weaponIdleAnimation;
    public AnimationClip weaponAimAnimation;
    public AnimationClip weaponSwapAnimation;
    public AnimationClip weaponReloadAnimation;
    public AnimationClip weaponShootAnimation;

    private void Start()
    {
        isReloading = false;
        muzzleFlame = GetComponent<SCRIPT_MuzzleFlame>();
        ammoShells = GetComponent<SCRIPT_AmmoShells>();
        ammoCounter = GameObject.Find("HUD").GetComponentInChildren<AmmoCounterTest>();
    }

    public void StartFiring()
    {
        isFiring = true;
        fireDelay = (float)60 / fireRate;
        accumulatedTime = 0.0f;

    
        FireBullet();
    }

    // Выстрел
    // Проигрывание Пламени от выстрела
    // Создание и добавление в список экземпляра пули
    private void FireBullet()
    {
        if ((lastTimeShot + fireDelay <= Time.time)
            && (currentAmmo > 0)
            && !isReloading)
        {
            lastTimeShot = Time.time;
            audioSource.PlayOneShot(shotSound);
            //Vector3 velocity = (raycastDestination.position - muzzle.position).normalized * bulletSpeed;
            //Vector3 velocity = transform.forward/*.normalized*/ * bulletSpeed;

            //Vector3 velocity = GetSpreadDirection() * bulletSpeed;

            for (int i = 0; i < projectilesPerShot; i++)
            {
                Vector3 velocity = GetSpreadDirection() * bulletSpeed;
                var bullet = CreateBullet(muzzle.position, velocity);
                bullets.Add(bullet);
            }

            muzzleFlame.LightFlame();
            ammoShells.EjectShell();

            currentAmmo--;
            ammoCounter.SetCurrentAmmo(currentAmmo, currentStock);
        }
    }

    // Функция создает пулю в момент выстрела
    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.lifeTime = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);

        return bullet;
    }

    // Задержка между выстрелами
    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while (accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    // Обработка уже выпущенных пуль
    public void UpdateBullet(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    // Обработка перемещения всех выпущенных пуль
    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.lifeTime += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    // Вычисление следующей позиции пули с учетом времени после выстрела
    Vector3 GetPosition(Bullet bullet)
    {
        return bullet.initialPosition + bullet.initialVelocity * bullet.lifeTime;
    }

    // Рассчет отрезка вектора перемещения пули
    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, distance, activeLayers))
        {
            //hitEffect.transform.position = hitInfo.point;
            //hitEffect.transform.forward = hitInfo.normal;
            //hitEffect.Emit(1);

            GameObject impactObj = null/* Instantiate(impactEffect, point, Quaternion.LookRotation(normal) /*gunbarrel*//*Quaternion.Euler(normal))*/;

            Target target = hitInfo.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForce(-hitInfo.normal * impactForce);
            }

          //  GameObject impactObj;
            if (hitInfo.transform.CompareTag("Concrete"))
            {
                 impactObj = Instantiate(hitEffectConcrete, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                //hitEffect = hitEffectConcrete;
            }
            else
            {
                //hitEffect = hitEffectMetal;
                impactObj = Instantiate(hitEffectMetal, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            }
          //  impactObj.GetComponent<ParticleSystem>().Play();
            //hitEffect.transform.position = hitInfo.point;
            //hitEffect.transform.forward = hitInfo.normal;
            //hitEffect.Play();

            Destroy(impactObj, 12f);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.lifeTime = maxLifeTime;
            //Destroy(bullet.tracer, 1f);
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    // Контроль времени жизни пули, если она никуда не врезалась
    private void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.lifeTime >= maxLifeTime);
    }

    public void Reload()
    {
        int toFill = magCapacity - currentAmmo;
        if (toFill > 0 && currentStock > 0)
        {
            isReloading = true;
            audioSource.PlayOneShot(reloadSound);

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

    public void AddAmmo(int ammo)
    {
        currentStock += ammo;
        if (currentStock > ammoStock)
            currentStock = ammoStock;

        ammoCounter.SetCurrentAmmo(currentAmmo, currentStock);
    }

    public void IsReloading()
    {
        if (isReloading == true && !audioSource.isPlaying)
            isReloading = false;
    }

    public Vector3 GetSpreadDirection()
    {
        Vector3 direction = muzzle.transform.forward;
        direction += new Vector3(
            Random.Range(-bulletSpreadValue, bulletSpreadValue),
            Random.Range(-bulletSpreadValue, bulletSpreadValue),
            Random.Range(-bulletSpreadValue, bulletSpreadValue)
            );

        direction.Normalize();

        return direction;
    }
}
    
