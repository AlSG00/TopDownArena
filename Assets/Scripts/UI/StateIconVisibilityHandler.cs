using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateIconVisibilityHandler : MonoBehaviour
{
    public enum StateValue
    {
        Low,
        Medium,
        High
    }

    [SerializeField] private Image[] _iconElements;

    public int warningBlinksCount = 0; // Число морганий иконки при появлении предупреждения

    public float highIndicationValue;
    public float mediumIndicationValue;
    public float lowIndicationValue;

    public StateValue currentStateValueRange;
    public StateValue previousStateValueRange;

    public void Initialize(float maxStateValue)
    {
        highIndicationValue = maxStateValue;
        mediumIndicationValue = maxStateValue / 2;
        lowIndicationValue = maxStateValue / 4;


    }

    private void Awake()
    {
        DisableIcon();
    }

    private void EnableIcon()
    {
        for (int i = 0; i < _iconElements.Length; i++)
        {
            _iconElements[i].enabled = true;
        }
    }

    private void DisableIcon()
    {
        for (int i = 0; i < _iconElements.Length; i++)
        {
            _iconElements[i].enabled = false;
        }
    }

    public void ShowStateChange()
    {
        Debug.Log("ShowStateChange");
        StartCoroutine(TestMethod());
    }

    public IEnumerator TestMethod()
    {
        EnableIcon();
        yield return new WaitForSeconds(5f);
        DisableIcon();
    }

    private void ShowStateWarning()
    {

    }

    private IEnumerator BlinkIconRoutine()
    {
        // for (int i = 0; i < warningBlinksCount)
        yield return null;
    }

    private IEnumerator SmoothAppearingRoutine()
    {
        yield return null;
    }

    private IEnumerator SmoothDissapearingRoutine()
    {
        yield return null;
    }

    public void HandleStateIconVisibility(float currentStateValue, float previousStateValue)
    {

        previousStateValueRange = currentStateValueRange;
        bool isIncreased = SetValueChangeDirectionFlag(currentStateValue, previousStateValue);
        GetValueRange(currentStateValue, previousStateValue);
        //int stateValueRange = GetValueRange(currentStateValue, previousStateValue);
        if (isIncreased)
        {
            if (currentStateValueRange != previousStateValueRange)
            {
                ShowStateChange();
            }
        }
        else
        {
            if (currentStateValueRange != previousStateValueRange)
            {
                // TODO: добавить условие, чтобы, помимо обычного отобржаения, здесь еще вызывалась функция моргания, если значение упало до критического
                ShowStateChange();
            }
        }

        // TODO: Дописать систему индикации:
        // Выявлять, в какую область попадает текущее значение переменной (низкое значение, среднее или высокое)
        // Выявлять, уменьшилось оно или увеличилось
        // Исходя из вышеперечисленного определять способ отображение (плавное появление или, например, быстрое мерцание, если значение уменьшилось и оказалось в нижней области, а так же цвет индикатора)
        
        // TODO: тут будет метод для определения, в какую область значений попал индикатор

        


        //if (isHpIncreased)
        //{
        //    if (currentHealth >= halfHpIndicationValue &&
        //        oldHealthValue < halfHpIndicationValue)
        //    {

        //        _stateIcon.ShowStateChange();
        //    }
        //}
        //else
        //{
        //    if (currentHealth < halfHpIndicationValue &&
        //        oldHealthValue > halfHpIndicationValue)
        //    {
        //        _stateIcon.ShowStateChange();
        //    }
        //}
    }

    private bool SetValueChangeDirectionFlag(float currentStateValue, float previousStateValue)
    {
        if (currentStateValue > previousStateValue)
        {
            return true;
        }

        return false;
    }

    private void GetValueRange(float currentStateValue, float previousStateValue)
    {
        if (currentStateValue < lowIndicationValue)
        {
            currentStateValueRange = StateValue.Low;
        }
        else if (currentStateValue > lowIndicationValue &&
            currentStateValue < mediumIndicationValue)
        {
            currentStateValueRange = StateValue.Medium;
        }
        else
        {
            currentStateValueRange = StateValue.High;
        }
    }
}
