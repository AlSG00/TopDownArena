using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovementTracker : MonoBehaviour
{
    public delegate void inactiveMode(bool isActive);
    public static inactiveMode OnCursorInactive;

    //[SerializeField] InventoryController playerInventory;

    public bool isMoving = false;
    public bool isActive = false;
    public float notMovingTimer = 0f;
    private Vector3 lastMousePosition;
    private Vector3 positionDelta;
    private float inactivityTime = 0f;

    private void Awake()
    {
        isMoving = false;
        isActive = true;
    }

    private void OnEnable()
    {
        InventoryController.OnInventoryOpened += ResetFlags;
    }

    private void OnDisable()
    {
        InventoryController.OnInventoryOpened -= ResetFlags;
    }

    private void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        SetMouseMovementFlag();
        SetMouseActivityFlag();
    }

    private void ResetFlags(bool isOpened)
    {
        isMoving = false;
        isActive = true;
        inactivityTime = 0f;
    }

    private void SetMouseActivityFlag()
    {
        if (isMoving == false)
        {
            inactivityTime += Time.deltaTime;
            if (inactivityTime >= notMovingTimer)
            {
                if (isActive)
                {
                    isActive = false;
                    //if (playerInventory.isCheckingInventory)
                    //{
                    OnCursorInactive?.Invoke(isActive);
                    //}
                }
            }
        }
        else
        {
            inactivityTime = 0f;
            if (isActive == false)
            {
                isActive = true;
                OnCursorInactive?.Invoke(isActive);
            }
        }
    }

    private void SetMouseMovementFlag()
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
    }
}
