using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLegsImplant : MonoBehaviour, IImplant
{
    [SerializeField] private SCRIPT_PlayerStamina _stamina;
    [SerializeField] private GameObject[] _additionalImplantParts;

    public void Activate()
    {
        gameObject.SetActive(true);
        _stamina.staminaDecreaseValue = 0f;
        for (int i = 0; i < _additionalImplantParts.Length; i++)
        {
            _additionalImplantParts[i].SetActive(true);
        }
    }

    public void Deactivate()
    {
        
    }
}
