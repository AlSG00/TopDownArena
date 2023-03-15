using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateIconVisibilityHandler : MonoBehaviour
{
    [SerializeField] private Image[] _iconElements;

    public int warningBlinksCount = 0; // Число морганий иконки при появлении предупреждения

    public float fullIndicationValue;
    public float halfIndicationValue;
    public float quarterIndicationValue;

    public enum StateValue
    {
        Low,
        Medium,
        High
    }

    public void Initialize(float maxStateValue)
    {
        fullIndicationValue = maxStateValue;
        halfIndicationValue = maxStateValue / 2;
        quarterIndicationValue = maxStateValue / 4;
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
        bool isIncreased = SetValueChangeDirectionFlag(currentStateValue, previousStateValue);

        if (isIncreased)
        {

        }
        else
        {

        }

        // TODO: Дописать систему индикации:
        // Выявлять, в какую область попадает текущее значение переменной (низкое значение, среднее или высокое)
        // Выявлять, уменьшилось оно или увеличилось
        // Исходя из вышеперечисленного определять способ отображение (плавное появление или, например, быстрое мерцание, если значение уменьшилось и оказалось в нижней области, а так же цвет индикатора)
        
        // TODO: тут будет метод для определения, в какую область значений попал индикатор

        


        if (isHpIncreased)
        {
            if (currentHealth >= halfHpIndicationValue &&
                oldHealthValue < halfHpIndicationValue)
            {

                _stateIcon.ShowStateChange();
            }
        }
        else
        {
            if (currentHealth < halfHpIndicationValue &&
                oldHealthValue > halfHpIndicationValue)
            {
                _stateIcon.ShowStateChange();
            }
        }
    }

    private bool SetValueChangeDirectionFlag(float currentStateValue, float previousStateValue)
    {
        if (currentStateValue > previousStateValue)
        {
            return true;
        }

        return false;
    }
}
