using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderLightController : MonoBehaviour
{
    [SerializeField] private Light _light;

    public Color _notificationColor;

    private float _maxRgbValue = 255;
    private float _colorStep = 0f;

    public Color CurrentTestColor;

    public void Initialize(float maxParameterValue)
    {
        //_colorStep = 255 / maxParameterValue;
        _colorStep = 1 / maxParameterValue;
    }

    public void SetHealthColor(float currentValue)
    {
        //float resizedValue = currentValue * _colorStep;

        //if (resizedValue < 0)
        //{
        //    resizedValue = 0;
        //}
        //else if (resizedValue > 255)
        //{
        //    resizedValue = 255;
        //}

        //_light.color = new Color32(
        //    255 - resizedValue,
        //    255 - (255 - resizedValue),
        //    0,
        //    255
        //    );

        //CurrentTestColor = _light.color;

        float resizedValue = currentValue * _colorStep;

        if (resizedValue < 0)
        {
            resizedValue = 0;
        }
        else if (resizedValue > 1)
        {
            resizedValue = 1;
        }

        _light.color = new Color(
            1 - resizedValue,
            1 - (1 - resizedValue),
            0,
            1
            );

        CurrentTestColor = _light.color;
    }
}
