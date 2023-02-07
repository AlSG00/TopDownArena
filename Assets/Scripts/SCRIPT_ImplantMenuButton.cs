using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_ImplantMenuButton : MonoBehaviour
{
    [SerializeField] private SCRIPT_ImplantShopMenuController _implantMenu;

    public int moneyPrice;
    public int pillsprice;

    public GameObject implant;

    public SCRIPT_PlayerResources _playerResource;
    public SCRIPT_ImplantUpgrades _playerImplants;

    public bool isSelected;

    public Image buttonImage;

    public bool isBuyed = false;

    private void Awake()
    {
        buttonImage = gameObject.GetComponent<Image>();
        _playerResource = GameObject.Find("_Player").GetComponent<SCRIPT_PlayerResources>();
        _playerImplants = GameObject.Find("_Player").GetComponent<SCRIPT_ImplantUpgrades>();
    }

    public void ClickImplantButton()
    {
        if (!isBuyed)
        {
            if (isSelected)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }
    }

    public void Select()
    {
        isSelected = true;
        buttonImage.color = Color.yellow;
        _implantMenu.SelectImplant(this);
    }

    public void Deselect()
    {
        isSelected = false;
        buttonImage.color = Color.white;
        _implantMenu.DeselectImplant(this);
    }

    public void SetAsBuyed()
    {
        Deselect();
        buttonImage.color = Color.green;
        isBuyed = true;
    }

    public void ClickCloseMenuButton()
    {
        _implantMenu.CloseMenu();
    }

    public void ClickBuyButton()
    {
        _implantMenu.BuySelectedImplants();
    }
}
