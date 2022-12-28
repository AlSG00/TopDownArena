using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_SatietyBar : MonoBehaviour
{
    [SerializeField] private Slider _satietyBar;

    public void SetMaxSatiety(float satiety)
    {
        _satietyBar.maxValue = satiety;
        _satietyBar.value = satiety;
    }

    public void SetSatiety(float satiety)
    {
        _satietyBar.value = satiety;    
    }
}
