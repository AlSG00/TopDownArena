using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject tooltip;

    private void Start()
    {
        tooltip.SetActive(false);
    }

    private void OnMouseEnter()
    {
        tooltip.SetActive(true);
    }

    private void OnMouseExit()
    {
        tooltip.SetActive(false);
    }
}
