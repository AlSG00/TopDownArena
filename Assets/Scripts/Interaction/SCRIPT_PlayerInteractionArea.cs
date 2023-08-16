using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerInteractionArea : MonoBehaviour
{
    IInteractable interactableObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactableObject = other.GetComponent<IInteractable>();
            interactableObject.inInteractionArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactableObject = other.GetComponent<IInteractable>();
            interactableObject.inInteractionArea = false;
        }
    }
}
