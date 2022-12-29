using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerCarryingWeight : MonoBehaviour
{
    // скрипт со стаминой добавить
    [SerializeField] private SCRIPT_PlayerHydration _playerHydration;
    [SerializeField] private Player_Movement _playerMovement;

    public float maxCarryingWeight;
    public float currentCarryingWeight;
    public float hydrationDecreaseDebuff;
    private bool _isOvercarried = false;

    private void Update()
    {
        HandleCaryingWeight();
    }

    private void HandleCaryingWeight()
    {
        // здесь же прописать дебафф скорости и выносливости
        if (currentCarryingWeight > maxCarryingWeight)
        {
            if (!_isOvercarried)
            {
                _isOvercarried = true;
                _playerHydration.hydrationDecreaseValue += hydrationDecreaseDebuff;
            }
        }
        else
        {
            if (_isOvercarried)
            {
                _isOvercarried = false;
                _playerHydration.hydrationDecreaseValue += hydrationDecreaseDebuff;
            }
        }
    }
}
