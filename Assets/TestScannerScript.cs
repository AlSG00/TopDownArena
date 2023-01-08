using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScannerScript : MonoBehaviour
{
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.LeftControl))
    //    {

    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<SCRIPT_PickableObject>().HighlightIconWithScanner();
        }
    }
}
