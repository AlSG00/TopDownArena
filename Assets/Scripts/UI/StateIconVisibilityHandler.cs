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


    public void Initialize(float maxStateValue, float currentStateValue)
    {
        highIndicationValue = maxStateValue;
        mediumIndicationValue = maxStateValue / 2;
        lowIndicationValue = maxStateValue / 5;
        GetValueRange(currentStateValue, currentStateValue);
    }

    private void Awake()
    {
        HideIcon();
    }

    // TODO: »зменени€ прозрачности вынеси в отдельную компоненту
    private void ShowIcon()
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

    private void HideIcon()
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

        yield return _iconElements[0].color = new Color(255, 255, 255, 0);

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

        yield return null;

        while (_iconElements[0].color.a < 1)
        {
            IncreaseImageAlphaColor(ref _iconElements[0], _notificationAppearingSpeed);
            IncreaseImageAlphaColor(ref _iconElements[1], _notificationAppearingSpeed);
            IncreaseImageAlphaColor(ref _iconElements[2], _notificationAppearingSpeed);
            yield return null;
        }

        ShowIcon();

        yield return new WaitForSeconds(_notificationShowDuration);

        while (_iconElements[0].color.a > 0)
        {
            DecreaseImageAlphaColor(ref _iconElements[0], _notificationDisappearingSpeed);
            DecreaseImageAlphaColor(ref _iconElements[1], _notificationDisappearingSpeed);
            DecreaseImageAlphaColor(ref _iconElements[2], _notificationDisappearingSpeed);

            yield return null;
        }

        HideIcon();
    }

    // TODO: ”станавливать нужный цвет в начале корутины
    // TODO: ѕресет нужных цветов задавать через инспектор

    public IEnumerator ShowStateWarningRoutine()
    {
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
                IncreaseImageAlphaColor(ref _iconElements[0], _warningAppearingSpeed);
                IncreaseImageAlphaColor(ref _iconElements[1], _warningAppearingSpeed);
                IncreaseImageAlphaColor(ref _iconElements[2], _warningAppearingSpeed);
                yield return null;
            }

            ShowIcon();
            yield return null;

            while (_iconElements[0].color.a > 0)
            {

                DecreaseImageAlphaColor(ref _iconElements[0], _warningDisappearingSpeed);
                DecreaseImageAlphaColor(ref _iconElements[1], _warningDisappearingSpeed);
                DecreaseImageAlphaColor(ref _iconElements[2], _warningDisappearingSpeed);
                yield return null;
            }

            HideIcon();
            yield return null;
        }

        while (_iconElements[0].color.a < 1)
        {
            IncreaseImageAlphaColor(ref _iconElements[0], _warningAppearingSpeed);
            IncreaseImageAlphaColor(ref _iconElements[1], _warningAppearingSpeed);
            IncreaseImageAlphaColor(ref _iconElements[2], _warningAppearingSpeed);
        }

        ShowIcon();

        yield return new WaitForSeconds(_warningShowDuration);

        while (_iconElements[0].color.a > 0)
        {
            DecreaseImageAlphaColor(ref _iconElements[0], _warningDisappearingSpeed);
            DecreaseImageAlphaColor(ref _iconElements[1], _warningDisappearingSpeed);
            DecreaseImageAlphaColor(ref _iconElements[2], _warningDisappearingSpeed);
            yield return null;
        }

        HideIcon();
    }

    private void IncreaseImageAlphaColor(ref Image image, float increaseValue)
    {
         image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            image.color.a + increaseValue
            );
    }

    private void DecreaseImageAlphaColor(ref Image image, float decreaseValue)
    {
        image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            image.color.a - decreaseValue
            );
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
