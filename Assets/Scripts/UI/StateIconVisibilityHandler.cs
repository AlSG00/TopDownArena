using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateIconVisibilityHandler : MonoBehaviour
{
    [SerializeField] private Image[] _iconElements;

    public int warningBlinksCount = 0; // Число морганий иконки при появлении предупреждения

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



    private void ShowStateChange()
    {

    }

    private void ShowStateWarning()
    {

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
}
