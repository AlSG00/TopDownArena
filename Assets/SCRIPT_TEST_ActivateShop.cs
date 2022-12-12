using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_TEST_ActivateShop : MonoBehaviour, SCRIPT_IInteract
{
    public Transform canSpawner;
    public GameObject canPrefab;
    bool canInteract = false;
    public bool onCursor = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    public void Interact()
    {
        if (canInteract == false || onCursor == false)
        {
            return;
        }

        Instantiate(canPrefab, canSpawner.position, Quaternion.identity);
    }
}
