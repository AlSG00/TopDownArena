using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ImplantShopMenuController : MonoBehaviour
{
    public Player_Movement _playerMovement;
    public Player_Shooting _playerShooting;
    public SCRIPT_ImplantUpgrades _playerImplants;
    public SCRIPT_PlayerResources _playerResources;
    public bool isOpened;

    public int finalMoneyPrice = 0;
    public int finalPillsPrice = 0;

    public List<SCRIPT_ImplantMenuButton> implantsToBuy = new List<SCRIPT_ImplantMenuButton>();



    private void Start()
    {
        GameObject player = GameObject.Find("_Player");
        _playerMovement = player.GetComponent<Player_Movement>();
        _playerShooting = player.GetComponent<Player_Shooting>();
        _playerImplants = player.GetComponent<SCRIPT_ImplantUpgrades>();
        _playerResources = player.GetComponent<SCRIPT_PlayerResources>();
        player = null;

        CloseMenu();
    }

    public void OpenMenu()
    {
        isOpened = true;

        gameObject.SetActive(true);
        _playerMovement.enabled = false;
        _playerShooting.enabled = false;
    }

    public void CloseMenu()
    {
        isOpened = false;

        if (implantsToBuy.Count != 0)
        {
            for (int i = 0; i < implantsToBuy.Count; i++)
            {
                implantsToBuy[i].Deselect();
            }
        }

        gameObject.SetActive(false);
        _playerMovement.enabled = true;
        _playerShooting.enabled = true;
    }

    public void ExitButtonPress()
    {
        CloseMenu();
    }

    public void SelectImplant(SCRIPT_ImplantMenuButton selectedImplant)
    {
        finalMoneyPrice += selectedImplant.moneyPrice;
        finalPillsPrice += selectedImplant.pillsprice;
        implantsToBuy.Add(selectedImplant);
    }

    public void DeselectImplant(SCRIPT_ImplantMenuButton selectedImplant)
    {
        finalMoneyPrice -= selectedImplant.moneyPrice;
        finalPillsPrice -= selectedImplant.pillsprice;
        implantsToBuy.Remove(selectedImplant);
    }

    public void BuySelectedImplants()
    {
        if (_playerResources.TakeMoney(finalMoneyPrice) &&
            _playerResources.TakePills(finalPillsPrice))
        {
            for (int i = 0; i < implantsToBuy.Count; i++)
            {
                _playerImplants.ActivateNewImplant(implantsToBuy[i].implant);
                implantsToBuy[i].SetAsBuyed();
            }
            implantsToBuy.Clear();
        }
    }
}
