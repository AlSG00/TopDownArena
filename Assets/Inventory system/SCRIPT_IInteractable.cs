using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SCRIPT_IInteractable
{
    public bool alreadyInteracting { get; set; }
    public bool canInteract { get; set; }
    public bool inInteractionArea { get; set; }
    public virtual void Interact() { }
}
