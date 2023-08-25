using System.Collections;
//using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: Check and rename all the methods here
public class Weapon : MonoBehaviour
{
    public Weapon(int remainingAmmo)
    {
        currentAmmoInMag = remainingAmmo;
    }

    ~Weapon()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            Destroy(bullets[i].tracer.gameObject, 1000f);
            bullets.Remove(bullets[i]);
            i--;
        }
    }

    public SCRIPT_PlayerAmmunition.AmmoType ammoType;

    //public InventoryController.BindSlot availableSlots;

    //SCRIPT_PlayerAmmunition.AmmoCollection ammoCollection;

    //private void bubaTest() // TODO: Test method. Redo
    //{
    //    SCRIPT_PlayerAmmunition.Ammo ammo = ammoCollection.AmmoTypeCollection[ammoType];
    //}

    private class Bullet
    {
        public float lifeTime;          // ѕо истечении указанного времени пул€ удалитс€, если ни с чем не столкнетс€
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;

        public Bullet (Vector3 position, Vector3 velocity, TrailRenderer bulletTracer)
        {
            initialPosition = position;
            initialVelocity = velocity;
            lifeTime = 0.0f;
            tracer = Instantiate(bulletTracer, position, Quaternion.identity);
            tracer.AddPosition(position);
        }

        public Vector3 GetPosition()
        {
            return initialPosition + initialVelocity * lifeTime;
        }
    }

    // TODO: Probably should incapsulate this to separate file?
    [System.Serializable]
    private class HitEffect
    {
        // TODO:
        // - Create some static class to store tags collection
        // - inherit created class and make selectable parameter to pick required tag
        public string tag;
        public ParticleSystem effect;
    }

    #region PARAMETERS

    [SerializeField] private List<HitEffect> hitEffects = new();
    [SerializeField] private float hitEffectLifetime = 1200f;

    [Header("References")]
    public SCRIPT_MuzzleFlame muzzleFlame;
    public SCRIPT_AmmoShells ammoShells;
    //public AmmoCounterTest ammoCounter;
    public WeaponAudio weaponAudio;

    // ѕрикрутить ссылку на скрипт, в котором будет AnimationEvents дл€ перезар€дки каждой отдельной пушки
    // —ейчас перезар€дка захардкожена под автомат

    // Muzzle effects
    // Ammo shels generator
    // Available ammo in storage handler
    // Available ammo in magazine handler
    // Weapon parameters
    // Weapon audio
    // 

    [Header("Weapon parameters")]
    public string weaponName;
    //public SCRIPT_ActiveWeapon.WeaponSlot WeaponSlot;
    public float fireRate = 25;
    public float bulletSpeed = 1000f;
    public int projectilesPerShot = 1;
    public LayerMask activeLayers; // TODO: What is it?
    public float singleShotDelay = 0.3f;
    public float impactForce;
    public float damage;
    public float bulletSpreadValue;
    [SerializeField] private int magCapacity = 30;
    public int currentAmmoInMag = 30;
    public bool singleShots;

    [Header("Visual")]
    public GameObject hitEffectMetal;
    public GameObject hitEffectConcrete;
    public GameObject hitEffectFlesh;

    public TrailRenderer tracerEffect;

    private ParticleSystem hitEffect;
    public GameObject magazine;
    public Transform muzzle;

     //TODO: move to separated component
    //[Header("Audio")]
    //public AudioClip reloadSound;
    //public AudioClip shotSound;
    //public AudioSource audioSource;

    //[Header("[Temp rifle reload audio]")]
    //public AudioClip ejectMagSound;
    //public AudioClip putMagSound;
    //public AudioClip pullOutMagSound;
    //public AudioClip InsertMagSound;

    // Other
    private Ray ray;
    private RaycastHit hitInfo;
    //private float accumulatedTime;
    private List<Bullet> bullets = new List<Bullet>();
    private float _maxBulletLifetime = 3f;

    public bool isReloading;
    public bool isFiring = false;
    public bool shotPerformed;
    private float _lastTimeShot;
    private float fireDelay;

    #endregion

    private void Awake()
    {
        _lastTimeShot = 0f;

        bullets = new List<Bullet>();
    }

    private void Start()
    {
        isReloading = false;
        TryGetComponent<SCRIPT_MuzzleFlame>(out muzzleFlame);
        TryGetComponent<SCRIPT_AmmoShells>(out ammoShells);
        //FireBullet();
        //_lastTimeShot = 0f;

        //bullets = new List<Bullet>();
        // TODO: Find another way to connect HUD to a newly picked weapon
        //ammoCounter = GameObject.Find("HUD").GetComponentInChildren<AmmoCounterTest>();


        //ammoInStock = GameObject.Find("Player").GetComponentInChildren<SCRIPT_PlayerAmmunition>();

        //if (weaponName == "Rifle")
        //{
        //    currentWeaponStock = ammoInStock.rifleAmmo;
        //    Debug.LogWarning(currentWeaponStock);
        //}
        //else if (weaponName == "Shotgun")
        //{
        //    currentWeaponStock = ammoInStock.shotgunAmmo;
        //    Debug.LogWarning(currentWeaponStock);
        //}
    }

    private void Update()
    {
        if ((bullets != null) && (bullets.Count != 0))
        {
            UpdateBullet(Time.deltaTime);
        }
    }

    public void StartFiring()
    {
        isFiring = true;
        fireDelay = 60f / fireRate;
        //accumulatedTime = 0.0f;

        FireBullet();
    }

    private void FireBullet()
    {
        //_lastTimeShot = 0;
        if ((_lastTimeShot + fireDelay <= Time.time) || (currentAmmoInMag <= 0) || isReloading)
        {
            Debug.Log($"<color=green>{_lastTimeShot + fireDelay} : {Time.time}</color>");
            //Debug.Log("<color=red>Can't shoot</color>");
            //return;


            _lastTimeShot = Time.time;

            GenerateShot();

            // TODO: Get it out of this script somehow
            if (muzzleFlame)
            {
                muzzleFlame.LightFlame();
            }

            // TODO: Get it out of this script somehow x2
            if (ammoShells)
            {
                ammoShells.EjectShell();
            }

            if (weaponAudio)
            {
                weaponAudio.PlayShotSound();
            }

            //currentAmmoInMag--;
            //ammoCounter.SetCurrentAmmo(currentAmmoInMag, currentWeaponStock.left);
        }
        else
        {
            Debug.Log("<color=red>Can't fire.</color>");
        }
    }

    // «адержка между выстрелами. TODO: ѕрвоерить, используетс€ ли она вообще где-то
    //public void UpdateFiring(float deltaTime)
    //{
    //    accumulatedTime += deltaTime;
    //    float fireInterval = 1.0f / fireRate;
    //    while (accumulatedTime >= 0.0f)
    //    {
    //        FireBullet();
    //        accumulatedTime -= fireInterval;
    //    }
    //}

    private void GenerateShot()
    {
        //if (bullets == null) { bullets = new List<Bullet>(); }
        
        for (int i = 0; i < projectilesPerShot; i++)
        {
            Vector3 velocity = GetSpreadDirection() * bulletSpeed;
            var bullet = new Bullet(muzzle.position, velocity, tracerEffect);
            bullets.Add(bullet);
        }
    }

    // ќбработка уже выпущенных пуль
    public void UpdateBullet(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    // ќбработка перемещени€ всех выпущенных пуль
    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = bullet.GetPosition();
            bullet.lifeTime += deltaTime;
            Vector3 p1 = bullet.GetPosition();
            RaycastSegment(p0, p1, bullet);
        });
    }

    // –ассчет отрезка вектора перемещени€ пули
    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        GameObject impactTarget = null;

        if (Physics.Raycast(ray, out hitInfo, distance, activeLayers))
        {
            TryAssignDamage(hitInfo);

            if (hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForce(-hitInfo.normal * impactForce);
            }

            TryGenerateHitEffect(hitInfo);
            // TODO: Get rid of this if-else-if construction
            // Wrote new class for that. Need to write a method that will take tag from hitInfo and find list item with required tag.
            //asdgasdf
            //if (hitInfo.transform.CompareTag("Concrete"))
            //{
            //    impactTarget = Instantiate(hitEffectConcrete, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //}
            //else if (hitInfo.transform.CompareTag("Metal"))
            //{
            //    impactTarget = Instantiate(hitEffectMetal, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //}
            //else if (hitInfo.transform.CompareTag("Flesh"))
            //{
            //    impactTarget = Instantiate(hitEffectFlesh, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //}

            //Destroy(impactTarget, 1200f);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.lifeTime = _maxBulletLifetime;
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    private void TryGenerateHitEffect(RaycastHit hit)
    {
        try
        {
            HitEffect requiredEffect = hitEffects.Find(effect => effect.tag == hit.transform.tag);
            ParticleSystem effectToCreate = Instantiate(requiredEffect.effect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(effectToCreate, 1200f);
        }
        catch
        {
            Debug.LogError($"<color=red>Cannot find hit effect for '{hit.transform.tag}'</color>");
        }
        //if (hitInfo.transform.CompareTag("Concrete"))
        //{
        //    impactTarget = Instantiate(hitEffectConcrete, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        //}
        //else if (hitInfo.transform.CompareTag("Metal"))
        //{
        //    impactTarget = Instantiate(hitEffectMetal, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        //}
        //else if (hitInfo.transform.CompareTag("Flesh"))
        //{
        //    impactTarget = Instantiate(hitEffectFlesh, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        //}
    }

    public void TryAssignDamage(RaycastHit hitInfo)
    {
        Target target = hitInfo.transform.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    //  онтроль времени жизни пули, если она никуда не врезалась
    private void DestroyBullets()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].lifeTime >= _maxBulletLifetime)
            {
                Destroy(bullets[i].tracer.gameObject, 3f);
                bullets.Remove(bullets[i]);
                i--;
            }
        }
        //bullets.RemoveAll(bullet => bullet.lifeTime >= _maxBulletLifetime);
    }


    // TODO: Rework
    public void Reload()
    {
        //int toFill = magCapacity - currentAmmoInMag; // считаем, сколько не хватает

        

        //currentAmmoInMag += currentWeaponStock.TakeAmmo(toFill); // досыпаем

        //if (toFill > 0 && currentWeaponStock.left > 0)
        //{
        //    //ammoCounter.SetCurrentAmmo(currentAmmoInMag, currentWeaponStock.left);
        //    Debug.Log($"Left {currentWeaponStock.left}");
        //}
        //else
        //{
        //    Debug.Log("Can't reload");
        //}

        //// PSEUDOCODE for method reworking:
        //// - Check remaining ammo in the ammunition component
        //// - If it has required ammo available, reload:
        ////      - add ammo
        ////      - play animation
        ////      - play sound
        ////      - reduce remaining ammo count in the ammunition component
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



    private float _lastTimeSingleShot;
    public bool NextShotReadyCheck()
    {
        if (shotPerformed = false && _lastTimeSingleShot + singleShotDelay <= Time.time)
        {
            return true;
        }

        return false;
    }
}
