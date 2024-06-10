using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLevelPicker : MonoBehaviour, IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    public void Interact()
    {
        // TODO: Disable player control
        // TODO: Show floor select UI
    }
}
