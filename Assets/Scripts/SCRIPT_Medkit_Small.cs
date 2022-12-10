using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_Medkit_Small : MonoBehaviour, SCRIPT_IItem
{
    //public GameObject prefab { get; set; }
    public float healingBonus = 10f;
    PlayerHealth playerHealth;


    private void Awake()
    {
        playerHealth = GameObject.Find("_Player").GetComponent<PlayerHealth>();
    }

    public void Use()
    {
        playerHealth.Heal(healingBonus, true);
    }
}
