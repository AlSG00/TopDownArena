using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PlayerResources : MonoBehaviour
{
    [SerializeField] private int _money = 0;

    // Еще не придумал название. Это медицинский препарат, необходимый для прокачки имплантов
    // (типо собери необходимое число препаратов, чтобы тебя ими накачали, иначе у тебя будет неизбежное отторжение)
    [SerializeField] private int _pills = 0;
    
    // TODO: может добавить каких-нибудь еще ресурсов

    public int GetMoney()
    {
        return _money;
    }

    public int GetPills()
    {
        return _pills;
    }

    public void AddMoney(int money)
    {
        _money += money;
    }

    public void AddPills(int pills)
    {
        _pills += pills;
    }

    public bool TakeMoney(int money)
    {
        if (!CheckResources(_money, money))
        {
            return false;
        }

        _money -= money;
        return true;
    }

    public bool TakePills(int pills)
    {
        if (!CheckResources(_pills, pills))
        {
            return false;
        }

        _pills -= pills;
        return true;
    }

    private bool CheckResources(int current, int required)
    {
        if (current < required)
        {
            return false;
        }

        return true;
    }
}
