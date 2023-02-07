using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_InteractableObjectTrigger : MonoBehaviour
{
    public bool playerOnly = true;
    public bool inInteractionArea;

    private void OnTriggerEnter(Collider other)
    {
        CheckEnteringTrigger(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckEnteringTrigger(other, false);
    }

    private void CheckEnteringTrigger(Collider other, bool isEntered)
    {
        if (playerOnly == true)
        {
            if (other.CompareTag("Player"))
            {
                inInteractionArea = isEntered;
            }
        }
        else
        {
            inInteractionArea = isEntered;
        }
    }
}
