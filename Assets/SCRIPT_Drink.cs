using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_Drink : MonoBehaviour, SCRIPT_IItem
{
    public bool isUsable { get; set; }

    public float hydration = 25f;

    private SCRIPT_PlayerHydration _playerHydration;

    private void Start()
    {
        isUsable = true;
        _playerHydration = GameObject.Find("_Player").GetComponent<SCRIPT_PlayerHydration>();
    }

    public void Use()
    {
        _playerHydration.Drink(hydration);
        Debug.Log("Popil vodichki");
    }
}
