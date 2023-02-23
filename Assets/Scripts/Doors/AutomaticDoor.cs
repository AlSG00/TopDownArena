using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    [SerializeField] private Animator _animator;

    [SerializeField] private ProtectedDoor _doorProtection;
    // TODO: Переименовать в какую-нибудь InteractiveDoor

    private void Awake()
    {
        canInteract = false;
        alreadyInteracting = false;
        inInteractionArea = false;
    }

    public void Interact()
    {
        if (_doorProtection != null)
        {

        }
        // Открыть дверь
    }

    private void Open()
    {

    }

    private void Close()
    {

    }
}
