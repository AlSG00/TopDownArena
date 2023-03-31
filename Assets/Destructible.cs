using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion;

    public float explosionForce = 0.1f;
    public float explosionRadius = 0.1f;
    public float explosionUpward = 0.1f;

    public void DestroyObject()
    {
        Debug.Log("Instantiating...");
        GameObject temp = Instantiate(destroyedVersion, transform.position, transform.rotation);

        foreach (Transform child in temp.transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);

                Vector3 randomDirection = Random.insideUnitSphere;
                randomDirection.y = Mathf.Abs(randomDirection.y);
                rb.AddForce(randomDirection * Random.Range(0.5f, 1.5f), ForceMode.Impulse);
            }
        }

        Debug.Log("Destroying...");
        Destroy(gameObject);


    }

    private void OnMouseUp()
    {
        DestroyObject();
    }
}
