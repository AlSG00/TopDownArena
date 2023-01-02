using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerInteractionArea : MonoBehaviour
{
    SCRIPT_IInteractable interactableObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactableObject = other.GetComponent<SCRIPT_IInteractable>();
            interactableObject.inInteractionArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactableObject = other.GetComponent<SCRIPT_IInteractable>();
            interactableObject.inInteractionArea = false;
        }
    }
}
