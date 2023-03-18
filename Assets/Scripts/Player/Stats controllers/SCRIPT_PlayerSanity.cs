using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerSanity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SCRIPT_SliderBarController _sanityBar;
    [SerializeField] private StateIconVisibilityHandler _stateIcon;

    [Header("Sanity parameters")]
    public float maxSanity = 100f;
    public float currentSanity = 100f;
    public float sanityDecreaseValue = 0f;
    public float sanityDecreaseDebuff = 0f;

    private float previousSanityValue = 0f;

    private void Start()
    {
        _sanityBar.SetMaxValue(maxSanity);
        _stateIcon.Initialize(maxSanity, currentSanity);
    }

    private void Update()
    {
        HandleSanity();
    }

    private void HandleSanity()
    {
        if (currentSanity > 0)
        {
            currentSanity -= sanityDecreaseValue + sanityDecreaseDebuff;
        }
        else
        {
            currentSanity = 0;
        }

        _sanityBar.SetValue(currentSanity);
        _stateIcon.HandleStateIconVisibility(currentSanity, previousSanityValue);
    }
}
