public interface IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }
    public void Interact();
    public virtual void InteractAndUse() { }
}
