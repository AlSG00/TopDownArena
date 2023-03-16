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

    //public int warningBlinksCount = 0; // „исло морганий иконки при по€влении предупреждени€

    public float highIndicationValue;
    public float mediumIndicationValue;
    public float lowIndicationValue;

    public StateValue currentStateValueRange;
    public StateValue previousStateValueRange;

    public Color maxStateValueColor;
    public Color normalStateValueColor;
    public Color lowStateValueColor;

    [Header("Status change notification parameters")]
    [SerializeField][Range(0, 1)] private float _notificationAppearingSpeed;
    [SerializeField][Range(0, 1)] private float _notificationDisappearingSpeed;
    [SerializeField] private float _notificationShowDuration;

    [Header("Status warning notification parameters")]
    [SerializeField][Range(0, 1)] private float _warningAppearingSpeed;
    [SerializeField][Range(0, 1)] private float _warningDisappearingSpeed;
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

    // TODO: »зменени€ прозрачности вынеси в отдельную компоненту
    private void EnableIcon()
    {
        _iconElements[0].color = new Color(
            _iconElements[0].color.r,
            _iconElements[0].color.g,
            _iconElements[0].color.b,
            1);

        _iconElements[1].color = new Color(
            _iconElements[1].color.r,
            _iconElements[1].color.g,
            _iconElements[1].color.b,
            1);

        _iconElements[2].color = new Color(
            _iconElements[2].color.r,
            _iconElements[2].color.g,
            _iconElements[2].color.b,
            1);
    }

    private void DisableIcon()
    {
        _iconElements[0].color = new Color(
            _iconElements[0].color.r,
            _iconElements[0].color.g,
            _iconElements[0].color.b,
            0);

        _iconElements[1].color = new Color(
            _iconElements[1].color.r,
            _iconElements[1].color.g,
            _iconElements[1].color.b,
            0);

        _iconElements[2].color = new Color(
            _iconElements[2].color.r,
            _iconElements[2].color.g,
            _iconElements[2].color.b,
            0);
    }

    public IEnumerator ShowStateNotificationRoutine()
    {
        //EnableIcon();

        yield return _iconElements[0].color = new Color(
            _iconElements[0].color.r,
            _iconElements[0].color.g,
            _iconElements[0].color.b,
            0);

        yield return _iconElements[1].color = new Color(
            _iconElements[1].color.r,
            _iconElements[1].color.g,
            _iconElements[1].color.b,
            0);

        yield return _iconElements[2].color = new Color(
            _iconElements[2].color.r,
            _iconElements[2].color.g,
            _iconElements[2].color.b,
            0);

        while (_iconElements[0].color.a < 1)
        {
            yield return _iconElements[0].color = new Color(
                _iconElements[0].color.r,
                _iconElements[0].color.g,
                _iconElements[0].color.b,
                _iconElements[0].color.a + _notificationAppearingSpeed);

            yield return _iconElements[1].color = new Color(
                _iconElements[1].color.r,
                _iconElements[1].color.g,
                _iconElements[1].color.b,
                _iconElements[1].color.a + _notificationAppearingSpeed);

            yield return _iconElements[2].color = new Color(
                _iconElements[2].color.r,
                _iconElements[2].color.g,
                _iconElements[2].color.b,
                _iconElements[2].color.a + _notificationAppearingSpeed);
        }

        yield return _iconElements[0].color = new Color(
            _iconElements[0].color.r,
            _iconElements[0].color.g,
            _iconElements[0].color.b,
            1);

        yield return _iconElements[1].color = new Color(
            _iconElements[1].color.r,
            _iconElements[1].color.g,
            _iconElements[1].color.b,
            1);

        yield return _iconElements[2].color = new Color(
            _iconElements[2].color.r,
            _iconElements[2].color.g,
            _iconElements[2].color.b,
            1);

        yield return new WaitForSeconds(_notificationShowDuration);

        while (_iconElements[0].color.a > 0)
        {
            yield return _iconElements[0].color = new Color(
                _iconElements[0].color.r,
                _iconElements[0].color.g,
                _iconElements[0].color.b,
                _iconElements[0].color.a - _notificationDisappearingSpeed);

            yield return _iconElements[1].color = new Color(
                _iconElements[1].color.r,
                _iconElements[1].color.g,
                _iconElements[1].color.b,
                _iconElements[1].color.a - _notificationDisappearingSpeed);

            yield return _iconElements[1].color = new Color(
                _iconElements[2].color.r,
                _iconElements[2].color.g,
                _iconElements[2].color.b,
                _iconElements[2].color.a - _notificationDisappearingSpeed);

        }

        yield return _iconElements[0].color = new Color(
            _iconElements[0].color.r,
            _iconElements[0].color.g,
            _iconElements[0].color.b,
            0);

        yield return _iconElements[1].color = new Color(
            _iconElements[1].color.r,
            _iconElements[1].color.g,
            _iconElements[1].color.b,
            0);

        yield return _iconElements[2].color = new Color(
            _iconElements[2].color.r,
            _iconElements[2].color.g,
            _iconElements[2].color.b,
            0);

        //DisableIcon();
    }

    // TODO: ”станавливать нужный цвет в начале корутины
    // TODO: ѕресет нужных цветов задавать через инспектор

    public IEnumerator ShowStateWarningRoutine()
    {
        //yield return _iconElements[0].color = new Color(
        //    _iconElements[0].color.r,
        //    _iconElements[0].color.g,
        //    _iconElements[0].color.b,
        //    0);

        yield return _iconElements[0].color = new Color(255, 0, 0, 0);

        yield return _iconElements[1].color = new Color(
            _iconElements[1].color.r,
            _iconElements[1].color.g,
            _iconElements[1].color.b,
            0);

        yield return _iconElements[2].color = new Color(
            _iconElements[2].color.r,
            _iconElements[2].color.g,
            _iconElements[2].color.b,
            0);

        for (int i = 0; i < _warningBlinksCount; i++)
        {
            while (_iconElements[0].color.a < 1)
            {
                /*yield return*/ _iconElements[0].color = new Color(
                    _iconElements[0].color.r,
                    _iconElements[0].color.g,
                    _iconElements[0].color.b,
                    _iconElements[0].color.a + _warningAppearingSpeed);

                /*yield return*/ _iconElements[1].color = new Color(
                    _iconElements[1].color.r,
                    _iconElements[1].color.g,
                    _iconElements[1].color.b,
                    _iconElements[1].color.a + _warningAppearingSpeed);

                /*yield return*/ _iconElements[2].color = new Color(
                    _iconElements[2].color.r,
                    _iconElements[2].color.g,
                    _iconElements[2].color.b,
                    _iconElements[2].color.a + _warningAppearingSpeed);

                yield return null;
            }

            yield return _iconElements[0].color = new Color(
            _iconElements[0].color.r,
            _iconElements[0].color.g,
            _iconElements[0].color.b,
            1);

            yield return _iconElements[1].color = new Color(
                _iconElements[1].color.r,
                _iconElements[1].color.g,
                _iconElements[1].color.b,
                1);

            yield return _iconElements[2].color = new Color(
                _iconElements[2].color.r,
                _iconElements[2].color.g,
                _iconElements[2].color.b,
                1);

            while (_iconElements[0].color.a > 0)
            {
                yield return _iconElements[0].color = new Color(
                    _iconElements[0].color.r,
                    _iconElements[0].color.g,
                    _iconElements[0].color.b,
                    _iconElements[0].color.a - _warningDisappearingSpeed);

                yield return _iconElements[1].color = new Color(
                    _iconElements[1].color.r,
                    _iconElements[1].color.g,
                    _iconElements[1].color.b,
                    _iconElements[1].color.a - _warningDisappearingSpeed);

                yield return _iconElements[1].color = new Color(
                    _iconElements[2].color.r,
                    _iconElements[2].color.g,
                    _iconElements[2].color.b,
                    _iconElements[2].color.a - _warningDisappearingSpeed);

            }

            yield return _iconElements[0].color = new Color(
           _iconElements[0].color.r,
           _iconElements[0].color.g,
           _iconElements[0].color.b,
           0);

            yield return _iconElements[1].color = new Color(
                _iconElements[1].color.r,
                _iconElements[1].color.g,
                _iconElements[1].color.b,
                0);

            yield return _iconElements[2].color = new Color(
                _iconElements[2].color.r,
                _iconElements[2].color.g,
                _iconElements[2].color.b,
                0);
        }

        while (_iconElements[0].color.a < 1)
        {
            yield return _iconElements[0].color = new Color(
                _iconElements[0].color.r,
                _iconElements[0].color.g,
                _iconElements[0].color.b,
                _iconElements[0].color.a + _warningAppearingSpeed);

            yield return _iconElements[1].color = new Color(
                _iconElements[1].color.r,
                _iconElements[1].color.g,
                _iconElements[1].color.b,
                _iconElements[1].color.a + _warningAppearingSpeed);

            yield return _iconElements[2].color = new Color(
                _iconElements[2].color.r,
                _iconElements[2].color.g,
                _iconElements[2].color.b,
                _iconElements[2].color.a + _warningAppearingSpeed);
        }

        yield return _iconElements[0].color = new Color(
        _iconElements[0].color.r,
        _iconElements[0].color.g,
        _iconElements[0].color.b,
        1);

        yield return _iconElements[1].color = new Color(
            _iconElements[1].color.r,
            _iconElements[1].color.g,
            _iconElements[1].color.b,
            1);

        yield return _iconElements[2].color = new Color(
            _iconElements[2].color.r,
            _iconElements[2].color.g,
            _iconElements[2].color.b,
            1);

        yield return new WaitForSeconds(_warningShowDuration);

        while (_iconElements[0].color.a > 0)
        {
            yield return _iconElements[0].color = new Color(
                _iconElements[0].color.r,
                _iconElements[0].color.g,
                _iconElements[0].color.b,
                _iconElements[0].color.a - _warningDisappearingSpeed);

            yield return _iconElements[1].color = new Color(
                _iconElements[1].color.r,
                _iconElements[1].color.g,
                _iconElements[1].color.b,
                _iconElements[1].color.a - _warningDisappearingSpeed);

            yield return _iconElements[1].color = new Color(
                _iconElements[2].color.r,
                _iconElements[2].color.g,
                _iconElements[2].color.b,
                _iconElements[2].color.a - _warningDisappearingSpeed);

        }

        yield return _iconElements[0].color = new Color(
            _iconElements[0].color.r,
            _iconElements[0].color.g,
            _iconElements[0].color.b,
            0);

        yield return _iconElements[1].color = new Color(
            _iconElements[1].color.r,
            _iconElements[1].color.g,
            _iconElements[1].color.b,
            0);

        yield return _iconElements[2].color = new Color(
            _iconElements[2].color.r,
            _iconElements[2].color.g,
            _iconElements[2].color.b,
            0);
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
        //GetStateIconColor();
        if (isIncreased)
        {
            if (currentStateValueRange != previousStateValueRange)
            {
                //ShowStateChange();

                StartCoroutine(ShowStateNotificationRoutine());
            }
        }
        else
        {
            if (currentStateValueRange != previousStateValueRange)
            {
                if (currentStateValueRange == StateValue.Low)
                {
                    //ShowStateWarning();

                    StartCoroutine(ShowStateWarningRoutine());
                }
                else
                {
                    //ShowStateChange();

                    StartCoroutine(ShowStateNotificationRoutine());
                }
            }
        }

        // TODO: ƒописать систему индикации:
        // ¬ы€вл€ть, в какую область попадает текущее значение переменной (низкое значение, среднее или высокое)
        // ¬ы€вл€ть, уменьшилось оно или увеличилось
        // »сход€ из вышеперечисленного определ€ть способ отображение (плавное по€вление или, например, быстрое мерцание, если значение уменьшилось и оказалось в нижней области, а так же цвет индикатора)
        
        // TODO: тут будет метод дл€ определени€, в какую область значений попал индикатор

        


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

    //private void GetStateIconColor()
    //{
    //    switch(currentStateValueRange)
    //    {
    //        case StateValue.Low
    //    }
    //}
}
