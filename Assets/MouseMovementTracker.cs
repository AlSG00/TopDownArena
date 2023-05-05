using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovementTracker : MonoBehaviour
{
    public bool isMoving = false;
    public bool isActive = false;
    public float notMovingTimer = 0f;
    private Vector3 lastMousePosition;
    private Vector3 positionDelta;
    private float inactivityTime = 0f;

    private void Awake()
    {
        isMoving = false;
        isActive = false;
        notMovingTimer = 0f;
        inactivityTime = 0f;
    }

    private void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        positionDelta = lastMousePosition - Input.mousePosition;
        if (positionDelta.magnitude > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        lastMousePosition = Input.mousePosition;

        if (isMoving == false)
        {
            aksdfhaisdfugh
        }
    }
}
