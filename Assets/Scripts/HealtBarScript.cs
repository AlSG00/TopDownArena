using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtBarScript : MonoBehaviour
{
    public Slider healtBar;

    public void SetMaxHealth(float health)
    {
        healtBar.maxValue = health;
        healtBar.value = health;
    }

    public void SetHealth(float health)
    {
        healtBar.value = health;
    }
}
