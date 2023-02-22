using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    private void Awake()
    {
        canInteract = false;
        alreadyInteracting = false;
        inInteractionArea = false;
    }

    public void Interact()
    {
        // Открыть дверь
    }
}
