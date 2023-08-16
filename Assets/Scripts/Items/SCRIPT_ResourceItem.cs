using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ResourceItem : MonoBehaviour, IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    [SerializeField] private SCRIPT_PlayerResources _playerResources;
    [SerializeField] private AudioSource _playerAudioSource;
    [SerializeField] private AudioClip _interactionAudio;

    [Range(0, 9999)] public int moneyValue = 0;
    [Range(0, 9999)] public int pillsValue = 0;



    private void Awake()
    {
        canInteract = false;
        alreadyInteracting = false;
        inInteractionArea = false;
    }

    private void Start()
    {
        _playerResources = GameObject.Find("_Player").GetComponent<SCRIPT_PlayerResources>();
        _playerAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (_playerAudioSource != null &&
            _interactionAudio != null)
        {
            _playerAudioSource.PlayOneShot(_interactionAudio);
        }

        if (moneyValue > 0)
        {
            _playerResources.AddMoney(moneyValue);
        }
        if (pillsValue > 0)
        {
            _playerResources.AddPills(pillsValue);
        }
        
        Destroy(gameObject);
    }
}
