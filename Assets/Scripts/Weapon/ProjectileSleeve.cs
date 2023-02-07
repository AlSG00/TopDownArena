using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSleeve : MonoBehaviour
{
    private Vector3 sleeveEjector;

    //[SerializeField]
    //private float projectileSpeed;

    [SerializeField]
    private float maxProjectileDistance;

    [SerializeField]
    private float ejectionForce;

    [SerializeField]
    private float minTorque;

    [SerializeField]
    private float maxTorque;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sleeveEjector = transform.position;
        MoveProjectile();
    }

    void MoveProjectile()
    {
        if (Vector3.Distance(sleeveEjector, transform.position) > maxProjectileDistance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            rb.AddRelativeForce(Vector3.right * ejectionForce * Time.deltaTime, ForceMode.Impulse);
            rb.AddRelativeTorque(Vector3.right * Random.Range(minTorque, maxTorque) * Time.deltaTime, ForceMode.Impulse);
            rb.AddRelativeTorque(Vector3.left * Random.Range(minTorque, maxTorque) * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
