using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_FollowChild : MonoBehaviour
{
    [SerializeField]
    private Transform follow = null;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private void Awake()
    {
        originalLocalPosition = follow.localPosition;
        originalLocalRotation = follow.localRotation;
    }
    private void Update()
    {
        transform.position = follow.position;
    }
}
