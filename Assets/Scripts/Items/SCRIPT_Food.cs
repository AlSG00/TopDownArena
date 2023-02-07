using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_Food : MonoBehaviour, SCRIPT_IItem
{
    public bool isUsable { get; set; }

    public float foodSatiety = 25f;

    private SCRIPT_PlayerSatiety _playerSatiety;

    private void Start()
    {
        isUsable = true;
        _playerSatiety = GameObject.Find("_Player").GetComponent<SCRIPT_PlayerSatiety>();
    }

    public void Use()
    {
        _playerSatiety.Eat(foodSatiety);
        Debug.Log("Vkusno pokushal");
    }
}
