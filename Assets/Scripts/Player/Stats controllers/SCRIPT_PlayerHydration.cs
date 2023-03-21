using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(SCRIPT_PlayerSatiety))]
public class SCRIPT_PlayerHydration : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private SCRIPT_SliderBarController _hydrationBar;
    [SerializeField] private StateIconVisibilityHandler _stateIcon;

    [Header("Hydration parameters")]
    public float maxHydration = 100f;
    public float currentHydration = 100f;
    public float hydrationDecreaseValue = 0.01f;
    public float hydrationDecreaseDebuff = 0f;
    public float satietyDecreaseDebuff = 0.05f; 
    public float healthDecreaseValue = 0.01f;
    private bool _isDehydrated = false;

    private PlayerHealth _health;
    private SCRIPT_PlayerSatiety _satiety;
    private SCRIPT_PlayerStamina _stamina;

    private float previousHydrationValue = 0f;


    private void Awake()
    {
        _health = gameObject.GetComponent<PlayerHealth>();
        _satiety = gameObject.GetComponent<SCRIPT_PlayerSatiety>();
        _stamina = gameObject.GetComponent<SCRIPT_PlayerStamina>();

        previousHydrationValue = currentHydration;
    }

    private void Start()
    {
        _hydrationBar.SetMaxValue(maxHydration);
        _stateIcon.Initialize(maxHydration, currentHydration);
    }

    private void FixedUpdate()
    {
        UpdateHydration();
        CalculateHydrationDebuffByStamina();
    }

    private void UpdateHydration()
    {
        previousHydrationValue = currentHydration;
        if (currentHydration > 0)
        {
            currentHydration -= hydrationDecreaseValue + hydrationDecreaseDebuff;
            if (_isDehydrated)
            {
                _isDehydrated = false;
                _satiety.satietyDecreaseValue -= satietyDecreaseDebuff;
            }
        }
        else
        {
            currentHydration = 0;
            if (!_isDehydrated)
            {
                _isDehydrated = true;
                _health.TakeDamage(healthDecreaseValue);
                _satiety.satietyDecreaseValue += satietyDecreaseDebuff;
            }
        }

        _hydrationBar.SetValue(currentHydration);
        _stateIcon.HandleStateIconVisibility(currentHydration, previousHydrationValue);
    }

    private void CalculateHydrationDebuffByStamina()
    {
        hydrationDecreaseDebuff = (_stamina.maxStamina - _stamina.currentStamina) * 0.0001f;
    }

    public void Drink(float hydration)
    {
        currentHydration += hydration;
        if (currentHydration > maxHydration)
        {
            currentHydration = maxHydration;
        }
        _hydrationBar.SetValue(currentHydration);
        _stateIcon.HandleStateIconVisibility(currentHydration, previousHydrationValue);
    }
}
