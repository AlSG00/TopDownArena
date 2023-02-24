using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAccessImplant : MonoBehaviour, IImplant
{
    public List<AccessType.Type> AccessCollection = new List<AccessType.Type>();

    public bool isActive = false;

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public void AddAccessType(AccessType.Type accessType)
    {
        AccessCollection.Add(accessType);
    }

    public void RemoveAccessType(AccessType.Type accessType)
    {
        AccessCollection.Remove(accessType);
    }
}
