using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float lifeTime;          // По истечении указанного времени пуля удалится, если ни с чем не столкнется
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public bool isFiring = false;
    public float fireRate = 25;
    public float bulletSpeed = 1000f;
    public float impactForce;
    public float damage;
    public float bulletSpreadValue;

    public ParticleSystem[] muzzleFlash;
    public GameObject hitEffect;
    public TrailRenderer tracerEffect;
    public Transform muzzle;
    [SerializeField]
    Light fireFlame;                        // Источник света для вспышки от выстрела
    [SerializeField]
    float fireFlameIntensity;               // Интенсивность вспышки от выстрела
    [SerializeField]
    float fireFlameFading;                  // Скорость затухания вспышки от выстрела
    [SerializeField]
    float fireFlameRange;                   // Дальность свечения вспышки от выстрела
    // public Transform raycastDestination;
    public LayerMask activeLayers;
    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifeTime = 3f;

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        FireBullet();
    }

    // Выстрел
    // Проигрывание Пламени от выстрела
    // Создание и добавление в список экземпляра пули
    private void FireBullet()
    {
        for (int i = 0; i < muzzleFlash.Length; i++)
        {
            muzzleFlash[i].Emit(1);
        }

        //Vector3 velocity = (raycastDestination.position - muzzle.position).normalized * bulletSpeed;
        Vector3 velocity = transform.forward/*.normalized*/ * bulletSpeed;
        var bullet = CreateBullet(muzzle.position, velocity);
        bullets.Add(bullet);
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

            //GameObject impactObj = Instantiate(impactEffect, point, Quaternion.LookRotation(normal) /*gunbarrel*//*Quaternion.Euler(normal)*/);

            Target target = hitInfo.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForce(-hitInfo.normal * impactForce);
            }

            GameObject impactObj = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactObj, 1f);

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

    public void FireFlameUpdate()
    {
        fireFlame.intensity -= fireFlameFading;
    }
}
