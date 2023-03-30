using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion;

    public void DestroyObject()
    {
        Debug.Log("Instantiating...");
        Instantiate(destroyedVersion, transform.position, transform.rotation);

        Debug.Log("Destroying...");
        Destroy(gameObject);
    }

    private void OnMouseUp()
    {
        DestroyObject();
    }
}
