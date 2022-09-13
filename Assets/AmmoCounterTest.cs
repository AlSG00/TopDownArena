using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterTest : MonoBehaviour
{
    public Text ammoCounter;

    //private void Start()
    //{
    //    ammoCounter = GetComponent<Text>();
    //}

    public void SetCurrentAmmo(int ammo, int stock)
    {
        ammoCounter.text = $"{ammo}/{stock}";
    }

    //public void SetMaxHealth(float health)
    //{

    //    healtBar.maxValue = health;
    //    healtBar.value = health;
    //}

    //public void SetHealth(float health)
    //{
    //    healtBar.value = health;
    //}
}
