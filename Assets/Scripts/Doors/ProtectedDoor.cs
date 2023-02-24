using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedDoor : MonoBehaviour
{
    // «акидываетс€ только на двери, которые должны быть изначально заперты

    public AccessType.Type accesType;
    [SerializeField] private EyeAccessImplant accessImplant;
    // public ссылка на коллекцию достпов игрока

    public bool CheckAccess()
    {
        if (accessImplant.AccessCollection.Contains(accesType))
        {
            return true;
        }

        return false;
    }
}
