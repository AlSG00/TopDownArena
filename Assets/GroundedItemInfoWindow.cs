using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundedItemInfoWindow : MonoBehaviour
{
    [SerializeField] private Vector3 _windowOffset;
    [SerializeField] private Image _window;

    private Vector3? _lastPosition;

    private void OnEnable()
    {
        GroundedItemInfoHandler.OnShow += Show;
        GroundedItemInfoHandler.OnHide += Hide;
    }

    private void OnDisable()
    {
        GroundedItemInfoHandler.OnShow -= Show;
        GroundedItemInfoHandler.OnHide -= Hide;
    }

    private void LateUpdate()
    {
        if (_lastPosition != null)
        {
            _window.rectTransform.position = Camera.main.WorldToScreenPoint((Vector3)_lastPosition);
        }
    }

    private void Show(Vector3 position)
    {
        position = new Vector3(
            position.x + _windowOffset.x,
            position.y + _windowOffset.y,
            position.z
            );
        _lastPosition = position;
        _window.rectTransform.position = Camera.main.WorldToScreenPoint(position);
        _window.enabled = true;
        Debug.Log("Enabled");
    }

    private void Hide(Vector3 position)
    {
        Debug.Log("Disabled");
        _lastPosition = null;
        _window.enabled = false;
    }
}
