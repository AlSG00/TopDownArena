using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_PickableObject : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    public GameObject inventoryPrefab;

    [Header("References")]
    private SCRIPT_InventoryController inventory;
    public SCRIPT_AreaScanner _scanner;

    [Header("Audio")]
    public AudioClip pickUpAudio;
    public AudioSource pickUpAudioSource;

    [Header("Image for scaned state")]
    public Image pickableImagePrefab;
    private Image _pickableImage;
    public float highlightTime = 5f;

    private void Start()
    {
        alreadyInteracting = false;
        canInteract = false;
        inInteractionArea = false;
        inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        pickUpAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
        _scanner = GameObject.Find("_Player").GetComponent<SCRIPT_AreaScanner>();
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

    private void Update()
    {
        if (_pickableImage != null)
        {
            _pickableImage.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        }
    }

    public void HighlightIconWithScanner()
    {
        if (_pickableImage == null)
        {
            _pickableImage = Instantiate(pickableImagePrefab,
                            Camera.main.WorldToScreenPoint(gameObject.transform.position),
                            Quaternion.identity,
                            GameObject.Find("_HUD").transform)
                            .GetComponent<Image>();

            StopAllCoroutines();
            StartCoroutine(ShowHighlighteddIconRoutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ShowHighlighteddIconRoutine());
        }
    }

    private IEnumerator ShowHighlighteddIconRoutine()
    {
        while(_pickableImage.color.a < 1)
        {
            yield return _pickableImage.color = new Color(
                _pickableImage.color.r,
                _pickableImage.color.g,
                _pickableImage.color.b,
                _pickableImage.color.a + 0.05f
                );
        }

        yield return StartCoroutine(RemoveHighlightedIconRoutine());
    }

        private IEnumerator RemoveHighlightedIconRoutine()
    {
        yield return _pickableImage.color = new Color(
                _pickableImage.color.r,
                _pickableImage.color.g,
                _pickableImage.color.b,
                1f
                );

        yield return new WaitForSeconds(highlightTime);

        while(_pickableImage.color.a > 0)
        {
            yield return _pickableImage.color = new Color(
                _pickableImage.color.r,
                _pickableImage.color.g,
                _pickableImage.color.b,
                _pickableImage.color.a - 0.01f
                );
        }

        yield return _pickableImage.color = new Color(
                _pickableImage.color.r,
                _pickableImage.color.g,
                _pickableImage.color.b,
                0f
                );

        Destroy(_pickableImage.gameObject);
    }
}
