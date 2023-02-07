using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ImplantShop : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    public SCRIPT_ImplantShopMenuController _implantMenu;

    //public GameObject ImplantShopMenu;
    // TODO: ссылка на меню прокачки имплантов
    private void Start()
    {
        canInteract = false;
        alreadyInteracting = false;
        inInteractionArea = false;

        _implantMenu = GameObject.Find("ImplantUpgradeMenu").GetComponent<SCRIPT_ImplantShopMenuController>();
        //_playerImplants = GameObject.Find("_Player").GetComponent<SCRIPT_ImplantUpgrades>();
    }

    public void Interact()
    {
        // TODO: Открыть меню прокачки имплантов
        _implantMenu.OpenMenu();
        alreadyInteracting = false;
       // _playerImplants.SetImplant();
    }

    

    
}
