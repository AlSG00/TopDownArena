using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 gunBarrel;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float maxProjectileDistance;
    RaycastHit hit;
    Ray ray;
    public ParticleSystem particle;

    public float damage;

    Vector3 velocity;
    void Start()
    {
        gunBarrel = transform.position;
        //  ray = new RaycastHit();
        //  ray.distance = 5;
        ray.direction = Vector3.back;
        ray.origin = transform.position;

        
       // counter = 0;
    }

    //void Update()
    //{
    //    MoveProjectile();
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        Debug.Log(hit.collider.name);
    //    }
    //}

    void MoveProjectile()
    {

        if (Vector3.Distance(gunBarrel, transform.position) > maxProjectileDistance)
            Destroy(this.gameObject);
        else
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        // Посмотреть видеоурок и переделать н Рэйкасты
     //   velocity += projectileSpeed * Time.fixedDeltaTime;
      //  Vector3 displacement = projectileSpeed * Time.fixedDeltaTime;

      //  ray = new Ray(transform.position, displacement);
    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyHeealth>().TakeDamage(damage);
            Destroy(gameObject);
          //  Debug.Log("Enemy");
        }
        if (collision.gameObject.tag == "Wall")
        {
            //    if (ray.rigidbody.tag == "Wall")
            //    if (ray.rigidbody.tag == "Wall")
            ContactPoint contact = collision.GetContact(0);

            Instantiate(particle);
            // Instantiate(particle, transform.position, contact.normal);
            particle.transform.position = contact.point;
            particle.transform.forward = contact.normal;
        //    collision.gameObject.GetComponent<EnemyHeealth>().TakeDamage(damage);
            Destroy(gameObject);
            
            Debug.Log("Wall");
        }
    }



    //public RaycastHit()
    //{

    //}
}
