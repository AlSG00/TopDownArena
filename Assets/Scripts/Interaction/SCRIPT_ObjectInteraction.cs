using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SCRIPT_ObjectInteraction : MonoBehaviour
{ 
    [Header("Cursor raycast settings")]
    [SerializeField] private LayerMask _cursorLayer;
    private Ray _ray;
    private RaycastHit _hit;
    
    private SCRIPT_IInteractable _interactableObject; // Стоит ли убрать это
    //[SerializeField] private Collider _playerInteractionArea;

    [Header("Cursor settings")]
    public Texture2D defaultCursor;

    [Header("Audio")]
    public AudioSource cantInteractAudioSource; // Проигрывать звук, когда наводишься на предмет, с которым можно взаимодействовать
    public AudioClip cantInteractAudio;

    private void Awake()
    {
        SetCursor(defaultCursor);
    }

    private void OnEnable()
    {
        InventoryController.OnUnablePickItem += PlayAudio;
    }

    private void OnDisable()
    {
        InventoryController.OnUnablePickItem -= PlayAudio;
    }

    private void Update()
    {
        MouseCursorRaycast();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void MouseCursorRaycast()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(_ray, out _hit, 100, _cursorLayer))
        {
            if (_hit.transform.CompareTag("Interactable"))
            {
                if (_interactableObject != null)
                {
                    _interactableObject.canInteract = false;
                }
                _interactableObject = _hit.transform.GetComponent<SCRIPT_IInteractable>();

                if (_interactableObject.inInteractionArea)
                {
                    _interactableObject.canInteract = true;
                }
            }
            else
            {
                if (_interactableObject != null)
                {
                    _interactableObject.canInteract = false;
                    _interactableObject = null;
                }
            }
        }
        else
        {
            if (_interactableObject != null)
            {
                _interactableObject.canInteract = false;
                _interactableObject = null;
            }
        }
    }

    private void Interact()
    {
        if (_interactableObject == null ||
            _interactableObject.canInteract == false ||
            _interactableObject.alreadyInteracting == true)
        {
            PlayAudio();
            //Debug.Log($"{_interactableObject}:{_interactableObject.inInteractionArea}:{_interactableObject.canInteract}:{_interactableObject.alreadyInteracting}");
            return;
        }

        _interactableObject.alreadyInteracting = true;
        _interactableObject.Interact();
    }

    private void SetCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void PlayAudio()
    {
        cantInteractAudioSource.PlayOneShot(cantInteractAudio);
    }
}
