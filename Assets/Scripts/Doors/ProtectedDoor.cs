using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutomaticDoor))]
public class ProtectedDoor : MonoBehaviour
{
    // «акидываетс€ только на двери, которые должны быть изначально заперты

    public AccessType.Type accesType;
    [SerializeField] private EyeAccessImplant accessImplant;
    // public ссылка на коллекцию достпов игрока

    public bool CheckAccess()
    {
        accessImplant = GameObject.Find("_Player").GetComponent<EyeAccessImplant>();
        if (accessImplant.AccessCollection.Contains(accesType))
        {
            return true;
        }

        return false;
    }
}
