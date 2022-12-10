using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator controller;
    [SerializeField]
    private Animator controller_2;

    private bool isFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isFinished && other.gameObject.tag == "Player")
        {
            
            controller.SetBool("isActivated", true);
            controller_2.SetBool("isActivated", true);

            isFinished = true;
        }
    }
}
