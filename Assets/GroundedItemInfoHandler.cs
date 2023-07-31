using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GroundedItemInfoHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /*When player holding cursor on item lied on the ground, he'll se it's description */
    [SerializeField] private string _itemName;
    TextAsset itemDescription;

    public delegate void showAction(Vector3 position, string itemName);
    public delegate void hideAction();
    public static event showAction OnShow;
    public static event hideAction OnHide;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnShow?.Invoke(transform.position, _itemName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHide?.Invoke();
    }
}
