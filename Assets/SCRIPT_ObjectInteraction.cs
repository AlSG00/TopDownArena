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
    public LayerMask interactionLayer;
    public float interactionDistance = 1f;
    float currentDistance;
    public GameObject player;
    public SCRIPT_PickableObject pickableObject;

    private void Awake()
    {
        player = GameObject.Find("_Player");
    }

    void Update()
    {
        MouseCursorRaycast();

        if (Input.GetKey(KeyCode.E))
        {
            ItemInteract();
        }
    }

    private void MouseCursorRaycast()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Item"))
            {
                if (pickableObject != null)
                {
                    pickableObject.canPick = false;

                }
                pickableObject = hits[i].transform.GetComponent<SCRIPT_PickableObject>();
                pickableObject.canPick = true;
                break;
            }
            else if (hits.All(s => !s.transform.CompareTag("Item")))
            {
                if (pickableObject != null)
                {
                    pickableObject.canPick = false;
                }

                pickableObject = null;
            }
        }
    }

    private void ItemInteract()
    {
        if (pickableObject == null ||
            pickableObject.canPick == false ||
            pickableObject.alreadyPicking == true)
        {
            return;
        }
        pickableObject.alreadyPicking = true;
        pickableObject.Pick();
        pickableObject = null;
    }
}
