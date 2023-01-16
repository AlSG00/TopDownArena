using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestResourceCounter : MonoBehaviour
{
    public SCRIPT_PlayerResources _playerResources;

    public TextMeshProUGUI counter;

    public enum Resource
    {
        Money,
        Pills
    }

    public Resource resourceType;

    private void Start()
    {
        _playerResources = GameObject.Find("_Player").GetComponent<SCRIPT_PlayerResources>();
    }

    private void FixedUpdate()
    {
        if (resourceType == Resource.Money)
        {
            counter.text = _playerResources.GetMoney().ToString();
        }
        else if (resourceType == Resource.Pills)
        {
            counter.text = _playerResources.GetPills().ToString();
        }
    }
}
