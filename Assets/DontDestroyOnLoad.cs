using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    //public DontDestroyOnLoad original;

    ////void Awake()
    ////{
    ////    //if (instance != null && instance != this)
    ////    //    Destroy(this.gameObject);
    ////    //else
    ////    //{
    ////    //    instance = this;
    ////    //    DontDestroyOnLoad(this.gameObject);
    ////    //}

    ////    //if (instance == this)
    ////    //    Destroy(this.gameObject);
    ////    //else if (instance == null)
    ////    //{
    ////    //    instance = this;
    ////        DontDestroyOnLoad(this.gameObject);
    ////    //}

    ////}

    //  private static Dictionary<string, GameObject> _instances = new Dictionary<string, GameObject>();
    //  public string ID; // HACK: This ID can be pretty much anything, as long as you can set it from the inspector

    // void Awake()
    //{
    //if (_instances.ContainsKey(ID))
    //{
    //    var existing = _instances[ID];

    //    // A null result indicates the other object was destoryed for some reason
    //    if (existing != null)
    //    {
    //        if (ReferenceEquals(gameObject, existing))
    //            return;

    //        Destroy(gameObject);

    //        // Return to skip the following registration code
    //        return;
    //    }
    //}

    //// The following code registers this GameObject regardless of whether it's new or replacing
    //_instances[ID] = gameObject;

    //DontDestroyOnLoad(gameObject);

    //private static DontDestroy original;
    //private void Awake()
    //{
    //    //GetComponent<DontDestroyOnLoad>();

    //    if (original != this)
    //    {
    //        if (original != null)
    //            Destroy(original.gameObject);
    //        DontDestroyOnLoad(gameObject);
    //        original = this;
    //    }
    //}
}
//}
