using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PickableObject : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }
    public AudioClip pickUpAudio;
    public AudioSource pickUpAudioSource;
    public GameObject inventoryPrefab;
    private SCRIPT_InventoryController inventory;


    private void Start()
    {
        alreadyInteracting = false;
        canInteract = false;
        inInteractionArea = false;
        inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        pickUpAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
    }

    public void Interact()
    {
        canInteract = false;
        Vector2Int? positionOnGrid = inventory.selectedItemGrid.FindSpaceForObject(inventoryPrefab.GetComponent<SCRIPT_InventoryItem>());
        if (positionOnGrid == null)
        {
            alreadyInteracting = false;
            return;
        }

        inventory.selectedItemGrid = inventory.inventoryGrid;
        inventory.InsertItemIntoInventory(gameObject);
        
        if (pickUpAudioSource != null &&
            pickUpAudio != null)
        {
            pickUpAudioSource.PlayOneShot(pickUpAudio);
        }

        Destroy(gameObject);
    }
}
