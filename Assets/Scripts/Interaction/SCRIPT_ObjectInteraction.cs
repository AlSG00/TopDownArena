using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SCRIPT_ObjectInteraction : MonoBehaviour
{ 
    private Ray _ray;
    private RaycastHit _hit;
    [SerializeField] private LayerMask _cursorLayer;
    public float interactionDistance = 1f;
    float currentDistance;
    private SCRIPT_IInteractable _interactableObject;
    //public float distanceToObject;
    [SerializeField] private Collider _playerInteractionArea;

    public Texture2D defaultCursor;

    public AudioSource cantInteractAudio; // ѕроигрывать звук, когда наводишьс€ на предмет, с которым можно взаимодействовать
    public AudioClip cantInteracClip;

    private void Awake()
    {
        SetCursor(defaultCursor);
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
            cantInteractAudio.PlayOneShot(cantInteracClip);
            return;
        }

        _interactableObject.alreadyInteracting = true;
        _interactableObject.Interact();
    }

    private void SetCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
    }
}
