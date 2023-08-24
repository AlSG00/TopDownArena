using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GroundedItemInfoWindow : MonoBehaviour
{
    [SerializeField] private Vector3 _windowOffset;
    [SerializeField] private Image _window;
    [SerializeField] private TextMeshProUGUI itemNameText;

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

    private void Start()
    {
        Hide();
    }

    private void LateUpdate()
    {
        if (_lastPosition != null)
        {
            _window.rectTransform.position = Camera.main.WorldToScreenPoint((Vector3)_lastPosition);
        }
    }

    private void Show(Vector3 position, string itemName)
    {
        position = new Vector3(
            position.x + _windowOffset.x,
            position.y + _windowOffset.y,
            position.z
            );
        _lastPosition = position;
        _window.rectTransform.position = Camera.main.WorldToScreenPoint(position);
        Enable(true);
        itemNameText.text = itemName;
    }

    private void Hide()
    {
        _lastPosition = null;
        Enable(false);
    }

    private void Enable(bool toEnable)
    {
        _window.enabled = toEnable;
        for (int i = 0; i < _window.transform.childCount; i++)
        {
            var child = _window.transform.GetChild(i);
            child.gameObject.SetActive(toEnable);
        }
    }
}
