using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GroundedItemInfoHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /*When player holding cursor on item lied on the ground, he'll se it's description */
    string itemName;
    TextAsset itemDescription;

    public delegate void showAction(Vector3 position);
    public static event showAction OnShow;
    public static event showAction OnHide;

    //private void OnMouseEnter()
    //{
    //    OnShow?.Invoke(transform.position);
    //}

    //private void OnMouseExit()
    //{
    //    OnHide?.Invoke(transform.position);
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnShow?.Invoke(transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHide?.Invoke(transform.position);
    }
}
