using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerWakefulness : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SCRIPT_SliderBarController _wakefulnessBar;
    [SerializeField] private SCRIPT_PlayerSanity _sanity;

    [Header("Wakefulness parameters")]
    public float maxWakefulness = 100f;
    public float currentWakefulness = 100f;
    public float wakefulnessDecreaseValue = 0.001f;

    [Header("Affection on player")]
    public float sanityDecreaseDebuff = 0.001f;

    public bool isTired = false;


    private void Start()
    {
        _wakefulnessBar.SetMaxValue(maxWakefulness);
    }

    private void Update()
    {
        HandleWakefulness();
        HandleTirednessFlag();
    }

    private void HandleWakefulness()
    {
        if (currentWakefulness > 0)
        {
            currentWakefulness -= wakefulnessDecreaseValue;
        }
        else
        {
            currentWakefulness = 0;
        }

        _wakefulnessBar.SetValue(currentWakefulness);
    }

    private void HandleTirednessFlag()
    {
        if (currentWakefulness > 0)
        {
            if (isTired)
            {
                isTired = false;
                _sanity.sanityDecreaseDebuff += sanityDecreaseDebuff;
            }
        }
        else
        {
            if (!isTired)
            {
                isTired = true;
                _sanity.sanityDecreaseDebuff -= sanityDecreaseDebuff;
            }
        }
    }

    // TODO: Это тестовая функция, переделать так, чтобы бодрость зависела
    // от количества времени, потраченного на сон
    private void Sleep()
    {
        currentWakefulness = maxWakefulness;
        _wakefulnessBar.SetValue(currentWakefulness);
    }
}
