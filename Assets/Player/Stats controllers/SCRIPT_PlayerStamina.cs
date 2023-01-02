using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerStamina : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SCRIPT_SliderBarController _staminaBar;
    [SerializeField] private Player_Movement _movement;
    [SerializeField] private SCRIPT_PlayerHydration _hydration;
    [SerializeField] private SCRIPT_PlayerCarryingWeight _carryingWeight;

    [Header("Stamina parameters")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float staminaDecreaseValue = 0.1f;
    public float staminaIncreaseValue = 0.1f;
    public float staminaRestoringDelay = 3f;

    [Header("Affection on player")]
    public float StaminaDecreaseDebuff = 0.05f;
    [HideInInspector] public float lastTimeRun;

    public enum Tiredness
    {
        Absent,
        Mild,
        Severe,
        Maximum
    }

    public Tiredness tiredness = Tiredness.Absent;

    public bool isExhaused = false;

    private void Start()
    {
        _staminaBar.SetMaxValue(maxStamina);
        _staminaBar.SetValue(currentStamina);
    }

    private void Update()
    {
        HandleStamina();
        HandleStaminaWhenOvercarrying();
        HandleTiredness();
    }
    private void HandleStamina()
    {
        if (_movement.movement.magnitude != 0 &&
            _movement.isRunning)
        {
            lastTimeRun = Time.time;

            if (currentStamina > 0)
            {
                currentStamina -= staminaDecreaseValue;
            }
            else
            { 
                currentStamina = 0;
                if (!isExhaused)
                {
                    isExhaused = true;
                }
            }
        }
        else
        {
            if (lastTimeRun + staminaRestoringDelay <= Time.time)
            {
                if (_hydration.currentHydration > 0 &&
                    !_carryingWeight._isOvercarried)
                {
                    if (currentStamina < maxStamina)
                    {
                        currentStamina += staminaIncreaseValue;
                    }
                    else
                    {
                        currentStamina = maxStamina;
                    }
                }
            }
        }

        _staminaBar.SetValue(currentStamina);
    }

    private void HandleStaminaWhenOvercarrying()
    {
        if (_movement.movement.magnitude != 0 &&
            _carryingWeight._isOvercarried)
        {
            currentStamina -= StaminaDecreaseDebuff;
        }
    }

    private void HandleTiredness()
    {
        if (currentStamina < maxStamina / 4)
        {
            tiredness = Tiredness.Severe;
            
        }
        else if (currentStamina < maxStamina / 2 )
        {
            tiredness = Tiredness.Mild;
            if (isExhaused)
            {
                isExhaused = false;
            }
        }
        else
        {
            tiredness = Tiredness.Absent;
        }
    }
}
