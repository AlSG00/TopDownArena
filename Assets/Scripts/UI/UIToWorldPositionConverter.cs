using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToWorldPositionConverter : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private RectTransform _rect;

    private void FixedUpdate()
    {
        _rect.position = Camera.main.WorldToScreenPoint(_pivot.position);
    }
}
