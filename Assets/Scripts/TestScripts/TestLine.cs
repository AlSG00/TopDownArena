using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestLine : MonoBehaviour
{
    [SerializeField] private GameObject _fisrt;
    [SerializeField] private GameObject _second;
    [SerializeField] private LineRenderer _line;

    private void Update()
    {
        _line.SetPosition(0, _fisrt.transform.position);
        _line.SetPosition(1, _second.transform.position);
    }
}
