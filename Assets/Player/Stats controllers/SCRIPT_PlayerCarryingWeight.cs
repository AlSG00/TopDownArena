using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerCarryingWeight : MonoBehaviour
{
    // скрипт со стаминой добавить
    [SerializeField] private SCRIPT_PlayerHydration _hydration;
    [SerializeField] private Player_Movement _movement;

    public float maxCarryingWeight;
    public float currentCarryingWeight;
    public float hydrationDecreaseDebuff = 0.01f;
    public bool _isOvercarried = false;

    private void Update()
    {
        HandleCaryingWeight();
        CalculateWalkSpeedDebuff();
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

    
    private void CalculateWalkSpeedDebuff()
    {
        if (_isOvercarried)
        {
            _movement.walkSpeedDebuff = (currentCarryingWeight - maxCarryingWeight) * 0.1f;
        }
        else
        {
            _movement.walkSpeedDebuff = 0f;
        }
        
    }
}
