using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateIconVisibilityHandler : MonoBehaviour
{
    //[SerializeField] private SCRIPT_InventoryController inventory;
    public enum StateValue
    {
        Empty,
        Low,
        Medium,
        High,
        Max
    }

    [SerializeField] private Image[] _iconElements;

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
        //GetValueRange(currentStateValue, currentStateValue);
        //previousStateValueRange = currentStateValueRange;

        //Debug.Log($"hg{highIndicationValue} : md{mediumIndicationValue} : lw{lowIndicationValue} - {currentStateValue} : {maxStateValue}");
    }

    private void Awake()
    {
        HideIcon();
    }

    private void OnEnable()
    {
        //SCRIPT_InventoryController.OnInventoryOpened += ShowForInventory;
        //SCRIPT_InventoryController.OnInventoryClosed += HideOnInventoryClosed;

        InventoryController.OnInventoryOpened += ShowForInventory;
        InventoryController.OnInventoryClosed += HideOnInventoryClosed;
    }

    private void OnDisable()
    {
        //SCRIPT_InventoryController.OnInventoryOpened -= ShowForInventory;
        //SCRIPT_InventoryController.OnInventoryClosed -= HideOnInventoryClosed;

        InventoryController.OnInventoryOpened -= ShowForInventory;
        InventoryController.OnInventoryClosed -= HideOnInventoryClosed;
    }

    public bool isInInventory = false;
    private void ShowForInventory()
    {
        isInInventory = true;
        StopAllCoroutines();
        StartCoroutine(SmoothAppearRoutine());
    }

    private void HideOnInventoryClosed()
    {
        //Debug.Log("HideForInventory");
        isInInventory = false;
        StopAllCoroutines();
        StartCoroutine(SmoothDisappearRoutine());
    }

    private IEnumerator SmoothAppearRoutine()
    {
        while (_iconElements[0].color.a < 1)
        {
            IncreaseImageAlphaColor(ref _iconElements[0], _notificationAppearingSpeed);
            IncreaseImageAlphaColor(ref _iconElements[1], _notificationAppearingSpeed);
            IncreaseImageAlphaColor(ref _iconElements[2], _notificationAppearingSpeed);
            //yield return null;
            yield return new WaitForFixedUpdate();
        }

        ShowIcon();
        yield return null;
    }

    private IEnumerator SmoothDisappearRoutine()
    {
        while (_iconElements[0].color.a > 0)
        {
            DecreaseImageAlphaColor(ref _iconElements[0], _notificationDisappearingSpeed);
            DecreaseImageAlphaColor(ref _iconElements[1], _notificationDisappearingSpeed);
            DecreaseImageAlphaColor(ref _iconElements[2], _notificationDisappearingSpeed);

            //yield return null;
            yield return new WaitForFixedUpdate();
        }

        HideIcon();
        yield return null;
    }

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
        //Debug.Log("Notification");
        yield return _iconElements[0].color = new Color(1, 1, 1, 0);

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
            //yield return null;
            yield return new WaitForFixedUpdate();
        }

        ShowIcon();

        yield return new WaitForSeconds(_notificationShowDuration);

        if (isInInventory == false)
        {
            while (_iconElements[0].color.a > 0)
            {
                DecreaseImageAlphaColor(ref _iconElements[0], _notificationDisappearingSpeed);
                DecreaseImageAlphaColor(ref _iconElements[1], _notificationDisappearingSpeed);
                DecreaseImageAlphaColor(ref _iconElements[2], _notificationDisappearingSpeed);

                //yield return null;
                yield return new WaitForFixedUpdate();
            }


            HideIcon();
        }

        _isNotifyActive = false;
    }

    public IEnumerator ShowStateWarningRoutine()
    {
        //Debug.Log("Warning");
        yield return _iconElements[0].color = new Color(1, 0, 0, 0);

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
                //yield return null;
                yield return new WaitForFixedUpdate();
            }

            ShowIcon();
            yield return null;

            while (_iconElements[0].color.a > 0)
            {

                DecreaseImageAlphaColor(ref _iconElements[0], _warningDisappearingSpeed);
                DecreaseImageAlphaColor(ref _iconElements[1], _warningDisappearingSpeed);
                DecreaseImageAlphaColor(ref _iconElements[2], _warningDisappearingSpeed);
                //yield return null;
                yield return new WaitForFixedUpdate();
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

        if (isInInventory == false)
        {
            while (_iconElements[0].color.a > 0)
            {
                DecreaseImageAlphaColor(ref _iconElements[0], _warningDisappearingSpeed);
                DecreaseImageAlphaColor(ref _iconElements[1], _warningDisappearingSpeed);
                DecreaseImageAlphaColor(ref _iconElements[2], _warningDisappearingSpeed);
                //yield return null;
                yield return new WaitForFixedUpdate();
            }

            HideIcon();
        }

        _isWarningActive = false;
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

    private bool _isNotifyActive = false;
    private bool _isWarningActive = false;
    public void HandleStateIconVisibility(float currentStateValue, float previousStateValue)
    {
        bool isIncreased = SetValueChangeDirectionFlag(currentStateValue, previousStateValue);
        previousStateValueRange = currentStateValueRange;
        GetValueRange(currentStateValue, previousStateValue/*, isIncreased*/);

        if (isIncreased)
        {
            if (currentStateValueRange != StateValue.Low)
            {
                _iconElements[0].color = new Color(1, 1, 1, _iconElements[0].color.a);
            }

            if ((currentStateValueRange == StateValue.Max) &&
            _isNotifyActive == false)
            {
                _isNotifyActive = true;
                StopAllCoroutines();
                StartCoroutine(ShowStateNotificationRoutine());
            }
        }
        else
        {
            if (currentStateValueRange != previousStateValueRange &&
                previousStateValueRange != StateValue.Empty)
            {
                //Debug.Log($"{currentStateValueRange} : {previousStateValueRange}");
                if (currentStateValueRange == StateValue.Low &&
                _isWarningActive == false)
                {
                    _isWarningActive = true;
                    StopAllCoroutines();
                    StartCoroutine(ShowStateWarningRoutine());
                }
                else
                {
                    if (_isNotifyActive == false)
                    {
                        _isNotifyActive = true;
                        StopAllCoroutines();
                        StartCoroutine(ShowStateNotificationRoutine());
                    }
                }
            }
        }
        // }
    }

    private bool SetValueChangeDirectionFlag(float currentStateValue, float previousStateValue)
    {
        if (currentStateValue > previousStateValue)
        {
            return true;
        }

        return false;
    }

    private void GetValueRange(float currentStateValue, float previousStateValue/*, bool isIncreased*/)
    {
        if (currentStateValue < lowIndicationValue)
        {
            currentStateValueRange = StateValue.Low;
        }
        else if (currentStateValue >= lowIndicationValue &&
            currentStateValue < mediumIndicationValue)
        {
            currentStateValueRange = StateValue.Medium;
        }
        else if (currentStateValue >= mediumIndicationValue &&
            currentStateValue < highIndicationValue)
        {
            currentStateValueRange = StateValue.High;
        }
        else
        {
            currentStateValueRange = StateValue.Max;
        }
    }
}
