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

    [Header("Stamina parameters")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float staminaDecreaseValue = 0.1f;
    public float staminaIncreaseValue = 0.1f;
    public float staminaRestoringDelay = 3f;

    //public bool _isTired = false;

    [Header("Affection on player")]

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
                Debug.Log("stamina cooldown check");
                if (_hydration.currentHydration > 0)
                {
                    Debug.Log("hydration check");
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
