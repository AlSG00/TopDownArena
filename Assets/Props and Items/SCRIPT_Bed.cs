using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_Bed : MonoBehaviour, SCRIPT_IInteractable
{
    public bool alreadyInteracting { get; set; }
    public bool canInteract { get; set; }
    public bool inInteractionArea { get; set; }

    //[Header("References")]

    [SerializeField] private SCRIPT_PlayerWakefulness _wakefulness;
    //[SerializeField] private SCRIPT_PlayerStamina _stamina;
    //[SerializeField] private SCRIPT_PlayerSatiety _satiety;
    //[SerializeField] private SCRIPT_PlayerHydration _hydration;
    //[SerializeField] private SCRIPT_PlayerSanity _sanity;

    private void Start()
    {
        _wakefulness = GameObject.Find("_Player").GetComponent<SCRIPT_PlayerWakefulness>();
    }

    public void Interact()
    {
        Debug.Log("using bed...");
        _wakefulness.Sleep();
        alreadyInteracting = false;
        Debug.Log("used bed");
    }
}
