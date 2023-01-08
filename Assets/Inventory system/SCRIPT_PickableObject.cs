using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_PickableObject : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }
    public AudioClip pickUpAudio;
    public AudioSource pickUpAudioSource;
    public GameObject inventoryPrefab;
    private SCRIPT_InventoryController inventory;
    public Image pickableImagePrefab;
    public Image pickableImage;

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

    //private void OnMouseOver()
    //{

    //}

    //private void OnMouseEnter()
    //{
    //}

    //private void OnMouseExit()
    //{
    //    Destroy(pickableImage);
    //}

    private void Update()
    {
        if (pickableImage != null)
        {
            pickableImage.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        }
    }

    public void ShowInteractionIcon()
    {
        if (pickableImage == null && inInteractionArea && canInteract)
        {
            pickableImage = Instantiate(pickableImagePrefab, 
                Camera.main.WorldToScreenPoint(gameObject.transform.position), 
                Quaternion.identity, 
                GameObject.Find("_HUD").transform)
                .GetComponent<Image>();
        }
    }

    public void RemoveInteractionIcon()
    {
        if (pickableImage != null)
        {
            Destroy(pickableImage.gameObject);
        }
    }

    public void HighlightIconWithScanner()
    {
        if (pickableImage == null)
        {
            pickableImage = Instantiate(pickableImagePrefab,
                            Camera.main.WorldToScreenPoint(gameObject.transform.position),
                            Quaternion.identity,
                            GameObject.Find("_HUD").transform)
                            .GetComponent<Image>();

            StopAllCoroutines();
            StartCoroutine(RemoveHighlightediconRoutine());
        }
    }

    private IEnumerator RemoveHighlightediconRoutine()
    {
        yield return pickableImage.color = new Color(
                pickableImage.color.r,
                pickableImage.color.g,
                pickableImage.color.b,
                1f
                );

        yield return new WaitForSeconds(10f);

        while(pickableImage.color.a > 0)
        {
            yield return pickableImage.color = new Color(
                pickableImage.color.r,
                pickableImage.color.g,
                pickableImage.color.b,
                pickableImage.color.a - 0.01f
                );
        }

        yield return pickableImage.color = new Color(
                pickableImage.color.r,
                pickableImage.color.g,
                pickableImage.color.b,
                0f
                );

        Destroy(pickableImage.gameObject);
    }
}
