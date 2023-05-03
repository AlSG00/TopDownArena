using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerCarryingWeight : MonoBehaviour
{
    // скрипт со стаминой добавить
    [SerializeField] private SCRIPT_PlayerHydration _hydration;
    [SerializeField] private Player_Movement _movement;
    [SerializeField] private SCRIPT_CarryingWeightText _carryingWeightUI;

    public float maxCarryingWeight;
    public float currentCarryingWeight;
    public float hydrationDecreaseDebuff = 0.01f;
    public bool _isOvercarried = false;

    private float _hideTime;

    private void Start()
    {
        _carryingWeightUI.SetWeightText(currentCarryingWeight, maxCarryingWeight);
        _carryingWeightUI.HideUI();
    }

    private void Update()
    {
        HandleUIVisibility();
        HandleCaryingWeight();
        CalculateWalkSpeedDebuff();
    }

    private void HandleUIVisibility()
    {
        if (_hideTime + _carryingWeightUI.uiDissapearingDelay < Time.time &&
            _carryingWeightUI.isVisible)
        {
            _carryingWeightUI.HideUI();
        }
    }

    private void HandleCaryingWeight()
    {
        // здесь же прописать дебафф скорости и выносливости
        if (currentCarryingWeight > maxCarryingWeight)
        {
            if (!_isOvercarried)
            {
                _isOvercarried = true;
                _hydration.hydrationDecreaseValue += hydrationDecreaseDebuff;
            }
        }
        else
        {
            if (_isOvercarried)
            {
                _isOvercarried = false;
                _hydration.hydrationDecreaseValue += hydrationDecreaseDebuff;
            }
        }
    }

    public void AddWeight(float weight)
    {
        currentCarryingWeight += weight;
        _carryingWeightUI.SetWeightText(currentCarryingWeight, maxCarryingWeight);
        _carryingWeightUI.ShowUI();
        _hideTime = Time.time;
    }

    public void TakeWeight(float weight)
    {
        currentCarryingWeight -= weight;
        _carryingWeightUI.SetWeightText(currentCarryingWeight, maxCarryingWeight);
        _carryingWeightUI.ShowUI();
        _hideTime = Time.time;
    }
    
    private void CalculateWalkSpeedDebuff()
    {
        if (_isOvercarried)
        {
            _movement.walkSpeedDebuff = (currentCarryingWeight - maxCarryingWeight) * 0.1f;
            
            if (_movement.walkSpeedDebuff >= _movement.walkSpeed)
            {
                _movement.walkSpeedDebuff = _movement.walkSpeed;
            }
        }
        else
        {
            _movement.walkSpeedDebuff = 0f;
        }
    }
}
