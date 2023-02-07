using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_SliderBarController : MonoBehaviour
{
    [SerializeField] private Slider _sliderBar;

    public void SetMaxValue(float satiety)
    {
        _sliderBar.maxValue = satiety;
        _sliderBar.value = satiety;
    }

    public void SetValue(float satiety)
    {
        _sliderBar.value = satiety;
    }
}
