using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ImplantUpgrades : MonoBehaviour
{
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private SCRIPT_PlayerCarryingWeight _carryingWeight;
    [SerializeField] private SCRIPT_PlayerStamina _stamina;
    [SerializeField] private Player_Movement _movement;
    [SerializeField] private SCRIPT_PlayerSatiety _satiety;
    [SerializeField] private SCRIPT_PlayerHydration _hydration;
    [SerializeField] private SCRIPT_PlayerWakefulness _wakefulness;
    [SerializeField] private SCRIPT_AreaScanner _scanner;

    public List<GameObject> implantList = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < implantList.Count; i++)
        {
            Debug.Log($"Activating {implantList[i].name}");
            implantList[i].GetComponent<IImplant>().Activate();
        }
    }

    // TODO: Сделать, чтобы коллекция имплантов хранилась в scriptable objects
    // TODO: Переименовать скрипт, он вносит дезу
    public void ActivateNewImplant(GameObject implant)
    {
        implantList.Add(implant);
        implant.GetComponent<IImplant>().Activate();
    }
}
