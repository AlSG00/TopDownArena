using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }
    public abstract void Interact();

    // TODO: Наверное, это можно убрать отсюда
    //public virtual void HighlightIconWithScanner() { }
}
