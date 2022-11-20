using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _object;
    [SerializeField] private int _ammo;
    private bool _isMouseOver;
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && isMouseOver)
    //    //{
    //    //    //collision.gameObject.GetComponent<Gun>().AddAmmo(ammo);
    //    //    collision.gameObject.GetComponentInChildren<Gun>().AddAmmo(ammo);
    //    //    Destroy(gameObject);
    //    //}


    //    if (collision.gameObject.tag == "Player"  )
    //    {
    //        Debug.Log("tag correct");
    //        if (isMouseOver)
    //        {

    //            Debug.Log("key correct");
    //            if (Input.GetKey(KeyCode.E))
    //            {
    //                Debug.Log("mouse aimed");
    //                collision.gameObject.GetComponentInChildren<Gun>().AddAmmo(ammo);
    //                Destroy(gameObject);
    //            }
    //        }
    //    }
    //}
    private void OnTriggerStay(Collider other)
    {
        //RaycastHit hit;
        //if (Physics.Raycast(other.transform.position, other.transform.forward, out hit))
        //{
        //    if (hit.transform.name == "AmmoCrate")
        //    {

                if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && _isMouseOver)
                {
            //collision.gameObject.GetComponent<Gun>().AddAmmo(ammo);
            //other.gameObject.GetComponentInChildren<SCRIPT_Weapon>().AddAmmo(_ammo);
            other.gameObject.GetComponent<SCRIPT_PlayerAmmunition>().rifleAmmo.AddAmmo(_ammo);
                    Destroy(gameObject);
                }
        //    }
        //}
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && isMouseOver)
    //    {
    //        //collision.gameObject.GetComponent<Gun>().AddAmmo(ammo);
    //        other.gameObject.GetComponentInChildren<Gun>().AddAmmo(ammo);
    //        Destroy(gameObject);
    //    }

    //}

    private void OnMouseEnter()
    {
        _isMouseOver = true;
    }

    private void OnMouseExit()
    {
        _isMouseOver = false;
    }
}
