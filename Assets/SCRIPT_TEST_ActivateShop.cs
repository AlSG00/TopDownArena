using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_TEST_ActivateShop : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public Transform canSpawner;
    public GameObject canPrefab;
    public SCRIPT_InteractableObjectTrigger interactableTrigger;
    public float canThrowingForce = 100f;
    public float testCooldown = 2f;
    private float cooldown;

    public void Interact()
    {
        if (canInteract == false ||
            interactableTrigger.inInteractionArea == false)
        {
            Debug.Log($"Can't interact {canInteract} {interactableTrigger.inInteractionArea}");
            alreadyInteracting = false;
            return;
        }

        if (testCooldown + cooldown > Time.time)
        {
            Debug.Log($"Can't interact: cooldown");
            alreadyInteracting = false;
            return;
        }

        spawnCan();
    }

    private void spawnCan()
    {
        Debug.Log("Started");
        cooldown = Time.time;
        GameObject can = Instantiate(canPrefab, canSpawner.position, Quaternion.Euler(
            canPrefab.transform.rotation.x,
            /*canPrefab.transform.rotation.y*/90f,
            canPrefab.transform.rotation.z)
            );

        can.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * canThrowingForce);
        alreadyInteracting = false;
        Debug.Log("Finished");
    }
}
