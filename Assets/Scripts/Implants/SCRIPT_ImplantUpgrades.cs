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
    public void ActivateNewImplant(GameObject implant)
    {
        //implantList.Find(x => x == implant).GetComponent<IImplant>().Activate();
        implantList.Add(implant);
        implant.GetComponent<IImplant>().Activate();
    }

    //[Header("Limbs meshes")]
    //public List<GameObject> headLimbs = new List<GameObject>();
    //public List<GameObject> bodysLimbs = new List<GameObject>();
    //public List<GameObject> handsLimbs = new List<GameObject>();
    //public List<GameObject> legsLimbs = new List<GameObject>();

    //private void Start()
    //{
    //    for (int i = 0; i < handsLimbs.Count; i++)
    //    {
    //        handsLimbs[i].SetActive(false);
    //    }
    //}

    //public void SetImplant()
    //{
    //    for (int i = 0; i < handsLimbs.Count; i++)
    //    {
    //        handsLimbs[i].SetActive(true);
    //    }
    //}
    // Пока не понимаю, зачем данный скрипт

    // Вижу два варианта
    // 1) навешать протезы на игрока и прятать их
    // 2) хранить здесь ссылки на нужные точки на теле, на которых потом спавнить протезы

    //[SerializeField]
    //public class Implant
    //{
    //    GameObject ImplantMesh;
    //    bool isActive;
    //}

    //public List<Implant> implants = new List<Implant>();
}
