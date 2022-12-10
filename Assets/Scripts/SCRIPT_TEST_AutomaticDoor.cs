using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_TEST_AutomaticDoor : MonoBehaviour
{
    public bool isBlocked = false;
    public float closingDelay = 0f;

    private Animator _animationController;

    private void Awake()
    {
        _animationController = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            !isBlocked)
        {
            _animationController.SetBool("isOpened", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(CloseAfterDelay());
        }
    }

    private IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(closingDelay);

        _animationController.SetBool("isOpened", false);
    }
}
