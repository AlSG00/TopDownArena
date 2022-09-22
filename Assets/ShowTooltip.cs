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
        Debug.Log("Entered");
        tooltip.SetActive(true);
    }

    private void OnMouseExit()
    {
        Debug.Log("Leaved");
        tooltip.SetActive(false);
    }
}
