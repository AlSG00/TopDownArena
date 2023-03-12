using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessItem : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    [SerializeField] private EyeAccessImplant _accessImplant;

    public AccessType.Type accessType;

    //private void Awake()
    //{
    //    EyeAccessImplant
    //}

    public void Interact()
    {
        // Проиграть анимацию
        

        _accessImplant.AddAccessType(accessType);

        Debug.Log($"Received access type: {accessType}");
        Destroy(gameObject);
    }
}
