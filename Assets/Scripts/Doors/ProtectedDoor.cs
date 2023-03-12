using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutomaticDoor))]
public class ProtectedDoor : MonoBehaviour
{
    // «акидываетс€ только на двери, которые должны быть изначально заперты

    public AccessType.Type accessType;
    [SerializeField] private EyeAccessImplant accessImplant;
    // public ссылка на коллекцию достпов игрока

    private void Awake()
    {
        
    }

    public bool CheckAccess()
    {
        //accessImplant = GameObject.Find("_Player").GetComponent<EyeAccessImplant>();
        if (accessImplant != null)
        {
            if (accessImplant.AccessCollection.Contains(accessType))
            {
                return true;
            }
        }

        return false;
    }
}
