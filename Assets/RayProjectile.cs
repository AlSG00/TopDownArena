using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    private Vector3 previousPos;
    public GameObject impactEffect;
    public float speed;
    public TrailRenderer trace;
    public LayerMask layerMask;
    public GameObject gunbarrel;
    public float damage;
    public float step;
    private Vector3 _direction;

    public float bulletSpreadValue;
    void Start()
    {
        _direction = GetDirection();
        rb = GetComponent<Rigidbody>();        
        Shoot();
        Destroy(gameObject, 10);
    }

    void FixedUpdate()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 thisPos = transform.position;
        Vector3 stepDirection = rb.velocity.normalized;
        float stepSize = (thisPos - previousPos).magnitude;

        if (stepSize > step)
        {
            if (Physics.Raycast(previousPos, stepDirection /*gunbarrel.transform.forward*/, out hit, stepSize, layerMask))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage); // наносим урон
                }
                //  Destroy(gameObject);
                Destruct(hit.point, hit.normal, hit.transform.root);
            }
            else
            {
                previousPos = thisPos;
            }
        }
    }

    private void Destruct(Vector3 point, Vector3 normal, Transform target)
    {
        //  var hitNormal = Quaternion.Euler(normal);
        // GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // эффект от попадания пули
       
        Destroy(gameObject);
        GameObject impactObj = Instantiate(impactEffect, point, Quaternion.LookRotation(normal) /*gunbarrel*//*Quaternion.Euler(normal)*/);
        //  AudioSource.PlayClipAtPoint(explosionAudio, transform.position, 2f);
        Destroy(impactObj, 1f);
    }

    public void Shoot()
    {
        previousPos = transform.position;

        rb.AddForce(_direction * speed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        direction += new Vector3(
            Random.Range(-bulletSpreadValue, bulletSpreadValue),
            Random.Range(-bulletSpreadValue, bulletSpreadValue),
            Random.Range(-bulletSpreadValue, bulletSpreadValue)
            );

        // direction.Normalize();

        return direction;
    }
}
