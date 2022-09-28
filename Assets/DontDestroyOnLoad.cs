using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad singleton { get; private set; }

    private void Awake()
    {
        //if (!singleton)
        //{
        //    singleton = this;
        //    DontDestroyOnLoad(this);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        //if (singleton == null)
        //{
        //    singleton = this;
        //}
        //else if (singleton != this)
        //{
        //    Destroy(gameObject);
        //}
        //gameObject.SetActive(true);

        DontDestroyOnLoad(gameObject);
        
    }
}

