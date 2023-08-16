using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    [Range(1, 999)] public int stackCount;

    [Header("References")]
    public SCRIPT_InventoryItem inventoryItem;


    public delegate int PickAction(SCRIPT_InventoryItem item, int stackCount);
    public static event PickAction OnItemPick;

    private void Awake()
    {
        if (stackCount > inventoryItem.maxStackCount)
        {
            stackCount = inventoryItem.maxStackCount;
        }
        else if (stackCount == 0)
        {
            stackCount = 1;
        }
    }

    private void Start()
    {
        alreadyInteracting = false;
        canInteract = false;
        inInteractionArea = false;
    }

    public void Interact()
    {
        //canInteract = false;
        if (OnItemPick != null)
        {
            stackCount = OnItemPick.Invoke(inventoryItem, stackCount);

            if (stackCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
