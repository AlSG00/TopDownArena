using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_PickableWeapon : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    public SCRIPT_Weapon weaponPrefab;
    SCRIPT_ActiveWeapon activeWeapon;

    [Header("Image for scaned state")]
    public Image pickableImagePrefab;
    private Image _pickableImage;
    public float highlightTime = 5f;

    private void Awake()
    {
        canInteract = false;
        alreadyInteracting = false;
        inInteractionArea = false;
    }

    private void Start()
    {
        activeWeapon = GameObject.Find("_Player").GetComponent<SCRIPT_ActiveWeapon>();
    }

    public void Interact()
    {
        if (activeWeapon)
        {
            SCRIPT_Weapon weaponToPickup = Instantiate(weaponPrefab);
            activeWeapon.Equip(weaponToPickup);
            Destroy(gameObject);
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
        while (_pickableImage.color.a < 1)
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

        while (_pickableImage.color.a > 0)
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


    //private void OnTriggerEnter(Collider other)
    //{
    //    SCRIPT_ActiveWeapon activeWeapon = other.GetComponent<SCRIPT_ActiveWeapon>();

    //    if (activeWeapon)
    //    {
    //        {
    //            SCRIPT_Weapon weaponToPickup = Instantiate(weaponPrefab);
    //            activeWeapon.Equip(weaponToPickup);
    //        }
    //        //Player_Shooting player = other.GetComponent<Player_Shooting>();
    //        // player.Equip(weaponToPickup);
    //    }
    //}
}
