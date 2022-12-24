using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SCRIPT_ObjectInteraction : MonoBehaviour
{
    
    RaycastHit[] hits;
    Ray ray;
    public LayerMask cursorLayer;
    //public LayerMask interactionLayer;
    public float interactionDistance = 1f;
    float currentDistance;
    public GameObject player;
    public SCRIPT_PickableObject pickableObject;
    public SCRIPT_IInteractable interactableObject;
    public SCRIPT_TEST_ActivateShop shop; 

    private void Awake()
    {
        player = GameObject.Find("_Player");
    }

    private void Update()
    {
        MouseCursorRaycast();

        if (Input.GetKeyDown(KeyCode.E))
        {
            ItemInteract();
        }
    }

    private void MouseCursorRaycast()
    {
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //hits = Physics.RaycastAll(ray);
        //// for (int i = 0; i < hits.Length; i++
        //for (int i = hits.Length - 1; i >= 0; i--)
        //{
        //    Debug.Log(hits[i].transform.name);
        //    if (hits[i].transform.CompareTag("Interactable"))
        //    {
        //        if (interactableObject != null)
        //        {
        //            interactableObject.canInteract = false;
        //        }

        //        interactableObject = hits[i].transform.GetComponent<SCRIPT_IInteractable>();
        //        interactableObject.canInteract = true;
        //        break;
        //    }
        //    //else if (hits.All(s => !s.transform.CompareTag("Interactable")))
        //    //{
        //    //    if (interactableObject != null)
        //    //    {
        //    //        interactableObject.canInteract = false;
        //    //    }

        //    //    interactableObject = null;
        //    //}
        //    else
        //    {
        //        hits = null;
        //        if (interactableObject != null)
        //        {
        //            interactableObject.canInteract = false;
        //        }

        //        interactableObject = null;
        //    }
        //}

        //Debug.Log(hits.Length);

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, cursorLayer))
        {
            Debug.Log("Raycasting...");
            if (hit.transform.CompareTag("Interactable"))
            {
                Debug.Log($"Found interactable {hit.transform.name}");
                if (interactableObject != null)
                {
                    interactableObject.canInteract = false;
                }
                interactableObject = hit.transform.GetComponent<SCRIPT_IInteractable>();
                interactableObject.canInteract = true;
            }
            else
            {
                Debug.Log($"Not interactable {hit.transform.name}");
            }
            //interactableObject = hits[i].transform.GetComponent<SCRIPT_IInteractable>();

            // break;
        }
        //else
        //{
        //    Debug.Log($"Not interactable {hit.transform.name}");
        //    hits = null;
        //    if (interactableObject != null)
        //    {
        //        interactableObject.canInteract = false;
        //    }

        //    interactableObject = null;
        //}
        // }
    }

    private void ItemInteract()
    {
        if (interactableObject == null ||
            interactableObject.canInteract == false ||
            interactableObject.alreadyInteracting == true)
        {
            return;
        }

        interactableObject.alreadyInteracting = true;
        interactableObject.Interact();
        //interactableObject = null;
    }
}
