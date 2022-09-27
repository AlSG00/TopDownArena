using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    //private void Awake()
    //{
    //    DontDestroyOnLoad(this);
    //}

    public static DontDestroyOnLoad instance;

    void Awake()
    {
        //if (instance != null && instance != this)
        //    Destroy(this.gameObject);
        //else
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}

        if (instance == this)
            Destroy(this.gameObject);
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }
}
