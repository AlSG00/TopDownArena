using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour, IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    [SerializeField] private ProtectedDoor _doorProtection;

    [SerializeField] private Animator _doorAnimator;

    [Header("Audio")]
    [SerializeField] private AudioSource _doorAudioSource;
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _closeSound;
    [SerializeField] private AudioClip _doorLockedSound;
    [SerializeField] private AudioClip _doorUnlockedSound; // Нужно, если я буду сначала отпирать дверь, а затем открывать, но пока не определился, как именно буду делать

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
            if (_doorProtection.CheckAccess())
            {
                Open();
            }
            else
            {
                DenyAccess();
            }
        }
        else
        {
            Open();
        }
    }

    // TODO: заполнить функции ниже звуковыми эффектами, анимациями и т.д.
    private void Open()
    {
        Debug.Log("Door opened");
    }

    private void Close()
    {
        Debug.Log("Door closed");
    }

    private void DenyAccess()
    {
        Debug.Log("Access denied");
    }
}
