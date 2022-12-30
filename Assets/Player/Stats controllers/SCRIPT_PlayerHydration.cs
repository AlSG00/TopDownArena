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

    [Header("Hydration parameters")]
    public float maxHydration = 100f;
    public float currentHydration = 100f;
    public float hydrationDecreaseValue = 0.01f;
    public float satietyDecreaseDebuff = 0.05f; 
    public float healthDecreaseValue = 0.01f;
    private bool _isDehydrated = false;

    private PlayerHealth _health;
    private SCRIPT_PlayerSatiety _satiety;
    private SCRIPT_PlayerStamina _stamina;
    
    private void Awake()
    {
        _health = gameObject.GetComponent<PlayerHealth>();
        _satiety = gameObject.GetComponent<SCRIPT_PlayerSatiety>();
    }

    private void FixedUpdate()
    {
        UpdateHydration();
    }

    private void Start()
    {
        _hydrationBar.SetMaxValue(maxHydration);
    }

    private void UpdateHydration()
    {
        if (currentHydration > 0)
        {
            currentHydration -= hydrationDecreaseValue;
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
    }

    public void Drink(float hydration)
    {
        currentHydration += hydration;
        if (currentHydration > maxHydration)
        {
            currentHydration = maxHydration;
        }
        _hydrationBar.SetValue(currentHydration);
    }
}
