using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Flashlight : MonoBehaviour
{

    [SerializeField]
    private Light flashlight;

    [SerializeField]
    private Light ambient;

    // Update is called once per frame
    void Update()
    {
        UseFlashlight();
    }

    private void UseFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && flashlight.enabled == false)
        {
            flashlight.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && flashlight.enabled == true)
        {
            flashlight.enabled = false;

        }
    }
}
