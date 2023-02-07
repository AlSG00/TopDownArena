using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLegsImplant : MonoBehaviour, IImplant
{
    [SerializeField] private SCRIPT_PlayerStamina _stamina;
    [SerializeField] private GameObject[] _implantParts;

    //private void A()
    //{
    //    _stamina = GameObject.Find("_Player").GetComponent<SCRIPT_PlayerStamina>();
    //}

    public void Activate()
    {
        Debug.Log("Activating");
        gameObject.SetActive(true);
        Debug.Log("stamina 0");
        _stamina.staminaDecreaseValue = 0f;
        Debug.Log("activating others");
        for (int i = 0; i < _implantParts.Length; i++)
        {
            _implantParts[i].SetActive(true);
        }
    }

    public void Deactivate()
    {
        
    }
}
