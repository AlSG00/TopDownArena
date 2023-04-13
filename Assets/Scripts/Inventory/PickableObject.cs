using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableObject : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    //public GameObject inventoryPrefab;
    [Range(1, 999)] public int stackCount;
    

    [Header("References")]
    //private SCRIPT_InventoryController inventory;
    private InventoryController inventory; // TODO: Наверное, заменить на Ивент
    [SerializeField] SCRIPT_InventoryItem inventoryItem;

    [Header("Audio")]
    [SerializeField] private AudioClip pickUpAudio;
    [SerializeField] private AudioSource pickUpAudioSource;

    //public delegate void PickAction();
    //public static event PickAction OnItemPick;

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
        //inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        inventory = GameObject.Find("_PlayerCamera").GetComponent<InventoryController>();
        pickUpAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
    }

    public void Interact()
    {
        canInteract = false;
        Vector2Int? positionOnGrid = inventory.selectedItemGrid.FindSpaceForObject(inventoryItem);
        if (positionOnGrid == null)
        {
            alreadyInteracting = false;
            return;
        }

        inventory.selectedItemGrid = inventory.inventoryGrid;
        int stackCountRemaining = inventory.InsertIntoAvailableStacks(inventoryItem, stackCount);
        if (stackCountRemaining > 0)
        {
            inventoryItem.stackCount = stackCountRemaining;
            inventory.InsertItemIntoInventory(inventoryItem);

            if (pickUpAudioSource != null &&
                pickUpAudio != null)
            {
                pickUpAudioSource.PlayOneShot(pickUpAudio);
            }
        }

        Destroy(gameObject);
    }
}
