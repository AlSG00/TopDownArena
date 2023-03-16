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

    [Header("Status change notification parameters")]
    [SerializeField] private float _notificationAppearingSpeed;
    [SerializeField] private float _notificationDisappearingSpeed;
    [SerializeField] private float _notificationShowDuration;

    [Header("Status warning notification parameters")]
    [SerializeField] private float _warningAppearingSpeed;
    [SerializeField] private float _warningDisappearingSpeed;
    [SerializeField] private float _warningBlinksCount;
    [SerializeField] private float _warningShowDuration;


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
        StartCoroutine(TestChangeMethod());
    }

    private void ShowStateWarning()
    {
        StartCoroutine(TestWarningMethod());
    }

    public IEnumerator TestChangeMethod()
    {
        EnableIcon();
        yield return new WaitForSeconds(5f);
        DisableIcon();
    }

    //public IEnumerator TestChangeMethod()
    //{
    //    EnableIcon();
    //    yield return new WaitForSeconds(5f);
    //    DisableIcon();
    //}

    public IEnumerator TestWarningMethod()
    {
        EnableIcon();

        yield return new WaitForSeconds(_notificationShowDuration);
        DisableIcon();
    }

    private IEnumerator BlinkIconRoutine()
    {
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
                if (currentStateValueRange == StateValue.Low)
                {
                    ShowStateWarning();
                }
                else
                {
                    ShowStateChange();
                }
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
