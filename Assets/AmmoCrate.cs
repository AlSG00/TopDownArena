using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    [SerializeField]
    private int ammo;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<Gun>().AddAmmo(ammo);
            collision.gameObject.GetComponentInChildren<Gun>().AddAmmo(ammo);
            Destroy(gameObject);
        }      
    }
}
