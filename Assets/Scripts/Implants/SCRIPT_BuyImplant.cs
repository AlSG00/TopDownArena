using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Возможно, этот скрипт вообще больше не нужен
public class SCRIPT_BuyImplant : MonoBehaviour
{
    public int moneyPrice = 0;
    public int pillsPrice = 0;

    public GameObject implant;

    public SCRIPT_PlayerResources _playerResource;
    public SCRIPT_ImplantUpgrades _playerImplants;


    public void ClickBuyButton()
    { 
        if (_playerResource.TakeMoney(moneyPrice) &&
            _playerResource.TakePills(pillsPrice))
        {
            _playerImplants.ActivateNewImplant(implant);
        }
    }
    
}
