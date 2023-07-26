using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    //TODO: Obsolete
    [SerializeField] private int _ammo;
    private bool _isMouseOver;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && _isMouseOver)
        {
            other.gameObject.GetComponent<SCRIPT_PlayerAmmunition>().rifleAmmo.AddAmmo(_ammo);
            Destroy(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        _isMouseOver = true;
    }

    private void OnMouseExit()
    {
        _isMouseOver = false;
    }
}
